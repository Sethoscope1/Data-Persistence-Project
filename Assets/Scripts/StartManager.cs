using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartManager : MonoBehaviour
{
    public TMP_InputField playerName;

    public void StartGame()
    {
        ScoreManager.Instance.playerName = currentPlayerName();
        SceneManager.LoadScene(1);
    }

    public string currentPlayerName()
    {
        if (playerName.text == "")
        {
            return "ANON";
        } else {
            return playerName.text;
        }
    }
}
