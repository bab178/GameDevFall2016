using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    public Transform player;
    public float lockOnRange;
    public float moveSpeed;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
    }

    // Update is called once per frame
    void Update () {
	    if (Vector3.Distance(player.position, transform.position) < lockOnRange)
        {
            Vector3 direction = player.position - transform.position;
            //direction.x = 0;
            //direction.y = 0;

            transform.rotation = Quaternion.LookRotation(Vector3.zero, direction);
        }
	}
}
