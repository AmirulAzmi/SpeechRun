using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
	public static MenuManager Instance;

	//public TileManager tm;
	public GameObject pause;
    public GameObject gameover;
    public GameObject bumped;
    public Text countDown;
    public bool isbumped = false;

	void Start()
	{
		Instance = this;
	}

    void Update() {
        if (isbumped && Input.GetKeyDown(KeyCode.Space)) {
            ReloadScene();
        }
    }
	
    public void PlayGame()
	{
		PlayerController.Instance.canMove = true;
	}

	public void Retry()
	{
        HideGameOver();
		ScoreManager.Instance.ResetScore ();
        ReloadScene();
	}

    public void Continue(){
        bumped.SetActive(true);
        isbumped = true;
    }

    public void ReloadScene()
    {
        PlayerController.Instance.canMove = true;
        DestroyAllObject();
        SceneManager.LoadScene("_SCENE", LoadSceneMode.Single);
    }
    
    public void Home()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    public void Pause() { pause.SetActive(true); }

    public void Resume() { pause.SetActive(false); }

    public void DisplayGameOver() { gameover.SetActive(true); }

    private void HideGameOver() { gameover.SetActive(false); }

    private void DestroyAllObject()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");

        foreach (GameObject g in objects)
        {
            Destroy(g);
        }
    }
}
