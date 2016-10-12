using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public Transform PlayerTransform;
    public Camera camera;

    float cameraZoom;
    float minZoom, maxZoom;

    // Use this for initialization
    void Start () {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        minZoom = 5f;
        maxZoom = 10f;
        cameraZoom = minZoom;
        RepositionCamera();
    }

    // Update is called once per frame
    void Update () {
        RepositionCamera();
        AdjustCameraZoom();
    }

    void RepositionCamera() {
        camera.transform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, -1f);
        camera.orthographicSize = cameraZoom;
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
