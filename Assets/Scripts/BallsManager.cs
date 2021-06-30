using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsManager : MonoBehaviour
{
    #region Singleton

    private static BallsManager _instance;

    public static BallsManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        InitBall();
    }

    #endregion


    [SerializeField]
    private Ball ballPrefab;
    private Ball initialBall;
    private Rigidbody2D initialBallRb;

    public List<Ball> Balls { get; set; }
    public float initialBallSpeed = 400;

    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted)
        {
            // Align ball position to player position
            Vector3 playerPosition = Player.Instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(playerPosition.x, playerPosition.y + 0.3f, 0);
            initialBall.transform.position = ballPosition;

            if (Input.GetMouseButtonDown(0))
            {
                initialBallRb.isKinematic = false;
                initialBallRb.AddForce(new Vector2(0, initialBallSpeed));
                GameManager.Instance.IsGameStarted = true;
            }
        }
    }

    internal void ResetBalls()
    {
        foreach (var ball in this.Balls)
        {
            Destroy(ball.gameObject);
        }

        InitBall();
    }

    private void InitBall()
    {
        Vector3 playerPosition = Player.Instance.gameObject.transform.position;
        Vector3 startingPosition = new Vector3(playerPosition.x, playerPosition.y + 0.3f, 0);// Get it from the Player
        initialBall = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
        initialBallRb = initialBall.GetComponent<Rigidbody2D>();

        this.Balls = new List<Ball>
        {
            initialBall
        };
    }
}
