using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    public class SettingPanelHandler : MonoBehaviour
    {
        [SerializeField]
        private Slider _sensiSlider, _volumeSlider;
        [SerializeField]
        private TMP_Text _sensiValueText, _volumeValueText, _playerRankText;
        [SerializeField]
        private TMP_InputField _playerNameField;

        private SensiHandler _sensiHandler;
        private SoundHandler _soundHandler;
        private PlayerInfoHandler _playerInfoHandler;

        private void Start()
        {
            _sensiHandler = SensiHandler.Instance;
            _soundHandler = GameObject.FindWithTag("SoundHandler").GetComponent<SoundHandler>();
            _playerInfoHandler = PlayerInfoHandler.Instance;
            
            _sensiSlider.onValueChanged.AddListener(ChangeSensiSlider);
            _volumeSlider.onValueChanged.AddListener(ChangeVolumeSlider);
            _playerNameField.onEndEdit.AddListener(ChangePlayerNameField);
            _playerInfoHandler.ChangeRank += ChangeRankText;
            PlayDataIO.Instance.DeleteDataEvent += InitializingDataSet;

            InitializingDataSet();
        }

        private void OnDestroy()
        {
            _playerInfoHandler.ChangeRank -= ChangeRankText;
            PlayDataIO.Instance.DeleteDataEvent -= InitializingDataSet;
        }

        private void InitializingDataSet()
        {
            ChangeSensiSlider(_sensiHandler.Sensitivity);
            var initialSetValue = _soundHandler.CurrentVolume * 10.0f;
            _volumeValueText.text = initialSetValue.ToString(CultureInfo.InvariantCulture);
            _sensiSlider.value = _sensiHandler.Sensitivity;
            _volumeSlider.value = initialSetValue;
            _playerNameField.text = _playerInfoHandler.PlayerName;
            _playerRankText.text = _playerInfoHandler.PlayerRank.ToString();
        }

        private void ChangeSensiSlider(float value)
        {
            var sensi = (int)value;
            _sensiValueText.text = sensi.ToString();
        }

        private void ChangeVolumeSlider(float value)
        {
            _volumeValueText.text = value.ToString(CultureInfo.CurrentCulture);
            _soundHandler.SetNewVolume(value / 10.0f);
        }

        private void ChangePlayerNameField(string newName)
        {
            if (newName != "") return;
            _playerNameField.text = PlayerInfoHandler.InitialName;
        }

        private void ChangeRankText(int level)
        {
            _playerRankText.text = level.ToString();
        }

        public void CloseFixedAction()
        {
            _sensiHandler.ChangeSensitivity((int)_sensiSlider.value);
            _soundHandler.FixedVolume();
            _playerInfoHandler.ChangePlayerName(_playerNameField.text);
        }

    }
}
