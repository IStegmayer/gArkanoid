using System;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Block : MonoBehaviour
{
    public ParticleSystem DestroyEffect;

    public static event Action<Block> OnBrickDestruction;
    public Sprite[] sprites;
    public Color[] BlocksColors;

    private SpriteRenderer sr;

    private int blockType;

    private void Awake()
    {
        this.sr = this.GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            ApplyCollisionLogic(ball);
        }
    }

    private void ApplyCollisionLogic(Ball ball)
    {
        //TODO: There should be a method for each blockType
        if (this.blockType != 5 && this.blockType != 6)
        {
            BlocksManager.Instance.RemainingBlocks.Remove(this);
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        } 
        if (this.blockType == 5)
        {
            GameManager.Instance.SubsctractLife();
            if (GameManager.Instance.Lives < 1)
            {
                foreach (Ball activeBall in BallsManager.Instance.Balls)
                {
                    Destroy(activeBall.gameObject);
                }
                GameManager.Instance.ShowGameOver();
            }
        }
        OnBrickDestruction?.Invoke(this);
    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPos.x, brickPos.y, brickPos.z = 0.02f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }

    public void Init(Transform containerTransform, int blockType)
    {
        this.blockType = blockType;
        this.transform.SetParent(containerTransform);
        this.sr.sprite = this.sprites[this.blockType - 1];
        this.sr.color = this.BlocksColors[this.blockType - 1];
    }
}
