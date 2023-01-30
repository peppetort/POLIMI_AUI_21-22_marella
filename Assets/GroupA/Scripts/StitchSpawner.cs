using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StitchSpawner : MonoBehaviour
{
    public GameObject StitchPrefab;
    public Transform[] SpawnPoints;
    public float GameTime;
    public float RestartGame = 60;
    public Text GameText;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        //Setting the time
        GameTime -= Time.deltaTime;
        //Stops timer from going below 0
        if(GameTime < 1)
        {
            GameTime = 0;
            //Switches scene
            Progression.progressionLevelValue += 1;
            SceneSwitcher.loadCorridorScene();
        }
        // Sets the game time to a text
        GameText.text = GameTime.ToString();
        /*
        // Restart Game after 60 seconds
        if (GameTime ==0) {
            RestartGame -= Time.deltaTime;
            if (RestartGame < 1) GameTime=60;
        }
        */
    }

    public void Spawn()
    {
        GameObject stitch = Instantiate(StitchPrefab) as GameObject;
        stitch.transform.position = SpawnPoints[Random.Range(0,SpawnPoints.Length)].transform.position;

    }
}
