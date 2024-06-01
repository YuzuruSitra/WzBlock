using UnityEngine;

namespace InGame.Obj.Paddle
{
    public class PaddleEffector : MonoBehaviour
    {
        private AbilityReceiver _abilityReceiver;
        private GameObject _currentEffect;
        [SerializeField]
        private GameObject _stanEffect;
        
        private void Start()
        {
            _abilityReceiver = AbilityReceiver.Instance;
            _abilityReceiver.ConditionChanged += ChangeEffect;
        }

        private void ChangeEffect(AbilityReceiver.Condition newCondition)
        {
            _currentEffect?.SetActive(false);
            GameObject newEffect = null;
            if (newCondition == AbilityReceiver.Condition.Stan)
                newEffect = _stanEffect;
            newEffect?.SetActive(true);
            _currentEffect = newEffect;
        }
    }
}
