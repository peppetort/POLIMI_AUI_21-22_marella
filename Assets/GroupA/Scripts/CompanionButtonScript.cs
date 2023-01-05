using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompanionButtonScript : MonoBehaviour
{

    //camera position
    [SerializeField]
    private GameObject arCameraObject;

    [SerializeField]
    private GameObject octopusGO;

    private int c;

    private void Awake()
    {
        c = 0;
    }

    //On click event
    public void OnClick(){
        SpawnCompanion spawnCompanion = gameObject.GetComponent<SpawnCompanion>();

        
        if(c == 0)
        {
            PlaceOctopus();
            c = 1;
        }
        

        spawnCompanion.UpdateOctopusPose(arCameraObject);
        print("pulsante schiacciato");
    }

    public void PlaceOctopus()
    {
        Quaternion rotation = octopusGO.transform.rotation;
        Vector3 position = octopusGO.transform.position;

        octopusGO.transform.SetParent(arCameraObject.transform);

        octopusGO.transform.localPosition = position;
        octopusGO.transform.localRotation = rotation;
    }

}
