using UnityEngine;
using LCG;

public class CoinSpawner : MonoBehaviour {

	public GameObject[] line;

	const int MAX_SPAWN_RATE = 10;
	const int COIN_SPAWN_RATE = 3;
	RandomNumberGenerator rng;

	void Awake() {
		rng = new RandomNumberGenerator(MAX_SPAWN_RATE);

		for (int i = 0; i < line.Length; i++) {
			
			int rnd = rng.Next ();

			if (rnd <= COIN_SPAWN_RATE)
				line [i].SetActive (true);
		}
	}

}
