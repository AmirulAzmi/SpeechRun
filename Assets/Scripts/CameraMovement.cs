
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public Transform lookAt;

	Vector3 startOffset;
	Vector3 moveVector;

	float transition = 0.0f;
	float animationDuration = 4.0f;
	Vector3 animationOffset = new Vector3 (0, 5, 5);

	void Start () {
		startOffset = transform.position - lookAt.position;
	}

	void Update () {
		moveVector = lookAt.position + startOffset;

		moveVector.x = 0;

		moveVector.y = Mathf.Clamp (moveVector.y, 3, 6);

		if (transition > 1.0f) {
			transform.position = moveVector;
		} 
		
		else {
			transform.position = Vector3.Lerp (moveVector + animationOffset, moveVector, transition);
			transition += Time.deltaTime / animationDuration;
			transform.LookAt (lookAt.position + Vector3.up);
		}
	}
}
