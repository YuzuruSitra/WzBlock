using System;
using TMPro;
using UnityEngine;

namespace InGame.UI
{
    public class TitlePanelHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject _settingsPanel;
        [SerializeField]
        private GameObject _confirmationPanel;
        [SerializeField]
        private GameObject _confirmationBts;
        [SerializeField]
        private TMP_Text _confirmationText;
        private const string Faze1 = "Really ?";
        private const string Faze2 = "Deleted .";

        [SerializeField]
        private TMP_Text _userNameText;
        [SerializeField]
        private TMP_Text _userRankText;
        private PlayerInfoHandler _playerInfoHandler;

        private void Start()
        {
            _playerInfoHandler = PlayerInfoHandler.Instance;
            _userNameText.text = _playerInfoHandler.PlayerName;
            _userRankText.text = _playerInfoHandler.PlayerRank.ToString();
            _playerInfoHandler.ChangeName += ChangeNameText;
            _playerInfoHandler.ChangeRank += ChangeRankText;
        }

        private void OnDestroy()
        {
            _playerInfoHandler.ChangeName -= ChangeNameText;
            _playerInfoHandler.ChangeRank -= ChangeRankText;
        }

        public void ChangeSettingPanel()
        {
            var isActive = _settingsPanel.activeInHierarchy;
            if (isActive)
            {
                _confirmationPanel.SetActive(false);
                _confirmationBts.SetActive(false);
            }
            _settingsPanel.SetActive(!isActive);
        }

        public void OpenConfirmationPanel()
        {
            _confirmationPanel.SetActive(true);
            if (!_confirmationBts.activeSelf)
                _confirmationBts.SetActive(true);
            _confirmationText.text = Faze1;
        }

        public void CloseConfirmationPanel()
        {
            _confirmationPanel.SetActive(false);
        }

        public void DeletedData()
        {
            _confirmationBts.SetActive(false);
            _confirmationText.text = Faze2;
        }

        private void ChangeNameText(string valueName)
        {
            _userNameText.text = valueName;
        }

        private void ChangeRankText(int rank)
        {
            _userRankText.text = rank.ToString();
        }
    }
}
