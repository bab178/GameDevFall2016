using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
    public Transform PlayerTransform;
    public Camera camera;

    // Use this for initialization
    void Start () {
        RepositionCamera();
    }

    // Update is called once per frame
    void Update () {
        RepositionCamera();
    }

    void RepositionCamera() {
        camera.transform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, -5);
    }
}
