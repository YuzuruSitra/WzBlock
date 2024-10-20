using System;
using InGame.Obj.Paddle;
using UnityEngine;

namespace InGame.InGameSystem
{
    public class MetaAIManipulator : MonoBehaviour
    {
        [SerializeField] private PaddleInfo _paddleInfo;

        private BoredomMetaAI _boredomMetaAI;
        private int _currentBoredomLevel = -1;
        public event Action<int> ChangeBoredomLevel;
        public int InitialBoredomLevel;
        public int MaxLevel { get; private set; }
        
        private void Awake()
        {
            _boredomMetaAI = new BoredomMetaAI();
            InitialBoredomLevel = _boredomMetaAI.CurrentSelectAction;
            MaxLevel = BoredomMetaAI.Act;
            _paddleInfo.DoLearnEvent += DoLearn;
        }

        private void OnDestroy()
        {
            _paddleInfo.DoLearnEvent -= DoLearn;
        }

        private void DoLearn()
        {
            _boredomMetaAI.Learning(_paddleInfo.RallyTime, _paddleInfo.AvoidEnemyBullet, _paddleInfo.LastCombo);
            ChangeBoredom(_boredomMetaAI.CurrentSelectAction);
        }

        private void ChangeBoredom(int newLevel)
        {
            if (newLevel == _currentBoredomLevel) return;
            _currentBoredomLevel = _boredomMetaAI.CurrentSelectAction;
            ChangeBoredomLevel?.Invoke(_currentBoredomLevel);
        }
    }
}
