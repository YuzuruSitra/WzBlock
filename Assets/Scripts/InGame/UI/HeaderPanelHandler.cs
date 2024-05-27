using System;
using TMPro;
using UnityEngine;

namespace InGame.UI
{
    public class HeaderPanelHandler : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _nameText, _levelText;
    
        private PlayerInfoHandler _playerInfoHandler;

        private void Start()
        {
            _playerInfoHandler = PlayerInfoHandler.Instance;
            _playerInfoHandler.ChangeName += ChangeNameText;
            _playerInfoHandler.ChangeRank += ChangeRankText;
            _nameText.text = _playerInfoHandler.PlayerName;
            _levelText.text = _playerInfoHandler.PlayerRank.ToString();
        }

        private void ChangeNameText(string valueName)
        {
            _nameText.text = valueName;
        }

        private void ChangeRankText(int level)
        {
            _levelText.text = level.ToString();
        }

    }
}
