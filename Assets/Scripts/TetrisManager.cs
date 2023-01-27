using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris;
using TMPro;
using UnityEngine.UI;

public class TetrisManager : MonoBehaviour
{
    private readonly TetrisCore _tetris = new TetrisCore();

    [SerializeField] private RectTransform grid;
    [SerializeField] private SpriteRenderer squarePrefab;
    [SerializeField] private float step = 0.5f;
    [SerializeField] private Transform pointsObj;
    [SerializeField] private TextMeshProUGUI scoreText;

    private float _nextTime = 0;
    private float verticalStep;
    private float horizontalStep;
    private Vector3 startPos;

    private List<Transform> freePoints = new List<Transform>();
    private List<Transform> activePoints = new List<Transform>();

    private bool isPlaying;
    

    private void Start()
    {
        Vector3[] worldCorners = new Vector3[4];
        grid.GetWorldCorners(worldCorners);
        float width = worldCorners[2].x - worldCorners[1].x;
        float height = worldCorners[1].y - worldCorners[0].y;
        
        verticalStep = height / 20;
        horizontalStep = width / 10;
        Bounds bounds = squarePrefab.localBounds;
        float boxScale = horizontalStep / bounds.size.x;
        squarePrefab.transform.localScale = new Vector3(boxScale, boxScale, boxScale);
        startPos = worldCorners[1] + 
                   new Vector3(bounds.extents.x * boxScale, -bounds.extents.y * boxScale);

        squarePrefab.gameObject.SetActive(false);

        _tetris.GameOverEvent += () =>
        {
            isPlaying = false;
            Debug.Log("Game Over");
        };

        isPlaying = true;
    }

    private void Update()
    {
        if (!isPlaying) 
            return;
        CheckInput();
        if (Time.time > _nextTime)
        {
            _tetris.MoveDown();
            DrawField(_tetris.ViewMatrix);
            _nextTime = Time.time + step;
            scoreText.text = $"Completed rows: {_tetris.CompletedRows}";
        }
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _tetris.MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _tetris.MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _tetris.Rotate();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _tetris.MoveDown();
        }
    }

    private void DrawField(byte[,] matrix)
    {
        FreePoints();
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] != 0)
                {
                    Transform point = GetPoint();
                    point.position = startPos + new Vector3(horizontalStep * j, -verticalStep * i);
                }
            }
        }
    }

    private void FreePoints()
    {
        activePoints.ForEach(a => a.gameObject.SetActive(false));
        freePoints.AddRange(activePoints);
        activePoints.Clear();
    }

    private Transform GetPoint()
    {
        Transform point;
        if (freePoints.Count == 0)
        {
            point = Instantiate(squarePrefab, pointsObj).transform;
            activePoints.Add(point.transform);
        }
        else
        {
            int lastIndex = freePoints.Count - 1;
            point = freePoints[lastIndex];
            freePoints.RemoveAt(lastIndex);
            activePoints.Add(point);
        }
        
        point.gameObject.SetActive(true);
        return point;
    }
}
