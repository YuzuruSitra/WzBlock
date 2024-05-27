using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace InGame.UI
{
    public class TitleBtHandler : MonoBehaviour
    {
        [SerializeField]
        private Button _startBt;
        [SerializeField]
        private Button _settingsBt;
        [SerializeField]
        private Button _quitBt;
        [SerializeField]
        private Button _settingResumeBt;
        [SerializeField]
        private Button _settingDeleteBt;
        [SerializeField]
        private Button _confirmYesBt;
        [SerializeField]
        private Button _confirmNoBt;

        [SerializeField]
        private TitlePanelHandler _titlePanelHandler;
        [SerializeField]
        private SettingPanelHandler _settingPanelHandler;

        private void Start()
        {
            _startBt.onClick.AddListener(SceneHandler.Instance.GoMainGameScene);
            _settingsBt.onClick.AddListener(_titlePanelHandler.ChangeSettingPanel);
            _settingResumeBt.onClick.AddListener(_titlePanelHandler.ChangeSettingPanel);
            _settingResumeBt.onClick.AddListener(_settingPanelHandler.CloseFixedAction);
            _quitBt.onClick.AddListener(QuitGame);
            _settingDeleteBt.onClick.AddListener(_titlePanelHandler.OpenConfirmationPanel);
            _confirmNoBt.onClick.AddListener(_titlePanelHandler.CloseConfirmationPanel);
            _confirmYesBt.onClick.AddListener(PlayDataIO.Instance.DeleteData);
            _confirmYesBt.onClick.AddListener(_titlePanelHandler.DeletedData);
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        }
    }
}
