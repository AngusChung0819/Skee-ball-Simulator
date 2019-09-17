using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    // Singleton Setup
    public static GameManager instance = null;

    // Initial Variables
    public enum GameStage {
        MainMenu,
        CountDown,
        Gameplay,
        Pause,
        Result
    };
    public GameStage gameStage;

    // Gameplay Variables
    public int counter = 3;
    public float gameTime = 30;
    private float timeLeft = 30;
    public int score = 0; 
    public float gameSpeed = 0;
    public int highestScore;
    public int lowestScore;
    public GameObject skeeball;

    // UI Elements //
    // Main Menu 
    public GameObject mainMenu;
    public Text highScoreText;
    public GameObject leaderBoardUI;
    // CountDown UI
    public GameObject cover;
    public Text counterText;
    // Gameplay UI
    public TextMesh timerText;
    public TextMesh scoreText;
    // Pause Menu
    public GameObject pauseMenu;
    // Result Page
    public GameObject resultPage;
    public LeaderBoard leaderBoard;
    public Text position;
    public Text scoreFinal;
    public Text message;
    public GameObject restartButton;

    // Awake Checks - Singleton setup
    void Awake() {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start() {
        // Scene: Main Meun
        if (gameStage == GameStage.MainMenu) {
            leaderBoardUI.GetComponent<LeaderBoard>().LoadLeaderBoardPP();
            highestScore = leaderBoardUI.GetComponent<LeaderBoard>().scores[0];
            highScoreText.text = highestScore.ToString();
        }
        // Scene: Main Game
        if (gameStage == GameStage.CountDown) {
            StartCoroutine(CountDownToGameplay());
            highestScore = PlayerPrefs.GetInt("Top1",0);
            lowestScore = PlayerPrefs.GetInt("Top5", 0);
        }
    }

    // Update is called once per frame
    void Update() {
        // Gamee Stages
        switch (gameStage) {
            case GameStage.MainMenu:
                // Use of Escape button
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    if (leaderBoardUI.activeSelf) {
                        ExitLeaderboard();
                    } else {
                        QuitGame();
                    }
                }
                break;
            case GameStage.CountDown:
                // Nothing to do
                gameSpeed = 0;
                break;
            case GameStage.Gameplay:
                // Game loop
                gameSpeed = 1;
                UpdateScoreAndTime();
                if (timeLeft <= 0) {
                    skeeball.GetComponent<SkeeBall>().PauseBall();
                    gameStage = GameStage.Result;
                    ShowResult();
                }
                // Use of Escape button
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    PauseGame();
                }
                break;
            case GameStage.Pause:
                // Use of Escape button
                gameSpeed = 0;
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    ResumeGame();
                }
                break;
            case GameStage.Result:
                // Use of Escape button
                gameSpeed = 0;
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    HomeMeun();
                }
                break;
        }

        // Delete all record on the local device
        if (Input.GetKeyDown("r")) {
            PlayerPrefs.DeleteAll();
            highestScore = 0;
            lowestScore = 0;
            highScoreText.text = highestScore.ToString();
        }


    }

    // From CountDown Stage to Gameplay Stage
    public IEnumerator CountDownToGameplay() {
        cover.SetActive(true);
        for (int i = counter; i > 0; i--) {
            // Count Down
            counterText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        counterText.text = "GO!";
        yield return new WaitForSeconds(1f);
        // Start the game
        cover.SetActive(false);
        gameStage = GameStage.Gameplay;
    }

    // Reset Score and Time
    public void ResetScoreAndTime() {
        timeLeft = gameTime;
        score = 0;
    }

    // Update Score and Time displayment
    public void UpdateScoreAndTime() {
        timeLeft -= 1 * Time.deltaTime * gameSpeed;
        timerText.text = timeLeft.ToString("0");
        scoreText.text = score.ToString();
    }

    // Show Result Page
    public void ShowResult() {
        resultPage.SetActive(true);
        // update message depends on score
        if (score >= highestScore) {
            // New highest Score
            message.text = "Amazing!\nYou got the highest score!";
            restartButton.SetActive(false);
        } else if (score >= lowestScore) {
            // Position at 2nd - 5th
            message.text = "You got a high score!\nNice Try!";
            restartButton.SetActive(true);
        } else {
            // Score Too Low
            position.text = "---";
            message.text = "Your score is low.\nTry again!";
            restartButton.SetActive(true);
        }
        // Show final score
        scoreFinal.text = score.ToString();
        // Update Leaderboard
        leaderBoard = resultPage.GetComponent<LeaderBoard>();
        leaderBoard.NewScore(score);
        // Check and Show position
        for (int i = leaderBoard.scores.Count - 1; i >= 0; i--) {
            if (score >= leaderBoard.scores[i]) {
                switch (i) {
                    case 0:
                        position.text = "1st";
                        break;
                    case 1:
                        position.text = "2nd";
                        break;
                    case 2:
                        position.text = "3rd";
                        break;
                    case 3:
                        position.text = "4th";
                        break;
                    case 4:
                        position.text = "5th";
                        break;
                }
            }
        }
    }
    
    // ----------- Button Section ---------- //
    // Button Action: Start new game
    public void StartNewGame() {
        Debug.Log("Start New Game");
        ResetScoreAndTime();
        SceneManager.LoadScene("MainGame");
    }

    // Button Action: Quit Game (no Quit Button in game, just for the android Escape button)
    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

    // Button Action: Pause Game
    public void PauseGame() {
        gameStage = GameStage.Pause;
        pauseMenu.SetActive(true);
        skeeball.GetComponent<SkeeBall>().PauseBall();
    }

    // Button Action: Resume Game 
    public void ResumeGame() {
        gameStage = GameStage.Gameplay;
        pauseMenu.SetActive(false);
        skeeball.GetComponent<SkeeBall>().ResumeBall();
    }

    // Button Action: Back to Home Meun
    public void HomeMeun() {
        gameStage = GameStage.MainMenu;
        SceneManager.LoadScene("MainMenu");
    }

    // Button Action: From main menu to Leaderboard
    public void EnterLeaderboard() {
        mainMenu.SetActive(false);
        leaderBoardUI.SetActive(true);
    }

    // Button Action: From Leaderboard to main menu
    public void ExitLeaderboard() {
        mainMenu.SetActive(true);
        leaderBoardUI.SetActive(false);
    }

}
