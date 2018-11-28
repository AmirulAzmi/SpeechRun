using System;

namespace LCG {
	
	public class RandomNumberGenerator {

		int prevValue = 0;
		int maxValue;

		public RandomNumberGenerator(int max) {
			UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
			maxValue = max;
		}

		public int Next() {
			int rnd = UnityEngine.Random.Range(0, maxValue);
			if (rnd == prevValue)
				rnd = Next ();
			else
				prevValue = rnd;
			return rnd;
		}
	}
}
