using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTapFace : MonoBehaviour
{
    public GameObject face1;
    public GameObject face2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (face1.activeSelf)
            {
                face1.SetActive(false);
                face2.SetActive(true);
            }
            else
            {
                face2.SetActive(false);
                face1.SetActive(true);
            }
        }
    }
}
