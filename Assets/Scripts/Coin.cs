using UnityEngine;

public class Coin : MonoBehaviour {

	Animator anim;
	AudioSource aud;
	BoxCollider box;
	GameObject player;

	void Start()
	{
		anim = GetComponent<Animator> ();
		aud = GetComponent<AudioSource> ();
		box = GetComponent<BoxCollider> ();
		player = GameObject.FindGameObjectWithTag("Player");
		aud.volume = 0.1f;
	}

	void Update(){
		if (PlayerController.Instance.state == State.Magnet) {
			if (Vector3.Distance (player.transform.position, transform.position) < 5)
				transform.parent.position = Vector3.MoveTowards (transform.parent.position, player.transform.position, 10 * Time.deltaTime);
		}

		if (PlayerController.Instance.isPause)
			anim.speed = 0;
		else
			anim.speed = 1;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") {
			aud.Play ();
			anim.SetTrigger ("Collect");
			box.size = new Vector3 (0, 0, 0);
			ScoreManager.Instance.GetCoin ();
			Destroy (gameObject, 1.0f);
		}
	}
}
