
using UnityEngine;
using LCG;

public class Car : MonoBehaviour {

	public GameObject[] spawnPoint;
	public float distance;

	float speed;
	GameObject player;
	RandomNumberGenerator chance;
	RandomNumberGenerator point;

	// Use this for initialization
	void Start () 
	{
		speed = PlayerController.speed;
		player = GameObject.FindGameObjectWithTag ("Player");

		chance = new RandomNumberGenerator (10);

		if (chance.Next () > 1) {
			if (chance.Next () > 5) {
				transform.position = spawnPoint [0].transform.position;
				transform.Rotate (0, -90, 0);
			} 

			else {
				transform.position = spawnPoint [1].transform.position;
				transform.Rotate (0, 90, 0);
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Vector3.Distance (player.transform.position, transform.position) <= distance)
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
	}
}
