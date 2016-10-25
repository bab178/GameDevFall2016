using UnityEngine;

public class FollowEntity : MonoBehaviour {
    public float minZoom, maxZoom;
    private Transform targetTransform;
    private GameObject playerGO;
    Camera cam;
    float cameraZoom;

    // Use this for initialization
    void Start () {
        cam = gameObject.GetComponent<Camera>();
        cameraZoom = minZoom;

        playerGO = GameObject.FindGameObjectWithTag("Player"); // NOTE: This only works with one player
        targetTransform = playerGO.transform;

        foreach(var canvas in playerGO.GetComponentsInChildren<Canvas>())
        {
            // Set Player GUI to be attached to camera
            canvas.worldCamera = cam;
        }
    }

    // Update is called once per frame
    void Update () {
        RepositionCamera();
        AdjustCameraZoom();
    }

    void RepositionCamera() {
        cam.transform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, -1f);
        cam.transform.rotation = Quaternion.identity;
        cam.orthographicSize = cameraZoom;
    }

    void AdjustCameraZoom()
    {
        // Only zoom if holding Zoom
        if(Input.GetAxis("Zoom") != 0f)
        {
            // Invert scroll
            float scroll = -Input.GetAxis("Mouse ScrollWheel") * 4;
            if (scroll != 0f) // if scrolled
            {
                if (cameraZoom + scroll > maxZoom) cameraZoom = maxZoom;
                else if (cameraZoom + scroll < minZoom) cameraZoom = minZoom;
                else cameraZoom += scroll;
            }
        }
    }
}
