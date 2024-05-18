using UnityEngine;
using UnityEngine.UI;

public class TitleBtHandler : MonoBehaviour
{
    [SerializeField]
    private Button _startBt;
    [SerializeField]
    private Button _settingsBt;
    [SerializeField]
    private Button _FinishBt;
    [SerializeField]
    private Button _settingResumeBt;

    [SerializeField]
    private TitlePanelHandler _titlePanelHandler;
    [SerializeField]
    private SettingPanelHandler _settingPanelHandler;
    void Start()
    {
        _startBt.onClick.AddListener(SceneHandler.Instance.GoMainGameScene);
        _settingsBt.onClick.AddListener(_titlePanelHandler.ChangeSettingPanel);
        _settingResumeBt.onClick.AddListener(_titlePanelHandler.ChangeSettingPanel);
        _settingResumeBt.onClick.AddListener(_settingPanelHandler.CloseFixedAction);
    }
}
