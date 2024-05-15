using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingPanelHandler : MonoBehaviour
{
    [SerializeField]
    private Slider _sensiSlider, _volumeSlider;
    [SerializeField]
    private TMP_Text _sensiValueText, _volumeValueText;
    
    private SensiHandler _sensiHandler;

    void Start()
    {
        _sensiSlider.onValueChanged.AddListener(ChangeSensiSlider);
        _volumeSlider.onValueChanged.AddListener(ChangeVolumeSlider);  
        _sensiHandler = SensiHandler.Instance;
        ChangeSensiSlider(_sensiHandler.Sensitivity);
        ChangeVolumeSlider(_volumeSlider.value);
    }

    private void ChangeSensiSlider(float value)
    {
        _sensiValueText.text = "" + (int)value;
    }

    private void ChangeVolumeSlider(float value)
    {
        _volumeValueText.text = "" + (int)value;
    }

    // �p�l����A�N�e�B�u���ɒl�̕ύX�𔽉f
    void OnDisable()
    {
        _sensiHandler.ChangeSensitivity((int)_sensiSlider.value);
    }

}
