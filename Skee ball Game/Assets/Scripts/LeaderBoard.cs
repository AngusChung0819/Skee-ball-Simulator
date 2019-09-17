using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour {

    // Game Manager
    public GameManager gameManager;

    // Score Variables
    public List<Text> scoreTexts = new List<Text>();
    public List<int> scores = new List<int>();

    // Start is called before the first frame update
    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        LoadLeaderBoardPP();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnEnable() {
        LoadLeaderBoardPP();
    }

    // Add new Score to top 5 list if it is higher then the current 5th score
    public void NewScore(int score) {
        Debug.Log(scores.Count);
        if (scores.Count >= 5) {
            if (score > scores[scoreTexts.Count - 1]) {
                // Remove the current 5th score
                scores.RemoveAt(scoreTexts.Count - 1);
                scores.Add(score);
                SortScores();
                UpdateLeaderBoardPP();
            }
        } else {
            scores.Add(score);
            SortScores();
            UpdateLeaderBoardPP();
        }
    }

    // Sort the Top 5 score list from highest to lowest
    public void SortScores() {
        for (int i = scores.Count - 1; i > 0; i--) {
            if (scores[i] > scores[i - 1]) {
                // swap the socre position
                int smaller = scores[i - 1];
                scores[i - 1] = scores[i];
                scores[i] = smaller;
            }
        }
    }

    // Update the score texts and PlayerPrefs
    public void UpdateLeaderBoardPP() {
        for (int i = 0; i < scores.Count; i++) {
            PlayerPrefs.SetInt("Top" + (i + 1), scores[i]);
            scoreTexts[i].text = scores[i].ToString();
        }
    }

    // Reload the Top 5 score from PlayerPrefs
    public void LoadLeaderBoardPP() {
        scores = new List<int>();
        for (int i = 0; i < scoreTexts.Count; i++) {
            scores.Add(PlayerPrefs.GetInt("Top" + (i + 1), 0));
            scoreTexts[i].text = scores[i].ToString();
        }
    }

    // Clear the PlayerPrefs
    public void ClearRecords() {
        PlayerPrefs.DeleteAll();
    }

}
