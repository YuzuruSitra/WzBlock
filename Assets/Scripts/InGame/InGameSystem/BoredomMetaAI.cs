using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InGame.InGameSystem
{
    public class BoredomMetaAI
    {
        public const int Act = 10;
        private const int MaxQValue = 100;
        private const int InitialQValue = 50;
        private const int MaxReward = 100;
        private const int MinReward = -100;
        private const int BaseReward = 0;
        private const float Alpha = 0.1f;
        private const float Gamma = 0.9f;
        private const float ExplorationRate = 0.3f;
        private const int MaxStackData = 10;
        private const int MaxAvoidBullet = 4;
        
        // Learning Parameter.
        private readonly List<int> _qValues;
        private int[] _qStackValue = { InitialQValue, InitialQValue};
        private int _currentMaxQ = MaxQValue;
        private int[] _rewardStackValue = { BaseReward, BaseReward};
        private int[] _actionStackValue = { 0, 0};
        
        private readonly List<float> _rallyTimes;
        private readonly System.Random _random;
        private readonly List<int> _comboCounts;
        
        public int CurrentSelectAction { get; private set; }
        // Debug.
        // public int LearnCount;
        // public int CurrentQValue;
        public BoredomMetaAI()
        {
            CurrentSelectAction = 4;
            _qValues = new List<int>();
            _random = new System.Random();
               
            for (var i = 0; i < Act; ++i)
            {
                var diff = CurrentSelectAction - i;
                var addValue = InitialQValue - Mathf.Abs(diff);
                _qValues.Add(addValue);
            }
            _rallyTimes = new List<float>();
            _comboCounts = new List<int>();
        }

        public void Learning(float rallyTime, int avoidBulletCount, int comboCount)
        {
            // Convert calc value.
            var calcActionRate = CalcRallyScore(rallyTime);
            var calcIdleRate = CalcAvoidScore(avoidBulletCount);
            var calcRepeatingActionsRate = CalcComboCount(comboCount);

            // Calc reward.
            CalcReward(calcActionRate, calcIdleRate, calcRepeatingActionsRate);
            // Update Q value.
            CalcQValue();
            SelectAction();
            
            // Debug.
            // LearnCount++;
        }
        
        // Select action for ?ｾ趣ｽｵ - greedy.
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
            CurrentSelectAction = bestAction;
            // Debug.
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
            var clampReward = Mathf.Clamp((int)calcValue, MinReward, MaxReward );
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
            if (_rallyTimes.Count > MaxStackData)
                _rallyTimes.RemoveAt(0);
            var ratio = 0;
            foreach (var time in _rallyTimes)
                if (time < rallyTime) ratio++;
            return MinReward + (MaxReward - MinReward) / _rallyTimes.Count * ratio;
        }

        private int CalcAvoidScore(int avoidBulletCount)
        {
            var clampAvoid = Mathf.Clamp(avoidBulletCount, 0, MaxAvoidBullet);
            var ratio = MaxAvoidBullet - clampAvoid;
            return MinReward + (MaxReward - MinReward) / MaxAvoidBullet * ratio;
        }

        private int CalcComboCount(int comboCount)
        {
            _comboCounts.Add(comboCount);
            if (_comboCounts.Count > MaxStackData)
                _comboCounts.RemoveAt(0);
            var ratio = 0;
            foreach (var count in _comboCounts)
                if (count > comboCount) ratio++;
            return MinReward + (MaxReward - MinReward) / _comboCounts.Count * ratio;
        }
    }
}
