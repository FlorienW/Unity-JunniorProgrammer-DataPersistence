using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public string playerName;
    public Dictionary<string, int> scoreBoard = new Dictionary<string, int>();
    public TextMeshProUGUI bestScoreText;
    public GameObject debug;

    public void Start()
    {
        debug = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        bestScoreText = debug.GetComponent<TextMeshProUGUI>();
        
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public void SetPlayerScore(int score)
    {
        int dictionaryRegister = scoreBoard.GetValueOrDefault(playerName, -1);
        
        if (dictionaryRegister == -1)
        {
            scoreBoard.Add(playerName, score);
        }
        
        else if (score > dictionaryRegister)
        {
            scoreBoard[playerName] = score;
        }
        
        UpdateScore();
    }

    public void UpdateScore()
    {
        if (scoreBoard.Count != 0)
        {
            scoreBoard = scoreBoard.OrderBy(key => key.Value).ToDictionary(entry => entry.Key, entry => entry.Value);
            bestScoreText.text = $"Best Score : {scoreBoard.Keys.ElementAt(0)} : {scoreBoard.Values.ElementAt(0)} ";
        }
        
    }
    

}
