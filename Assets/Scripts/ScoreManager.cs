
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
	public static ScoreManager Instance;

	public Text score;
	public Text mic;
	public Text coin;
	public ScoreData data;
    public GameObject hud;
    //public GameObject scoreContainer;
    //public GameObject coinContainer;

    public static float value = 0;

	float transition = 0.0f;
	float animationDuration = 4.0f;
	float baseMultiplier = 1.0f;
	int milestone = 100;
	int nextMilestone = 100;

	public int coins = 0;
	const float COINS_VALUE = 5.0f;

	void Start()
	{
		Instance = this;
        coins = PlayerPrefs.GetInt("coins");
        coin.text = coins.ToString();
	}

	void Update()
	{
		if (transition > 1.0f) {
            if (PlayerController.Instance.canMove) {
                UpdateScore();
                //coinContainer.SetActive(true);
                //scoreContainer.SetActive(true);
                hud.SetActive(true);
            }
			if (value >= nextMilestone) {
				baseMultiplier += 1f;
				milestone += milestone;
				nextMilestone += milestone;
				PlayerController.speed += 1f;
				Debug.Log (PlayerController.speed);
			}
		} 

		else {
			transition += Time.deltaTime / animationDuration;
		}


	}

	void UpdateScore()
	{
		if (PlayerController.Instance.state == State.Multiplier)
			value += ((2.0f * baseMultiplier) * 2) * Time.deltaTime;
		else
			value += (2.0f * baseMultiplier) * Time.deltaTime;
		score.text = ((int)value).ToString();
	}

	public void GetCoin()
	{
		coins++;
		value += COINS_VALUE;
		coin.text = coins.ToString();
	}

	public void ResetScore()
	{
		value = 0;
		baseMultiplier = 1.0f;
		milestone = 100;
		nextMilestone = 100;

		PlayerController.speed = 4;
	}

	public void ShowMic(string text)
	{
		/*
		foreach (string devices in Microphone.devices) {
			if (devices == null)
				mic.text = "NULL";
			mic.text = mic.text + "\n" +  devices;
		}*/

		int min;
		int max;
		Microphone.GetDeviceCaps (null, out min, out max);

		mic.text = Microphone.devices [0];
	}

	public void SaveScore()
	{
		data.scores.Add (value);
		data.scores.Sort ();
		data.scores.Reverse ();
        if (data.scores.Count >= 10) {
            data.scores.RemoveAt(10);
        }
	}
}
