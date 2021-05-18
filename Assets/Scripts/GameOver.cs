using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverUI;
    public static GameObject gameOverUIStatic;

    void Start()
    {
        GameOver.gameOverUIStatic = gameOverUI;
        gameOverUIStatic.SetActive(false);
    }

    public static void ShowGameOver()
    {
        gameOverUIStatic.SetActive(true);
    }

}
