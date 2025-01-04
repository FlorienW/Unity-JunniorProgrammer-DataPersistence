using System;
using System.Collections.Generic;
using System.IO;
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
    public TextMeshProUGUI scoreBoardText;
    

    //* Score class holding the scoreBoard dictionary items with seperated as keys and values. But why?
    //* Because JsonUtility.ToJson() is not able to handle dictionary.
    [Serializable]
    public class Score
    {
        public List<string> keys;
        public List<int> values;
    }
    
    public Score score = new Score();
    
    public void Start()
    {
        bestScoreText = GameObject.Find("Canvas").transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        scoreBoardText = GameObject.Find("Canvas").transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        LoadScoreBoard();
        //* If GameManager at the Menu and scoreBoard isn't empty chances the text.
        if (SceneManager.GetActiveScene().buildIndex == 0 && scoreBoard.Count > 0)
        {
            bestScoreText.text = $"Best Score : {scoreBoard.Keys.ElementAt(0)} : {scoreBoard.Values.ElementAt(0)}";
        }
        
    }
    
    
    //* GameManager Instance...
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
    
    //* Changing the current player name.
    public void SetPlayerName(string name)
    {
        playerName = name;
    }
    
    //* Comparing the current endgame score between the that player high score or is there any score has for that player name.
    //* If there is no score yet or current game score is higher than the current player's high score,
    //* changing or creating the high score is current score.
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

    //* Updates the scoreBoard Dictionary and changes the texts.
    public void UpdateScore()
    {
        if (scoreBoard.Count != 0)
        {
            //* Sorts the scoreBoard based by the values.
            scoreBoard = scoreBoard.OrderByDescending(key => key.Value).ToDictionary(entry => entry.Key, entry => entry.Value);
            //* Changes the bestScoreText.
            bestScoreText.text = $"Best Score : {scoreBoard.Keys.ElementAt(0)} : {scoreBoard.Values.ElementAt(0)}";
            //* Changes the scoreBoardText.
            string scoreBoardString = "Top Scores\n------------------------------";
            for (int i = 0; i < scoreBoard.Keys.Count; i++)
            {
                scoreBoardString += $"\n{scoreBoard.Keys.ElementAt(i)} : {scoreBoard.Values.ElementAt(i)}";
            }
            scoreBoardText.text = scoreBoardString;
            //* Saves the scoreBoards to Score() class by separates the dictionary.Keys and dictionary.Values.
            score.keys = scoreBoard.Keys.ToList();
            score.values = scoreBoard.Values.ToList();
            string jsonScore = JsonUtility.ToJson(score);
            File.WriteAllText(Application.persistentDataPath + "/score.json", jsonScore);
            
        }
        
    }

    //* Loads the savefile and instruct the scoreBoard from save.
    public void LoadScoreBoard()
    {
        string path = Application.persistentDataPath + "/score.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            score = JsonUtility.FromJson<Score>(json);
        }
        scoreBoard = score.keys.Zip(score.values, (key, value) => new { key, value }).ToDictionary(entry => entry.key, entry => entry.value);
        scoreBoard = scoreBoard.OrderByDescending(key => key.Value).ToDictionary(entry => entry.Key, entry => entry.Value);
    }
    

}
