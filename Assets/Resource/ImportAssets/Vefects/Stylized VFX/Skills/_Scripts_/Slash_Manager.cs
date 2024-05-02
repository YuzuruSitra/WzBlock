using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash_Manager : MonoBehaviour
{
    public float delay;
    private float reinitializeDelay;

    public Transform spawnOffset;
    bool usingSlashCircle;

    public GameObject[] FXList_Slash;
    public GameObject[] FXList_SlashCircle;
    public GameObject[] FXList_Piercing;
    GameObject[] currentFXList;

    int currentFXElement;

    // Start is called before the first frame update
    void Awake()
    {
        reinitializeDelay = delay;
        currentFXList = FXList_Slash;

    }

    // Update is called once per frame
    void Update()
    {
        if(delay > 0)
        {
            delay -= Time.deltaTime;
        }

        if(delay <= 0)
        {
            DoTheSlash(currentFXList[currentFXElement]);
            delay = reinitializeDelay;
        }

        InputsFXElement();
        InputsFXType();

        if(usingSlashCircle)
        {
            //SlashCircle();
        }

    }


    void InputsFXType()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            currentFXList = FXList_Slash;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            currentFXList = FXList_SlashCircle;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            currentFXList = FXList_Piercing;
        }

    }


    void InputsFXElement()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentFXElement < currentFXList.Length - 1)
            {
                currentFXElement += 1;
            }

            else if (currentFXElement >= currentFXList.Length - 1)
            {
                currentFXElement = 0;
            }

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentFXElement > 0)
            {
                currentFXElement -= 1;
            }

            else if (currentFXElement <= 0)
            {
                currentFXElement = currentFXList.Length - 1;
            }

        }
    }

    void DoTheSlash(GameObject FX)
    {
        GameObject slashFX;
        slashFX = Instantiate(FX, spawnOffset.position, spawnOffset.rotation);
        Destroy(slashFX, 3f);

    }
}
