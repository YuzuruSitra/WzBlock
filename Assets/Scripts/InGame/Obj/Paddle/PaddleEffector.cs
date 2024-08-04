using InGame.Gimmick;
using UnityEngine;

namespace InGame.Obj.Paddle
{
    public class PaddleEffector : MonoBehaviour
    {
        private AbilityReceiver _abilityReceiver;
        private ParticleSystem _currentEffect;
        [SerializeField] private ParticleSystem _stanEffect;
        
        private void Start()
        {
            _abilityReceiver = AbilityReceiver.Instance;
            _abilityReceiver.ConditionChanged += ChangeEffect;
        }

        private void ChangeEffect(AbilityReceiver.Condition newCondition)
        {
            _currentEffect?.Stop();
            _currentEffect?.Clear();
            ParticleSystem newEffect = null;
            if (newCondition == AbilityReceiver.Condition.Stan)
                newEffect = _stanEffect;
            newEffect?.Play();
            _currentEffect = newEffect;
        }
    }
}
