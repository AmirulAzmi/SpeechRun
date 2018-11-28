using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewScore", menuName = "Create/Score", order = 1)]
public class ScoreData : ScriptableObject {

	public List<float> scores;
}
