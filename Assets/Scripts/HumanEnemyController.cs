using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HumanEnemyController : MonoBehaviour {

    public bool roam;
    public bool patrol;
    public bool stand;
    public bool chase;

    public float moveSpeed;

    private Rigidbody2D myRigidbody;

    private bool moving;

    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;

    private Vector3 moveDirection;

    public float lockOnRange;

    public Transform[] patrolPoints;
    private int currentPoint;

    // Use this for initialization
    void Start()
    {
        if (roam)
        {
            patrol = false;
            stand = false;
        }
        else if (patrol)
        {
            transform.position = patrolPoints[0].position;
            currentPoint = 0;
            roam = false;
            stand = false;
        }
        else if (stand)
        {
            roam = false;
            patrol = false;
        }
        else
        {
            stand = true;
        }

        myRigidbody = GetComponent<Rigidbody2D>();

        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Chase())
        {
            if (roam)
                Roam();
            else if (patrol)
                Patrol();
        }
    }

    bool Chase()
    {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position, this.transform.position) < lockOnRange)
        {
            Vector3 direction = GameObject.FindGameObjectWithTag("Player").gameObject.transform.position - this.transform.position;

            if (direction.magnitude > 0.5f)
            {
                //this.transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
                myRigidbody.velocity = new Vector3(direction.x * moveSpeed, direction.y * moveSpeed, 0);
            }
            RotateEnemy();
            return true;
        }
        else
        {
            return false;
        }
    }

    void Roam()
    {
        if (moving)
        {
            timeToMoveCounter -= Time.deltaTime;
            myRigidbody.velocity = moveDirection;

            if (timeToMoveCounter < 0f)
            {
                moving = false;
                timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
            }

            RotateEnemy();
        }
        else
        {
            timeBetweenMoveCounter -= Time.deltaTime;
            myRigidbody.velocity = Vector2.zero;

            if (timeBetweenMoveCounter < 0f)
            {
                moving = true;
                timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);

                moveDirection = new Vector3(Random.Range(1f, -1f) * moveSpeed, Random.Range(1f, -1f) * moveSpeed, 0f);
            }
        }
    }


    void Patrol()
    {
        if (transform.position == patrolPoints[currentPoint].position)
        {
            currentPoint++;
        }
        if (currentPoint >= patrolPoints.Length)
        {
            currentPoint = 0;
        }

        //moveDirection = new Vector3(patrolPoints[currentPoint].position.x * moveSpeed * Time.deltaTime, patrolPoints[currentPoint].position.y * moveSpeed * Time.deltaTime, 0);
        //myRigidbody.velocity = moveDirection;
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPoint].position, moveSpeed * Time.deltaTime);
        //RotateEnemy();
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

    void RotateEnemy()
    {
        // Rotate enemy in direction they're moving
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
