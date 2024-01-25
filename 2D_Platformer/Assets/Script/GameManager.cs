using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameManager>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        UpdateUICoin();
        UpdateHighScore();
        curCoin = 0;
    }
    private void Update()
    {
        UpdateHighScore();
    }
    private int curCoin;
    [SerializeField] Text txtCoin, txtScore;
    private int curScore;
    private void UpdateHighScore()
    {
        if (SceneManager.GetActiveScene().name == "FinishGame")
        {
            HighScore = curScore;
        }
    }
    private int HighScore
    {
        get => PlayerPrefs.GetInt("high_Score", 0);
        set
        {
            if (value > PlayerPrefs.GetInt("high_Score", 0))
                PlayerPrefs.SetInt("high_Score", value);
        }
    }
    public int GetHighScore()
    {
        return HighScore;
    }
    public void AddScore(int point)
    {
        curScore += point;
        UpdateUIScore();
    }
    public void AddCoin()
    {
        curCoin += 1;
        UpdateUICoin();
    }
    void UpdateUICoin() => txtCoin.text = curCoin.ToString();
    void UpdateUIScore() => txtScore.text = curScore.ToString();
    //Pause Game
    
    public void DestroyGO()
    {
        Destroy(gameObject);
    }
}
