using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts
{
    public class HumanEnemyController : MonoBehaviour
    {
        [HideInInspector]
        public Stats EnemyStats;

        public bool roam;
        public bool patrol;
        public bool stand;
        public bool chase;

        private bool moving;
        private Rigidbody2D myRigidbody;

        public float timeBetweenMove;
        public float timeToMove;
        private float timeBetweenMoveCounter;
        private float timeToMoveCounter;

        private Vector3 moveDirection;

        public float lockOnRange;

        public Transform[] patrolPoints;
        private int currentPoint;

        private GameObject target;

        // Use this for initialization
        void Start()
        {
            EnemyStats = Stats.CreateInstance<Stats>();
            EnemyStats.SetStats(20, 4f, 10, 0.5f, Random.Range(1.2f, 2f));
            gameObject.transform.localScale = new Vector3(EnemyStats.Scale, EnemyStats.Scale, 1f);

            target = GameObject.FindGameObjectWithTag("Player").gameObject;

            if (roam)
            {
                patrol = false;
                stand = false;
            }
            else if (patrol)
            {
                // Roam if nowhere to start patrol
                if (patrolPoints[0] == null)
                {
                    patrol = false;
                    roam = false;

                    stand = true;
                }
                else
                {
                    transform.position = patrolPoints[0].position;
                    currentPoint = 0;
                    roam = false;
                    stand = false;
                }
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
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player").gameObject;
            }

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
            if (target == null) return false;
            if (Vector3.Distance(target.transform.position, this.transform.position) < lockOnRange)
            {
                Vector3 direction = target.transform.position - this.transform.position;

                if (direction.magnitude > 0.5f)
                {
                    //this.transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
                    myRigidbody.velocity = new Vector3(direction.x * EnemyStats.Speed, direction.y * EnemyStats.Speed, 0);
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

                    moveDirection = new Vector3(Random.Range(1f, -1f) * EnemyStats.Speed, Random.Range(1f, -1f) * EnemyStats.Speed, 0f);
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
            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPoint].position, EnemyStats.Health * Time.deltaTime);
            //RotateEnemy();
        }


        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerController>().TakeDamage(EnemyStats.AttackDamage);
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

        public void TakeDamage(int dmg)
        {
            EnemyStats.Health -= dmg;

            Debug.Log("*Enemy-Hit*");

            if (EnemyStats.Health <= 0)
            {
                Debug.Log("------------->OW!");
                Destroy(gameObject);
            }
        }
    }
}