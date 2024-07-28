using System;
using InGame.Obj.Paddle;
using UnityEngine;

namespace InGame.InGameSystem
{
    public class MetaAIManipulator : MonoBehaviour
    {
        [SerializeField] 
        private PaddleInfo _paddleInfo;

        private BoredomMetaAI _boredomMetaAI;
        public int CurrentBoredomLevel => _boredomMetaAI.CurrentSelectAction;
        public int MaxLevel { get; private set; }
        
        private void Awake()
        {
            _boredomMetaAI = new BoredomMetaAI();
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
        }
    }
}
