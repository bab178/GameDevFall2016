using UnityEngine;

public class FollowEntity : MonoBehaviour {
    public Transform targetTransform;
    public float minZoom, maxZoom;

    Camera cam;
    float cameraZoom;

    // Use this for initialization
    void Start () {
        cam = gameObject.GetComponent<Camera>();
        cameraZoom = minZoom;
        RepositionCamera();
    }

    // Update is called once per frame
    void Update () {
        RepositionCamera();
        AdjustCameraZoom();
    }

    void RepositionCamera() {
        cam.transform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, -1f);
        cam.orthographicSize = cameraZoom;
    }

    void AdjustCameraZoom()
    {
        // Invert scroll
        float scroll = -Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f) // if scrolled
        {
            if (cameraZoom + scroll > maxZoom) cameraZoom = maxZoom;
            else if (cameraZoom + scroll < minZoom) cameraZoom = minZoom;
            else cameraZoom += scroll;
        }
    }
}
