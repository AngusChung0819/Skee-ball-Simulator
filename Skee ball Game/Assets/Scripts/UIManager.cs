using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Button Action: Start new game
    public void StartNewGame() {
        Debug.Log("Start New Game");
        SceneManager.LoadScene("MainGame");
    }

    //// Button Action: Quit Game
    //public void QuitGame() {
    //    Debug.Log("Quit");
    //    Application.Quit();
    //}

    // Button Action: Pause Game
    public void PauseGame() {
        gameManager.gameStage = GameManager.GameStage.Pause;
    }

    // Button Action: Resume Game 
    public void ResumeGame() {
        gameManager.gameStage = GameManager.GameStage.Gameplay;
    }

    // Button Action: Back to Home Meun
    public void HomeMeun() {
        SceneManager.LoadScene("MainGame");
    }

    // Button Action: From main menu to Leaderboard
    public void EnterLeaderboard() {

    }

    // Button Action: From Leaderboard to main menu
    public void ExitLeaderboard() {

    }

}
