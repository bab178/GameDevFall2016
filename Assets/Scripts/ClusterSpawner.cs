using UnityEngine;

public class ClusterSpawner : MonoBehaviour {

    public GameObject EntityToSpawn;
    public int NumberToSpawn;
    public float SpreadModifier;

	void Start ()
    {
        int spawned = 0;
        while(spawned < NumberToSpawn)
        {
            var randomPos = new Vector3(transform.position.x + Random.Range(-SpreadModifier, SpreadModifier),
                                        transform.position.y + Random.Range(-SpreadModifier, SpreadModifier));
            var randomRot = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
            GameObject go = (GameObject)Instantiate(EntityToSpawn, randomPos, randomRot);
            go.transform.parent = gameObject.transform;
            spawned++;
        }
	}
}
