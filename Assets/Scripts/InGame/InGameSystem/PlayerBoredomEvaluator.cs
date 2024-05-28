using System;
using System.Collections.Generic;
using System.Linq;

namespace InGame.InGameSystem
{
    public class PlayerBoredomEvaluator
    {
        // �萔
        private const int Nodeno = 5;  // �s���̐��i��Ƃ���5�ɐݒ�j
        private const double Initial = 50;
        private const double Alpha = 0.1;
        private const double Gamma = 0.9;
        private const double ExplorationRate = 0.3;

        // �����o�ϐ�
        private readonly List<double> _qValues;
        private readonly List<float> _rallyTimes;
        private readonly List<int> _avoidBulletCounts;
        private readonly List<int> _receiveBulletCounts;

        // �V���O���g���C���X�^���X
        private static PlayerBoredomEvaluator _instance;

        // �v���p�e�B�ŃC���X�^���X���擾
        public static PlayerBoredomEvaluator Instance => _instance ??= new PlayerBoredomEvaluator();

        // �v���C�x�[�g�R���X�g���N�^
        private PlayerBoredomEvaluator()
        {
            _qValues = new List<double>();
            for (var i = 0; i < Nodeno; ++i)
            {
                _qValues.Add(Initial);
            }
            _rallyTimes = new List<float>();
            _avoidBulletCounts = new List<int>();
            _receiveBulletCounts = new List<int>();
        }

        private double UpdateQValue(double oldValue, double reward)
        {
            return oldValue + Alpha * (Gamma * reward - oldValue);
        }

        private int SelectActionWithExploration()
        {
            if (UnityEngine.Random.value < ExplorationRate)
            {
                // �Â̊m���Ń����_���ȍs����I��
                return UnityEngine.Random.Range(0, Nodeno);
            }
            else
            {
                // 1-�Â̊m���ōł�����Q�l�����s����I��
                var bestAction = 0;
                var maxQValue = _qValues[0];

                for (int i = 1; i < Nodeno; ++i)
                {
                    if (_qValues[i] > maxQValue)
                    {
                        maxQValue = _qValues[i];
                        bestAction = i;
                    }
                }
                return bestAction;
            }
        }

        // �w�K���đދ��x���Z�o
        public double CalcBoredom(float rallyTime, int avoidBulletCount, int receiveBulletCount)
        {
            var action = SelectActionWithExploration();

            // �v�Z�l�ɕϊ�
            var calcActionRate = CalcRallyRate(rallyTime);
            var calcIdleRate = CalcAvoidRate(avoidBulletCount);
            var calcRepeatingActionsRate = CalcReceiveBulletRate(receiveBulletCount);

            // ��V�̌v�Z���@
            var reward = (calcActionRate - calcIdleRate - calcRepeatingActionsRate) / 3.0 * 100;
            var updateValue = Math.Max(0.0, Math.Min(100.0, UpdateQValue(_qValues[action], reward)));
            // Q�l�̍X�V
            _qValues[action] = updateValue;

            var boredom = 0.0;
            for (var i = 0; i < Nodeno; ++i)
            {
                boredom += _qValues[i];
            }
            return boredom / Nodeno;
        }

        private double CalcRallyRate(float rallyTime)
        {
            _rallyTimes.Add(rallyTime);
            var averageRallyTime = _rallyTimes.Average();
            var maxDeviation = _rallyTimes.Max() - _rallyTimes.Min();
            if (maxDeviation == 0) maxDeviation = 1.0f;
            return 0.5 + (rallyTime - averageRallyTime) / maxDeviation / 2.0;
        }

        private double CalcAvoidRate(int avoidBulletCount)
        {
            _avoidBulletCounts.Add(avoidBulletCount);
            var averageAvoidBulletCount = _avoidBulletCounts.Average();
            var maxDeviation = _avoidBulletCounts.Max() - _avoidBulletCounts.Min();
            if (maxDeviation == 0) maxDeviation = 1;
            return 0.5 + (avoidBulletCount - averageAvoidBulletCount) / maxDeviation / 2.0;
        }

        private double CalcReceiveBulletRate(int receiveBulletCount)
        {
            _receiveBulletCounts.Add(receiveBulletCount);
            var averageReceiveBulletCount = _receiveBulletCounts.Average();
            var maxDeviation = _receiveBulletCounts.Max() - _receiveBulletCounts.Min();
            if (maxDeviation == 0) maxDeviation = 1;
            return 0.5 + (receiveBulletCount - averageReceiveBulletCount) / maxDeviation / 2.0;
        }
    }
}
