using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BandaidController : MonoBehaviour
{
    public GameObject bandaid;
    public Text scoreText;
    public int score;

    // Start is called before the first frame update
    private StitchSpawner stitchspawner; // ms = stitchspawner
    void Start()
    {
        score = 0;
        stitchspawner = GetComponent<StitchSpawner>();
    }

    // Update is called once per frame
    void mouseUpdate() // mouse update
    {
        
        if (Input.GetButtonDown("Fire1") && stitchspawner.GameTime>0)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                if (hit.collider != null)
                {
                    Destroy(hit.transform.gameObject);
                    Touch myTouch = Input.GetTouch(0);
                    AddBandaid(myTouch);
                    stitchspawner.Spawn();
                }
        }

    }
    void Update() //touch
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && stitchspawner.GameTime>0)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);            
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Destroy(hit.transform.gameObject); // stitch is removed
                Touch myTouch = Input.GetTouch(0);
                GameObject _bandaid = AddBandaid(myTouch);
                score += 1;
                scoreText.text = score.ToString();

                stitchspawner.Spawn();
                //WaitForSecond
                //remove the bandaid
                Destroy(_bandaid,1);
                
            }
        }
    }
    void toucholdUpdate() //touch
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && stitchspawner.GameTime>0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    Destroy(hit.transform.gameObject);
                    Touch myTouch = Input.GetTouch(0);
                    AddBandaid(myTouch);
                    stitchspawner.Spawn();
                }
            }
        }
    }

    private GameObject AddBandaid(Touch TouchPos)
    {
        Vector3 objPos = Camera.main.ScreenToWorldPoint(TouchPos.position);
        objPos.z = 7;
        //bandaid.GetComponent<SpriteRenderer>().sprite = objectList[Random.Range(0, objectList.Count)];
        //bandaid.GetComponent<SpriteRenderer>().sprite = objectList[0];
        GameObject _bandaid = Instantiate(bandaid, objPos, Quaternion.identity) as GameObject;
        return _bandaid;
    }
}
