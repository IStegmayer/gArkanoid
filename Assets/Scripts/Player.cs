using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour
{
    #region Singleton

    private static Player _instance;

    public static Player Instance => _instance;

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

        mainCamera = FindObjectOfType<Camera>();
        paddleInitialY = this.transform.position.y;
        sr = GetComponentInChildren<SpriteRenderer>();
        raycastPlane = new Plane(Vector3.back, transform.position);
        leftWall = GameObject.FindGameObjectWithTag("LeftWall");
        rightWall = GameObject.FindGameObjectWithTag("RightWall");
    }

    #endregion

    public int shotSpeed;
    private Camera mainCamera;
    private float paddleInitialY;
    private float defaultPaddleWidthInPixels = 2f;
    private SpriteRenderer sr;
    private float maxShotX = 160; 
    private Plane raycastPlane;
    private GameObject leftWall;
    private GameObject rightWall;

    // Update is called once per frame
    void Update()
    {
        Ray screenPointToRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        raycastPlane.Raycast(screenPointToRay, out float distance);
        Vector3 raycastHitPoint = screenPointToRay.origin + screenPointToRay.direction * distance;
        Debug.DrawLine(raycastHitPoint, mainCamera.transform.position);

        PaddleMovement(raycastHitPoint.x);
    }

    private void PaddleMovement(float x)
    {
        float paddleShift = defaultPaddleWidthInPixels / 2;
        float leftClamp = leftWall.transform.position.x + paddleShift;
        float rightClamp = rightWall.transform.position.x - paddleShift;
        float mousePositionPixels = Mathf.Clamp(x, leftClamp, rightClamp);
        this.transform.position = new Vector3(mousePositionPixels, paddleInitialY, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>() != null)
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 paddleCenter = new Vector3(sr.transform.position.x, sr.transform.position.y);

            ballRb.velocity = Vector2.zero;
            float diff = paddleCenter.x - hitPoint.x;

            if (hitPoint.x < paddleCenter.x){
                ballRb.AddForce(new Vector2(-(Mathf.Clamp(Mathf.Abs(diff * this.shotSpeed), 0, this.maxShotX)), BallsManager.Instance.initialBallSpeed));
            }
            else
            {
                ballRb.AddForce(new Vector2((Mathf.Clamp(Mathf.Abs(diff * this.shotSpeed), 0, this.maxShotX)), BallsManager.Instance.initialBallSpeed));
            }

        }
    }
}
