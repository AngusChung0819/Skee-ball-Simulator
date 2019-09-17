using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour {

    public GameManager gameManager;

    public List<Text> scoreTexts = new List<Text>();
    public List<int> scores = new List<int>();

    // Start is called before the first frame update
    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); ;
        LoadLeaderBoardPP();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnEnable() {
        LoadLeaderBoardPP();
    }

    public void NewScore(int score) {
        Debug.Log(scores.Count);
        if (scores.Count >= 5) {
            if (score > scores[scoreTexts.Count - 1]) {
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

    public void UpdateLeaderBoardPP() {
        for (int i = 0; i < scores.Count; i++) {
            PlayerPrefs.SetInt("Top" + (i + 1), scores[i]);
            scoreTexts[i].text = scores[i].ToString();
        }
    }

    public void LoadLeaderBoardPP() {
        scores = new List<int>();
        for (int i = 0; i < scoreTexts.Count; i++) {
            scores.Add(PlayerPrefs.GetInt("Top" + (i + 1), 0));
            scoreTexts[i].text = scores[i].ToString();
        }
    }

    public void ClearRecords() {
        PlayerPrefs.DeleteAll();
    }

}
