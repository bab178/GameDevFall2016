using System.Linq;
using UnityEngine;
using System.Collections;

public class Player_attack : MonoBehaviour {

    private bool attacking = false;

    private float attackTimer = 0;

    public float attackCooldown =0.03f;

    public Collider2D attackingTrigger;


	// Use this for initialization
	void Awake ()
    {

        attackingTrigger.enabled = false;
        Debug.Log("Hi");

    }
	
	// Update is called once per frame
	void Update ()
    {
	
        if (Input.GetKey(KeyCode.Q) && !attacking)
        {
            attacking = true;
            attackTimer = attackCooldown;

            attackingTrigger.enabled = true;

            Debug.Log("attacking");
        }

        if (attacking)
        {
            if( attackTimer>0)
            {
                attackTimer -= Time.deltaTime;
            }

            else
            {
                attacking = false;
                attackingTrigger.enabled = false;
            }


        }


	}
}
