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
        // リスナー登録
        _sensiSlider.onValueChanged.AddListener(ChangeSensiSlider);
        _volumeSlider.onValueChanged.AddListener(ChangeVolumeSlider);
        // 初期値のセット
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


    // パネル非アクティブ時に値の変更を反映
    void OnDisable()
    {
        _sensiHandler.ChangeSensitivity((int)_sensiSlider.value);
        _saundHandler.FixedVolume();
        _playerInfoHandler.ChangePlayerName(_playerNameField.text);
    }

}
