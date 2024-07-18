using System.Collections;
using UnityEngine;
using UnityEngine.UI;

class ButtonManager: MonoBehaviour 
{
    public Button buttonObject;
    public BlockManager bM;

    void Update()
    {
        if(bM.routineStarted)
        {
            buttonObject.interactable = false;
            gameObject.SetActive(false);
        }
    }
}