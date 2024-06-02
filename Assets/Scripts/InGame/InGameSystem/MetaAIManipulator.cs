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

        private void Start()
        {
            _boredomMetaAI = new BoredomMetaAI();
            _paddleInfo.DoLearnEvent += DoLearn;
        }

        private void DoLearn()
        {
            _boredomMetaAI.Learning(_paddleInfo.RallyTime, _paddleInfo.AvoidEnemyBullet, _paddleInfo.ReceiveEnemyBullet);
        }
    }
}
