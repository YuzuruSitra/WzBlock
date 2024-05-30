using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InGame.InGameSystem
{
    public class BoredomMetaAI
    {
        private static BoredomMetaAI _instance;
        public static BoredomMetaAI Instance => _instance ??= new BoredomMetaAI();
        
        private const int Act = 10;
        private const int MaxQValue = 100;
        private const int InitialQValue = 50;
        private const int MaxRewardScore = 100;
        private const int BaseScore = MaxRewardScore / 2;
        private const int InitialRewardValue = 50;
        private const float Alpha = 0.2f;
        private const float Gamma = 0.7f;
        private const float ExplorationRate = 0.3f;
        private const int MaxStackData = 10;
        private const int MaxAvoidBullet = 4;
        private const int MaxReceiveBullet = 2;
        
        // Learning Parameter.
        private readonly List<int> _qValues;
        private int[] _qStackValue = { InitialQValue, InitialQValue};
        private int _currentMaxQ = MaxQValue;
        private int[] _rewardStackValue = { InitialRewardValue, InitialRewardValue};
        private int[] _actionStackValue = { 0, 0};
        
        private readonly List<float> _rallyTimes;

        private readonly System.Random _random;
        // Debug.
        // public int CurrentSelectAction;
        // public int LearnCount;
        // public int CurrentQValue;
        private BoredomMetaAI()
        {
            _qValues = new List<int>();
            _random = new System.Random();
            
            for (var i = 0; i < Act; ++i)
            {
                var addValue = InitialQValue - i;
                _qValues.Add(addValue);
            }
            _rallyTimes = new List<float>();
        }

        public void Learning(float rallyTime, int avoidBulletCount, int receiveBulletCount)
        {
            // Convert calc value.
            var calcActionRate = CalcRallyScore(rallyTime);
            var calcIdleRate = CalcAvoidScore(avoidBulletCount);
            var calcRepeatingActionsRate = CalcReceiveBulletScore(receiveBulletCount);

            // Calc reward.
            CalcReward(calcActionRate, calcIdleRate, calcRepeatingActionsRate);
            // Update Q value.
            CalcQValue();
            SelectAction();
            
            // Debug.
            // LearnCount++;
        }
        
        // Select action for ε - greedy.
        private void SelectAction()
        {
            var bestAction = 0;
            if (_random.NextDouble() < ExplorationRate)
            {
                bestAction = _random.Next(0, Act);
            }
            else
            {
                var maxQValue = _qValues[0];
                for (var i = 1; i < Act; ++i)
                {
                    if (!(_qValues[i] > maxQValue)) continue;
                    maxQValue = _qValues[i];
                    bestAction = i;
                }    
            }
            _actionStackValue = SwapStacks(_actionStackValue, bestAction);
            
            // Debug.
            // CurrentSelectAction = bestAction;
            // CurrentQValue = _qValues[bestAction];
        }
        
        private void CalcQValue()
        {
            var twoBeforeQ = _qStackValue[0];
            var twoBeforeReward = _rewardStackValue[0];
            var oneBeforeMaxQ = _currentMaxQ;
            var calcValue = twoBeforeQ + Alpha * (twoBeforeReward + Gamma * oneBeforeMaxQ - twoBeforeQ);
            var clampQValue = Mathf.Clamp((int)calcValue, 0, MaxQValue);
            
            // Set values.
            var twoBeforeAction = _actionStackValue[0];
            _qValues[twoBeforeAction] = clampQValue;
            _qStackValue = SwapStacks(_qStackValue, clampQValue);
            _currentMaxQ = _qValues.Max();
        }

        private void CalcReward(int rallyRate, int avoidBulletRate, int receiveBulletRate)
        {
            var calcValue = (rallyRate + avoidBulletRate + receiveBulletRate) / 3.0f;
            var clampReward = Mathf.Clamp((int)calcValue, 0, MaxRewardScore) - BaseScore;
            _rewardStackValue = SwapStacks(_rewardStackValue, clampReward);
        }
        
        // Update Stack Value.
        private int[] SwapStacks(int[] stackIndex, int newValue)
        {
            for (var i = 0; i < stackIndex.Length; i++)
                if (i == stackIndex.Length - 1)
                    stackIndex[i] = newValue;
                else
                    stackIndex[i] = stackIndex[i + 1];
            return stackIndex;
        }
        
        // Normalize the training data to scores by deviation.
        private int CalcRallyScore(float rallyTime)
        {
            _rallyTimes.Add(rallyTime);
            if (_rallyTimes.Count >= MaxStackData)
                _rallyTimes.RemoveAt(0);
            var deviation = rallyTime - _rallyTimes.Average();
            var maxDeviation = _rallyTimes.Max() - _rallyTimes.Min();
            return maxDeviation != 0 ? BaseScore + (int)(deviation / maxDeviation * BaseScore) : BaseScore;
        }

        private int CalcAvoidScore(int avoidBulletCount)
        {
            var clampAvoid = Mathf.Clamp(avoidBulletCount, 0, MaxAvoidBullet);
            var ratio = clampAvoid / (float)MaxAvoidBullet;
            return (int)(MaxRewardScore * (1 - ratio));
        }

        private int CalcReceiveBulletScore(int receiveBulletCount)
        {
            var clampReceive = Mathf.Clamp(receiveBulletCount, 0, MaxReceiveBullet);
            var ratio = clampReceive / (float)MaxReceiveBullet;
            return (int)(MaxRewardScore * (1 - ratio));
        }
    }
}
