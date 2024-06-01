using InGame.Obj.Paddle;
using UnityEngine;

namespace InGame.InGameSystem
{
    public class TrainingHandler : MonoBehaviour
    {
        [SerializeField] 
        private PaddleInfo _paddleInfo;
        public BoredomMetaAI BoredomMetaAI { get; private set; }

        private void Start()
        {
            BoredomMetaAI = new BoredomMetaAI();
            _paddleInfo.HitBallEvent += DoLearn;
        }

        private void DoLearn()
        {
            BoredomMetaAI.Learning(_paddleInfo.RallyTime, _paddleInfo.AvoidEnemyBullet, _paddleInfo.ReceiveEnemyBullet);
        }
    }
}
