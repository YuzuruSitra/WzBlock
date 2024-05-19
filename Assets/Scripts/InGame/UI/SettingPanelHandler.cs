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
        _playerNameField.onEndEdit.AddListener(ChangePlayerNameField);
        _playerInfoHandler.ChangeRank += ChangeLevelText;
        // 初期値のセット
        InitializingDataSet();
        PlayDataIO.Instance.DeleteDataEvent += InitializingDataSet;
    }

    private void InitializingDataSet()
    {
        ChangeSensiSlider(_sensiHandler.Sensitivity);
        float initialSetValue = _saundHandler.CurrentVolume * 10.0f;
        _volumeValueText.text = initialSetValue.ToString();
        _sensiSlider.value = _sensiHandler.Sensitivity;
        _volumeSlider.value = initialSetValue;
        _playerNameField.text = _playerInfoHandler.PlayerName;
        _playerLevelText.text = _playerInfoHandler.PlayerRank.ToString();
    }

    private void ChangeSensiSlider(float value)
    {
        int sensi = (int)value;
        _sensiValueText.text = sensi.ToString();
    }

    private void ChangeVolumeSlider(float value)
    {
        _volumeValueText.text = value.ToString();
        _saundHandler.SetNewVolume(value / 10.0f);
    }

    private void ChangePlayerNameField(string newName)
    {
        if (newName != "") return;
        _playerNameField.text = PlayerInfoHandler.INITIAL_NAME;
    }

    private void ChangeLevelText(int level)
    {
        _playerLevelText.text = level.ToString();
    }

    public void CloseFixedAction()
    {
        _sensiHandler.ChangeSensitivity((int)_sensiSlider.value);
        _saundHandler.FixedVolume();
        _playerInfoHandler.ChangePlayerName(_playerNameField.text);
    }

}
