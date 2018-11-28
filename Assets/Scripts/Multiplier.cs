using System.Collections;
using UnityEngine;
using LCG;

public class Multiplier : MonoBehaviour {

	float duration = 15.0f;

	int count;
	const float SPAWN_RATE = 2.0f;
	RandomNumberGenerator rng;

	Animator anim;
	GameObject player;

	// Use this for initialization
	void Start () {
		rng = new RandomNumberGenerator (10);
		int rnd = rng.Next ();

		if (rnd <= (int)SPAWN_RATE)
			gameObject.SetActive (true);
		else
			gameObject.SetActive (false);

		count = (int)duration;
		player = GameObject.FindGameObjectWithTag ("Player");
		anim = GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") {
			PlayerController.Instance.state = State.Multiplier;
			anim.SetTrigger ("Collected");
			transform.SetParent (player.transform);
			StartCoroutine (StartCountdown ());
		}
	}

	IEnumerator StartCountdown()
	{
		while (count > 0) {
			yield return new WaitForSeconds (1.0f);
			count -= 1;
			Debug.Log ("Counter: " + count);
		}

		PlayerController.Instance.state = State.Normal;
		Destroy (gameObject);
	}
}