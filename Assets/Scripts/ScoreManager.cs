using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public string playerName;

    public List<HighScore> highScores = new List<HighScore>();
    public string highScorePlayerName;
    public int highScore;
    public bool newHighScore = false;
    public TMP_Text highScoreField;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        LoadHighScore();

        highScoreField.text = highScorePlayerName + "   |   " + highScore;
        DontDestroyOnLoad(gameObject);
    }
    
    public void UpdateHighScores(int score)
    {
        if (highScores.Count == 0 ||score > highScores.Last().score)
        {
            newHighScore = true;
        }

        if (highScores.Count == 0 || (score > highScores[0].score))
        {
            highScore = score;
            highScorePlayerName = playerName;
        }
    }

    [System.Serializable]
    public class HighScore
    {
        public string playerName;
        public int score;
    }

    [System.Serializable]
    class SaveData
    {
        public List<HighScore> highScores;

        public string highScorePlayerName;
        public int highScore;
    }

    public void SaveHighScore(int score)
    {
        if (newHighScore || highScores.Count < 10)
        {
            HighScore scoreRecord = new HighScore();
            scoreRecord.playerName = playerName;
            scoreRecord.score = score;
            highScores.Add(scoreRecord);
            highScores = highScores.OrderByDescending(o => o.score).ToList();
        }

        SaveData data = new SaveData();
        data.highScores = highScores;
        data.highScore = highScores[0].score;
        data.highScorePlayerName = highScores[0].playerName;

        string json = JsonUtility.ToJson(data);
        highScores = data.highScores;
        highScore = data.highScore;
        highScorePlayerName = data.highScorePlayerName;
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            highScores = data.highScores;
            highScores = highScores.OrderByDescending(o => o.score).ToList();
            highScore = highScores[0].score;
            highScorePlayerName = highScores[0].playerName;

        }
    }

    public void DisplayGameOverScores()
    {
        TextMeshProUGUI names = GameObject.Find("ScoresNameText").GetComponent<TextMeshProUGUI>(); 
        TextMeshProUGUI scores = GameObject.Find("ScoresScoreText").GetComponent<TextMeshProUGUI>();
        
        highScores = highScores.OrderByDescending(o => o.score).ToList();

        for (int i = 0; i < highScores.Count; i++) 
        {
            names.text += highScores[i].playerName + "\n";
            scores.text += highScores[i].score + "\n";
        }
    }
}
