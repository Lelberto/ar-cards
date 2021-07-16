using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARBehavior : MonoBehaviour
{
    public GameObject blueSpinner;
    public GameObject redSpinner;
    private ARTrackedImageManager trackedImageManager;
    private Dictionary<string, GameObject> instanciedSpinners;

    // Start is called before the first frame update
    void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        instanciedSpinners = new Dictionary<string, GameObject>();
        instanciedSpinners.Add("Blue Spinner", Instantiate(blueSpinner, Vector3.zero, Quaternion.identity));
        instanciedSpinners.Add("Red Spinner", Instantiate(redSpinner, Vector3.zero, Quaternion.identity));
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += onImageChange;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= onImageChange;
    }

    // Update is called once per frame
    void Update()
    {
        var blue = instanciedSpinners["Blue Spinner"];
        var red = instanciedSpinners["Red Spinner"];
        var distance = Vector3.Distance(red.transform.position, blue.transform.position) * 5000;
        blue.transform.Rotate(0, distance * Time.deltaTime, 0);
        red.transform.Rotate(0, distance * Time.deltaTime, 0);
    }

    private void onImageChange(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (var arTrackedImage in obj.added)
        {
            instanciedSpinners[arTrackedImage.name].SetActive(true);
        }

        foreach (var arTrackedImage in obj.updated)
        {
            UpdateImage(arTrackedImage);
        }

        foreach (var arTrackedImage in obj.removed)
        {
            instanciedSpinners[arTrackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage img)
    {
        string name = img.referenceImage.name;
        Vector3 pos = img.transform.position;
        instanciedSpinners[name].transform.position = pos;
    }
}
