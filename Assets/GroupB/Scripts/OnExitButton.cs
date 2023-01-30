using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    attached to exit button to display the confirmation dialog
*/
public class OnExitButton : MonoBehaviour
{
    public GameObject confirmationPanel;
    public void onExitClick()
    {
        confirmationPanel.SetActive(true);
    }

    public void onNoConfirmation()
    {
        confirmationPanel.SetActive(false);
    }
}
