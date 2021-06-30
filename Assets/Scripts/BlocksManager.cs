using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksManager : MonoBehaviour
{
    #region Singleton

    private static BlocksManager _instance;

    public static BlocksManager Instance => _instance;

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
    }

    #endregion

    private int maxRows = 23;
    private int maxCols = 14;
    private GameObject BlocksContainer;
    // these 3 should be private
    public float initialBlockSpawnPositionX = 0.2f;
    public float initialBlockSpawnPositionY = 3.325f;
    public float shiftAmountY; // TODO: get height of brick spawn 
    public float shiftAmountX; // TODO: get width of brick spawn 

    public Block BlockPrefab;

    public List<Block> RemainingBlocks { get; set; }
    public List<int[,]> levelsData { get; set; }

    public int InitialBlocksCount { get; set; }

    public void LoadNextLevel()
    {
        this.currentLevel++;

        if (this.currentLevel >= this.levelsData.Count)
        {
            GameManager.Instance.ShowVictoryScreen();
        } else
        {
            this.LoadLevel(this.currentLevel);
        }
    }

    public int currentLevel;

    private void Start()
    {
        this.BlocksContainer = new GameObject("BlocksContainer");
        this.levelsData = this.LoadLevelsData();
        this.GenerateBricks();
    }

    public void LoadLevel(int currentLevel)
    {
        this.currentLevel = currentLevel;
        this.ClearRemainingBlocks();
        this.GenerateBricks();
    }

    public void ClearRemainingBlocks()
    {
        foreach (GameObject block in GameObject.FindGameObjectsWithTag("Block"))
        {
            Destroy(block);
        }
    }

    private void GenerateBricks()
    {
        this.RemainingBlocks = new List<Block>();
        int[,] currentLevelData = this.levelsData[this.currentLevel];
        float currentSpawnX = initialBlockSpawnPositionX;
        float currentSpawnY = initialBlockSpawnPositionY;
        float zShift = 0;

        for (int row = 0; row < this.maxRows; row++)
        {
            for (int col = 0; col < this.maxCols; col++)
            {
                int blockType = currentLevelData[row, col];

                if (blockType > 0)
                {
                    Block newBlock = Instantiate(BlockPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Block;
                    newBlock.Init(BlocksContainer.transform, blockType);

                    if (blockType != 5 && blockType != 6) this.RemainingBlocks.Add(newBlock);
                    zShift += 0.00001f;
                }

                currentSpawnX += shiftAmountX;
                if (col + 1 == this.maxCols){
                    currentSpawnX = initialBlockSpawnPositionX;
                }
            }

            currentSpawnY -= shiftAmountY;
        }

        this.InitialBlocksCount = this.RemainingBlocks.Count;
    }

    private List<int[,]> LoadLevelsData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;

        string[] rows = text.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols];
        int currentRow = 0;

        for (int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];

            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }

                currentRow++;
            } else
            {
                // end of current level
                // add the matrix to the last and continue the loop
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int[maxRows, maxCols];
            }
        }

        return levelsData;
    }
}
