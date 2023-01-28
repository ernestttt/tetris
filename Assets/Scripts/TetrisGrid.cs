using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGrid : MonoBehaviour
{
    private const int GRID_WIDTH = 10;
    private const int GRID_HEIGHT = 20;
    
    [SerializeField] private RectTransform grid;
    [SerializeField] private SpriteRenderer squarePrefab;
    [SerializeField] private Transform pointsObj;
    private Vector3 _startPos;
    private float _verticalStep;
    private float _horizontalStep;
    
    private readonly List<Transform> _freePoints = new List<Transform>();
    private readonly List<Transform> _activePoints = new List<Transform>();

    private void Start()
    {
        Vector3[] worldCorners = new Vector3[4];
        grid.GetWorldCorners(worldCorners);
        float width = worldCorners[2].x - worldCorners[1].x;
        float height = worldCorners[1].y - worldCorners[0].y;
        
        _verticalStep = height / GRID_HEIGHT;
        _horizontalStep = width / GRID_WIDTH;
        
        // set scale in prefab
        Bounds bounds = squarePrefab.localBounds;
        float boxScale = _horizontalStep / bounds.size.x;
        squarePrefab.transform.localScale = new Vector3(boxScale, boxScale, boxScale);
        
        _startPos = worldCorners[1] + 
                    new Vector3(bounds.extents.x * boxScale, -bounds.extents.y * boxScale);

        squarePrefab.gameObject.SetActive(false);
    }

    public void Draw(byte[,] matrix)
    {
        FreePoints();
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] != 0)
                {
                    Transform point = GetPoint();
                    point.position = _startPos + new Vector3(_horizontalStep * j, -_verticalStep * i);
                }
            }
        }
    }
    
    private void FreePoints()
    {
        _activePoints.ForEach(a => a.gameObject.SetActive(false));
        _freePoints.AddRange(_activePoints);
        _activePoints.Clear();
    }

    private Transform GetPoint()
    {
        Transform point;
        if (_freePoints.Count == 0)
        {
            point = Instantiate(squarePrefab, pointsObj).transform;
        }
        else
        {
            int lastIndex = _freePoints.Count - 1;
            point = _freePoints[lastIndex];
            _freePoints.RemoveAt(lastIndex);
        }

        _activePoints.Add(point);
        point.gameObject.SetActive(true);
        
        return point;
    }
}
