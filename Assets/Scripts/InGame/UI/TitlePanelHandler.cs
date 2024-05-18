using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanelHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _settingsPanel;

    void Start()
    {
        
    }

    public void ChangeSettingPanel()
    {
        bool isActive = _settingsPanel.activeInHierarchy;
        _settingsPanel.SetActive(!isActive);
    }
}
