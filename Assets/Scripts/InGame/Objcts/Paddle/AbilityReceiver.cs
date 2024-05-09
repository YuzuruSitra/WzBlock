using System;

public class AbilityReceiver
{
    // �V���O���g��
    private static AbilityReceiver instance;
    public static AbilityReceiver Instance => instance ?? (instance = new AbilityReceiver());
    private Condition _currentCondition;
    public Condition CurrentCondition => _currentCondition;
    public event Action<Condition> ConditionChanged;
    private const float COOL_TIME = 0.5f;
    private DateTime _lastConditionChange;
    public bool IsEnable => (DateTime.Now - _lastConditionChange).TotalSeconds >= COOL_TIME;

    public enum Condition
    {
        Default,
        Stan
    }

    private AbilityReceiver() {}

    public void ChangeCondition(Condition newCondition)
    {
        if (_currentCondition == newCondition) return;
        if (_currentCondition != Condition.Default && newCondition != Condition.Default) return;
        // �N�[���^�C�����m�F
        if (!IsEnable) return;

        _lastConditionChange = DateTime.Now;
        _currentCondition = newCondition;
        ConditionChanged?.Invoke(newCondition);
    }

}
