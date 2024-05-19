using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private const string FAZE_1 = "Really ?";
    private const string FAZE_2 = "Deleted .";

    public void ChangeSettingPanel()
    {
        bool isActive = _settingsPanel.activeInHierarchy;
        if (isActive)
        {
            _confirmationPanel.SetActive(false);
            _confirmationBts.SetActive(false);
        }
        _settingsPanel.SetActive(!isActive);
    }

    public void OpenContirmationPanel()
    {
        _confirmationPanel.SetActive(true);
        if (!_confirmationBts.activeSelf)
            _confirmationBts.SetActive(true);
        _confirmationText.text = FAZE_1;
    }

    public void CloseContirmationPanel()
    {
        _confirmationPanel.SetActive(false);
    }

    public void DeletedData()
    {
        _confirmationBts.SetActive(false);
        _confirmationText.text = FAZE_2;
    }

}
