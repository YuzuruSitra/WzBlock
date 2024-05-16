using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingPanelHandler : MonoBehaviour
{
    [SerializeField]
    private Slider _sensiSlider, _volumeSlider;
    [SerializeField]
    private TMP_Text _sensiValueText, _volumeValueText, _playerLevelText;
    [SerializeField]
    private TMP_InputField _playerNameField;

    private SensiHandler _sensiHandler;
    private SaundHandler _saundHandler;
    private PlayerInfoHandler _playerInfoHandler;

    void Start()
    {
        _sensiHandler = SensiHandler.Instance;
        _saundHandler = GameObject.FindWithTag("SaundHandler").GetComponent<SaundHandler>();
        _playerInfoHandler = PlayerInfoHandler.Instance;
        // ���X�i�[�o�^
        _sensiSlider.onValueChanged.AddListener(ChangeSensiSlider);
        _volumeSlider.onValueChanged.AddListener(ChangeVolumeSlider);
        // �����l�̃Z�b�g
        ChangeSensiSlider(_sensiHandler.Sensitivity);
        ChangeVolumeSlider(_saundHandler.CurrentVolume);
        _sensiSlider.value = _sensiHandler.Sensitivity;
        _volumeSlider.value = _saundHandler.CurrentVolume;
        _playerNameField.text = _playerInfoHandler.PlayerName;
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

    private void ChangeLevelText(int level)
    {
        _playerLevelText.text = "" + level;
    }

    void OnEnable()
    {
        if (_playerInfoHandler == null) return;
        ChangeLevelText(_playerInfoHandler.PlayerLevel);
    }


    // �p�l����A�N�e�B�u���ɒl�̕ύX�𔽉f
    void OnDisable()
    {
        _sensiHandler.ChangeSensitivity((int)_sensiSlider.value);
        _saundHandler.FixedVolume();
        _playerInfoHandler.ChangePlayerName(_playerNameField.text);
    }

}
