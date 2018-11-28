using UnityEngine;

public class Obstacle : MonoBehaviour {

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") {
            PlayerController.Instance.Dead();
		}
	}
}
