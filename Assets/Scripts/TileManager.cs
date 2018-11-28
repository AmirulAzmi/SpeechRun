using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LCG;
using System;

public class TileManager : MonoBehaviour {

	public Transform player;
	public GameObject[] tilePrefabs;

	float safeDistance = 10.0f;
	float lastZPosition = -8.0f;
	float tileLength = 8.0f;
	int amnTilesOnScreen = 7;
	List<GameObject> tiles;

	bool isSafe = false;

	RandomNumberGenerator rng;

	void Start () {
		rng = new RandomNumberGenerator(tilePrefabs.Length);
		tiles = new List<GameObject> ();

		//First four tile should be a basic tile
		NewGame();
	}

	void Update () {
		if (player.position.z > (lastZPosition - amnTilesOnScreen * tileLength)) {
			SpawnTile (rng.Next ());
			if (isSafe)
				DeleteTile ();
		}
	}



	void SpawnTile(int index) {
		GameObject go = Instantiate (tilePrefabs [index]);
		go.transform.SetParent (transform);
		go.transform.position = Vector3.forward * lastZPosition;
		lastZPosition += tileLength;
		tiles.Add (go);
	}

	void DeleteTile() {
		Destroy (tiles [0]);
		tiles.RemoveAt (0);
	}

	IEnumerator DeleteCountdown() {
		yield return new WaitForSeconds (safeDistance);
		isSafe = true;
	}

	public void NewGame() {

		//player.position = new Vector3 (0.0f, 0.0f, -1.0f);
		lastZPosition = -8.0f;

		SpawnTile (0); //Initial Tile;
		SpawnTile (0);
		SpawnTile (0);
		SpawnTile (0);
		Time.timeScale = 1;
		StartCoroutine(DeleteCountdown());
	}
}
