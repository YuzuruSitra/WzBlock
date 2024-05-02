using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo_Manager : MonoBehaviour
{
    public GameObject magicAttacksManager;
    public GameObject slashManager;

    public GameObject[] camerasList;
    int currentCam;



    void Awake()
    {
        slashManager.SetActive(true);
        magicAttacksManager.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchFXMode();
        CameraSelection();
    }

    void SwitchFXMode()
    {
        if(Input.GetKeyDown(KeyCode.Space) & magicAttacksManager.activeSelf == true)
        {
            slashManager.SetActive(true);
            magicAttacksManager.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.Space) & magicAttacksManager.activeSelf != true)
        {
            slashManager.SetActive(false);
            magicAttacksManager.SetActive(true);
        }
    }

    void CameraSelection()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (currentCam < camerasList.Length - 1)
            {
                camerasList[currentCam].SetActive(false);
                currentCam += 1;
                camerasList[currentCam].SetActive(true);
            }

            else if (currentCam >= camerasList.Length - 1)
            {
                camerasList[currentCam].SetActive(false);
                currentCam = 0;
                camerasList[currentCam].SetActive(true);
            }

        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (currentCam > 0)
            {
                camerasList[currentCam].SetActive(false);
                currentCam -= 1;
                camerasList[currentCam].SetActive(true);

            }

            else if (currentCam <= 0)
            {
                camerasList[currentCam].SetActive(false);
                currentCam = camerasList.Length - 1;
                camerasList[currentCam].SetActive(true);
            }

        }
    }
}
