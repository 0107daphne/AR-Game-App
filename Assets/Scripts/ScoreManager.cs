using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TMP_Text scoreText;
    public TMP_Text highscoreText;
    public TMP_Text console;

    int score =0;
    int highscore = 0;

    private void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        score =0;
        highscore= PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = $"{score} POINTS";
        highscoreText.text = $"HIGHSCORE: {highscore}";
        
    }

    
    public void AddPoint()
    {
        score += 1;
        scoreText.text = $"{score} POINTS";
        
        if (highscore < score)
        {
            highscore = score;
            PlayerPrefs.SetInt("highscore", score);
            highscoreText.text = $"HIGHSCORE: {highscore}";
        }
    }
    public void Restart()
        {
            score =0;
            scoreText.text = $"{score} POINTS"; 
            
        }
    
    public void GameOver()
    {
        PlayfabManager playfabManager = GameObject.Find("PlayfabManager").GetComponent<PlayfabManager>();
        playfabManager.SendLeaderboard(score);
    }
    
}
