using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public GameObject gameOverModal;
    
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void GameOver(String text)
    {
        Time.timeScale = 0;
        gameOverModal.SetActive(true);
        gameOverModal.transform.GetChild(1).GetComponent<Text>().text = text;
    }
}
