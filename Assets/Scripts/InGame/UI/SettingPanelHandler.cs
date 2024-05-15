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
    private SaundHandler _saundHandler;

    void Start()
    {
        _sensiSlider.onValueChanged.AddListener(ChangeSensiSlider);
        _volumeSlider.onValueChanged.AddListener(ChangeVolumeSlider);  
        _sensiHandler = SensiHandler.Instance;
        _saundHandler = GameObject.FindWithTag("SaundHandler").GetComponent<SaundHandler>();
        ChangeSensiSlider(_sensiHandler.Sensitivity);
        ChangeVolumeSlider(_saundHandler.CurrentVolume);
        _sensiSlider.value = _sensiHandler.Sensitivity;
        _volumeSlider.value = _saundHandler.CurrentVolume;
    }

    private void ChangeSensiSlider(float value)
    {
        _sensiValueText.text = "" + (int)value;
    }

    private void ChangeVolumeSlider(float value)
    {
        int volume = (int)value;
        _volumeValueText.text = "" + volume;
        _saundHandler.SetNewVolume(volume / 10.0f);
    }

    // パネル非アクティブ時に値の変更を反映
    void OnDisable()
    {
        _sensiHandler.ChangeSensitivity((int)_sensiSlider.value);
        _saundHandler.FixedVolume();
    }

}
