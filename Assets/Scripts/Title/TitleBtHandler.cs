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

    void Start()
    {
        _startBt.onClick.AddListener(SceneHandler.Instance.GoMainGameScene);
    }

    void Update()
    {
        
    }
}
