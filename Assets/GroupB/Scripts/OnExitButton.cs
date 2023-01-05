using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
