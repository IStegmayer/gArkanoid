using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        } else
        {
            _instance = this;
        }

        this.Lives = this.StartingLives;
        livesCounter.text = "Lives: " + this.Lives.ToString();
        Ball.OnBallDeath += OnBallDeath;
        Block.OnBrickDestruction += OnBrickDestruction;
    }

    #endregion

    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public Text livesCounter;
    public bool IsGameStarted { get; set; }
    public int StartingLives = 3;
    public int Lives { get; set; }

    private void OnBrickDestruction(Block obj)
    {
        if (BlocksManager.Instance.RemainingBlocks.Count <= 0)
        {
            BallsManager.Instance.ResetBalls();
            GameManager.Instance.IsGameStarted = false;
            BlocksManager.Instance.ClearRemainingBlocks();
            BlocksManager.Instance.LoadNextLevel();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnBallDeath(Ball obj)
    {
        if (BallsManager.Instance.Balls.Count <= 0)
        {
            Debug.Log("que apsa aca?");
            this.SubsctractLife();
            if (this.Lives < 1)
            {
                this.ShowGameOver();
            } else
            {
                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
                BlocksManager.Instance.LoadLevel(BlocksManager.Instance.currentLevel);
            }
        }
    }

    public void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
    }
    
    public void SubsctractLife()
    {
        this.Lives--;
        livesCounter.text = "Lives: " + this.Lives.ToString();
    }

    internal void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }

    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;
    }
}
