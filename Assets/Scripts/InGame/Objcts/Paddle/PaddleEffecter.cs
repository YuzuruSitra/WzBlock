using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleEffecter : MonoBehaviour
{
    private AbilityReceiver _abilityReceiver;
    private GameObject _currentEffect;
    [SerializeField]
    private GameObject _stanEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        _abilityReceiver = AbilityReceiver.Instance;
        _abilityReceiver.ConditionChanged += ChangeEffect;
    }

    private void ChangeEffect(AbilityReceiver.Condition newCondition)
    {
        if (_currentEffect != null) _currentEffect.SetActive(false);
        GameObject newEffect = null;
        if (newCondition == AbilityReceiver.Condition.Stan)
            newEffect = _stanEffect;
        if (newEffect != null) newEffect.SetActive(true);
        _currentEffect = newEffect;
    }
}
