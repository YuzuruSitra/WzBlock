using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InGame.InGameSystem
{
    public class BoredomMetaAI
    {
        private static BoredomMetaAI _instance;
        public static BoredomMetaAI Instance => _instance ??= new BoredomMetaAI();
        
        private const int Act = 100;
        private const int InitialAct = 50;
        private const int MaxQValue = 100;
        private const int InitialQValue = 50;
        private const int MaxRewardScore = 100;
        private const int BaseScore = MaxRewardScore / 2;
        private const int InitialRewardValue = 50;
        private const float Alpha = 0.3f;
        private const float Gamma = 0.9f;
        private const float ExplorationRate = 0.3f;
        private const int MaxStackData = 10;
        
        // Learning Parameter.
        private readonly List<int> _qValues;
        private int[] _qStackValue = { InitialQValue, InitialQValue};
        private int _currentMaxQ = MaxQValue;
        private int[] _rewardStackValue = { InitialRewardValue, InitialRewardValue};
        private int[] _actionStackValue = { InitialAct, InitialAct};
        
        private readonly List<float> _rallyTimes;
        private readonly List<int> _avoidBulletCounts;
        private readonly List<int> _receiveBulletCounts;

        private readonly System.Random _random;

        private BoredomMetaAI()
        {
            _qValues = new List<int>();
            _random = new System.Random();
            
            for (var i = 0; i < Act; ++i)
            {
                var addValue = InitialAct == i ? InitialQValue : _random.Next(0, InitialQValue);
                _qValues.Add(addValue);
            }
            _rallyTimes = new List<float>();
            _avoidBulletCounts = new List<int>();
            _receiveBulletCounts = new List<int>();
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
                var maxQValue = _qValues.Max();
                for (var i = 1; i < Act; ++i)
                {
                    if (!(_qValues[i] > maxQValue)) continue;
                    maxQValue = _qValues[i];
                    bestAction = i;
                }    
            }
            _actionStackValue = SwapStacks(_actionStackValue, bestAction);
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
            var calcValue = (rallyRate - avoidBulletRate - receiveBulletRate) / 3.0f;
            var clampReward = Mathf.Clamp((int)calcValue, 0, MaxRewardScore);
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
            _avoidBulletCounts.Add(avoidBulletCount);
            if (_avoidBulletCounts.Count >= MaxStackData)
                _avoidBulletCounts.RemoveAt(0);
            var deviation = _avoidBulletCounts.Average() - avoidBulletCount;
            var maxDeviation = _avoidBulletCounts.Max() - _avoidBulletCounts.Min();
            return maxDeviation != 0 ? BaseScore + (int)(deviation / maxDeviation * BaseScore) : BaseScore;
        }

        private int CalcReceiveBulletScore(int receiveBulletCount)
        {
            _receiveBulletCounts.Add(receiveBulletCount);
            if (_receiveBulletCounts.Count >= MaxStackData)
                _receiveBulletCounts.RemoveAt(0);
            var deviation = _receiveBulletCounts.Average() - receiveBulletCount;
            var maxDeviation = _receiveBulletCounts.Max() - _receiveBulletCounts.Min();
            return maxDeviation != 0 ? BaseScore + (int)(deviation / maxDeviation * BaseScore) : BaseScore;
        }
    }
}
