using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Menu
{
    main = 1,
    scoreboard,
    tutorial,
}

public class MenuController : MonoBehaviour
{

    public static MenuController Instance;

    public Animator main;
    public Animator tutorpage;

    public Button play;
    public Button score;
    public Button tut;
    public Button quit;

    public ScoreData data;
    public List<Text> scores;

    Menu page = Menu.main;

    // Use this for initialization
    void Start()
    {
        Instance = this;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (page == Menu.main) { QuitGame(); }
            else { ViewMenu(); }
        }
    }
    public void SpeechHyp(string hypothesis)
    {
        switch (hypothesis)
        {
            case "play":
                if (page == Menu.main)
                    PlayGame();
                break;

            case "menu":
                if (page != Menu.main)
                    ViewMenu();
                break;

            case "score":
                ViewScoreboard();
                break;

            case "tutorial":
                ViewTutorial();
                break;

            case "next":
                if (page == Menu.tutorial)
                    NextPage();
                break;

            case "previous":
                if (page == Menu.tutorial)
                    PrevPage();
                break;

            case "quit":
                if (page == Menu.main)
                    QuitGame();
                break;
        }
    }

    public void ViewMenu()
    {
        page = Menu.main;

        main.SetBool("score", false);
        main.SetBool("tutor", false);
        main.SetBool("main", true);
        //tutorpage.SetBool("switch", false);
    }

    public void ViewScoreboard()
    {
        page = Menu.scoreboard;
        score.Select();
        main.SetBool("main", false);
        main.SetBool("score", true);
        DisplayScore();

    }

    public void ViewTutorial()
    {
        page = Menu.tutorial;
        tut.Select();

        main.SetBool("tutor", true);
        main.SetBool("main", false);
    }

    public void NextPage()
    {
        tutorpage.SetBool("switch", true);
    }

    public void PrevPage()
    {
        tutorpage.SetBool("switch", false);
    }

    public void PlayGame()
    {
        play.Select();
        SceneManager.LoadScene("_SCENE", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        quit.Select();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void DisplayScore()
    {
        int i = 0;

        foreach (Text t in scores)
        {
            t.text = (i + 1) + ": " + (int)data.scores[i];
            i++;
        }

    }
}
