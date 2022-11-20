using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class MarkerObjectsManager : MonoBehaviour
{
    private static string DEBUG_MARK = "[DEBUG][MarkerObjectsManager] ";
    private ARTrackedImageManager _trackedImagesManager;

    [SerializeField]
    ARRaycastManager m_RaycastManager;
    Camera arCam;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    public GameObject[] ArPrefabs;

    private readonly Dictionary<string, GameObject> _instantiatedPrefabs = new Dictionary<string, GameObject>();

    // reduce object size
    private Vector3 scaleChange = new Vector3(-0.992f, -0.992f, -0.992f);


    void Start()
    {
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
        Debug.Log(MarkerObjectsManager.DEBUG_MARK + "AR Camera reference: " + arCam);
    }
    void Awake()
    {
        _trackedImagesManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        // attach event handler when tracked images change
        _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
        Debug.Log(MarkerObjectsManager.DEBUG_MARK + "Image Traker event attached");
    }

    void OnDisable()
    {
        // remove event handler
        _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
        Debug.Log(MarkerObjectsManager.DEBUG_MARK + "Image Traker event detached");
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            var imageName = trackedImage.referenceImage.name;

            foreach (var curPrefab in ArPrefabs)
            {
                if (string.Compare(curPrefab.name, imageName, System.StringComparison.OrdinalIgnoreCase) == 0
                && !_instantiatedPrefabs.ContainsKey(imageName))
                {
                    var newPrefab = Instantiate(curPrefab, trackedImage.transform);
                    newPrefab.transform.localScale += scaleChange;
                    newPrefab.transform.Rotate(90f, 0f, 0f);
                    _instantiatedPrefabs[imageName] = newPrefab;
                }
            }
        }

        // foreach (var trackedImage in eventArgs.updated)
        // {
        //     _instantiatedPrefabs[trackedImage.referenceImage.name]
        //     .SetActive(trackedImage.trackingState == TrackingState.Tracking);
        // }

        foreach (var trackedImage in eventArgs.removed)
        {
            Destroy(_instantiatedPrefabs[trackedImage.referenceImage.name]);
            _instantiatedPrefabs.Remove(trackedImage.referenceImage.name);
        }
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        // if (touch.position.IsPointerOverUIObject())
        //     return;

        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = arCam.ScreenPointToRay(touch.position);
            RaycastHit hitObject;

            if (Physics.Raycast(ray, out hitObject))
            {
                GameObject hittedGameObject = hitObject.collider.gameObject;
                Debug.Log(MarkerObjectsManager.DEBUG_MARK + hittedGameObject.name + " hitted");
                Animator objectAnimator = hittedGameObject.GetComponent(typeof(Animator)) as Animator;
                AudioSource objectTalk = hittedGameObject.GetComponent(typeof(AudioSource)) as AudioSource;
                objectAnimator.Play("Somersault");
                objectTalk.Play(0);
                //Do whatever you want to do with the hitObject, which in this case would be your, well, case. Identify it either through name or tag, for instance below.
                // if (hitObject.transform.CompareTag("case"))
                // {
                //     //Do something with the case
                // }
            }
        }
    }
}
