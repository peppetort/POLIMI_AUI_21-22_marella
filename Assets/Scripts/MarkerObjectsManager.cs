using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// keep the status of the interaction
enum InteractionStatus
{
    Ready,
    Talking,
    ShowingVideo,
    TalkingEnded,
    VideoEnded,
}

class Character
{

    private string DEBUG_MARK = "[DEBUG]";
    public int id;
    public GameObject gameObject;
    private Animator animator;
    private AudioSource audioSource;
    private Renderer renderer;
    //private Canvas dialog;
    private GameObject instance;


    private InteractionStatus InteractionStatus = InteractionStatus.Ready;

    private static float SCALE_FACTOR = -0.95f;


    public Character(GameObject gameObject)
    {
        this.gameObject = gameObject;
        DEBUG_MARK = DEBUG_MARK + "[" + gameObject.name + "] ";
    }

    public void setInstace(GameObject instance)
    {
        this.instance = instance;
        id = instance.GetHashCode();
        // assign the id of the character to its instance in order to 
        // be able to retreive it when hitted by a raycast
        this.instance.transform.name = id.ToString();

        animator = this.instance.GetComponent(typeof(Animator)) as Animator;
        audioSource = this.instance.GetComponent(typeof(AudioSource)) as AudioSource;
        renderer = this.instance.GetComponentInChildren(typeof(Renderer)) as Renderer;
        this.instance.transform.localScale += new Vector3(SCALE_FACTOR, SCALE_FACTOR, SCALE_FACTOR);
        this.instance.transform.Rotate(-90f, 0f, 180f);
    }

    public Object getInstance()
    {
        return this.instance;
    }

    public void touch()
    {
        if (InteractionStatus != InteractionStatus.Talking)
            animator.Play("Clicked");

    }

    public void update()
    {
        if (renderer.isVisible)
        {

            if ((InteractionStatus == InteractionStatus.Ready || InteractionStatus == InteractionStatus.TalkingEnded) && audioSource.isPlaying)
            {
                InteractionStatus = InteractionStatus.Talking;
                Debug.Log(DEBUG_MARK + InteractionStatus);
            }
            if (InteractionStatus == InteractionStatus.Talking && !audioSource.isPlaying)
            {
                InteractionStatus = InteractionStatus.TalkingEnded;
                Debug.Log(DEBUG_MARK + InteractionStatus);
                //TODO: show buttons
            }
        }
        else
        {
            // reset the interaction state if the object is no visible anymore
            if (InteractionStatus != InteractionStatus.Ready)
            {
                InteractionStatus = InteractionStatus.Ready;
                Debug.Log(DEBUG_MARK + InteractionStatus);
            }

        }

    }

}

[RequireComponent(typeof(ARTrackedImageManager))]
public class MarkerObjectsManager : MonoBehaviour
{
    private static string DEBUG_MARK = "[DEBUG][MarkerObjectsManager] ";
    private ARTrackedImageManager trackedImagesManager;
    private ARRaycastManager raycastManager;
    private Camera arCam;

    public GameObject[] ARGameObjects;

    private List<Character> availableCharacters = new List<Character>();
    private List<Character> showedCharacters = new List<Character>();

    private Dictionary<string, Character> markerToCharacterMap = new Dictionary<string, Character>();



    void Start()
    {

        foreach (var gameObject in ARGameObjects)
        {
            availableCharacters.Add(new Character(gameObject));
        }
    }

    void Awake()
    {
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
        trackedImagesManager = GetComponent<ARTrackedImageManager>();
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void OnEnable()
    {
        // attach event handler when tracked images change
        trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        // remove event handler
        trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            if (availableCharacters.Count == 0)
            {
                return;
            }
            // pick random element from available Characters and instantiate it 
            int randomCharacterIndex = Random.Range(0, availableCharacters.Count - 1);
            Character selectedCharacter = availableCharacters[randomCharacterIndex];
            var instance = Instantiate(selectedCharacter.gameObject, trackedImage.transform);
            selectedCharacter.setInstace(instance);

            // associate character to the marker
            var markerName = trackedImage.referenceImage.name;
            markerToCharacterMap[markerName] = selectedCharacter;

            // remove character from avaialble
            availableCharacters.RemoveAt(randomCharacterIndex);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            var markerName = trackedImage.referenceImage.name;
            Destroy(markerToCharacterMap[markerName].getInstance());
        }
    }

    private Character getHittedCharacter(RaycastHit hit)
    {
        foreach (var character in markerToCharacterMap.Values.ToList())
        {
            if (character.id.ToString() == hit.collider.gameObject.transform.name)
            {
                Debug.Log(DEBUG_MARK + character.gameObject.name + " hitted");
                return character;
            }
        }
        return null;
    }

    void Update()
    {
        foreach (var character in markerToCharacterMap.Values.ToList())
        {
            character.update();
        }

        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        // TODO:
        // if (touch.position.IsPointerOverUIObject())
        //     return;

        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = arCam.ScreenPointToRay(touch.position);
            RaycastHit hitObject;

            if (Physics.Raycast(ray, out hitObject))
            {
                Character hittedCharacter = getHittedCharacter(hitObject);

                if (hittedCharacter == null)
                    return;

                hittedCharacter.touch();

            }
        }
    }
}
