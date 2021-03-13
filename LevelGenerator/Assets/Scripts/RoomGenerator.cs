using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Generates the internal layout of rooms individually using a Cellular Automata method.
/// </summary>
[RequireComponent(typeof(Room))]
public class RoomGenerator : MonoBehaviour
{
    /// <summary>
    /// Spawn point prefab used for level geometry.
    /// </summary>
    [SerializeField] private GameObject spawnPointPrefab;
    /// <summary>
    /// Amount of the 'map' (internal room tiles) to fill.
    /// </summary>
    [SerializeField, Range(0, 100)] private int randomFillPercent = 40;
    /// <summary>
    /// Number of walls surrounding a tile to base CA conversion ('smoothing') on.
    /// </summary>
    [SerializeField] private int surroundingWallCheck = 5;
    /// <summary>
    /// Number of CA conversion ('smoothing') iterations.
    /// </summary>
    [SerializeField] private int smoothSteps = 5;
    
    private int[,] map;

    private int width = 10;
    private int height = 10;

    private List<Vector2> criticalPathTiles = new List<Vector2>();

    private void Start()
    {
        GenerateMap();
    }

    /// <summary>
    /// Generates the 'map' - internal room layout.
    /// </summary>
    private void GenerateMap()
    {
        map = new int[width, height];

        RoomType roomType = GetComponent<Room>().type;

        criticalPathTiles.Add(new Vector2(0, 3));
        criticalPathTiles.Add(new Vector2(0, 4));
        criticalPathTiles.Add(new Vector2(0, 5));
        criticalPathTiles.Add(new Vector2(0, 6));
        criticalPathTiles.Add(new Vector2(1, 3));
        criticalPathTiles.Add(new Vector2(1, 4));
        criticalPathTiles.Add(new Vector2(1, 5));
        criticalPathTiles.Add(new Vector2(1, 6));
        criticalPathTiles.Add(new Vector2(2, 3));
        criticalPathTiles.Add(new Vector2(2, 4));
        criticalPathTiles.Add(new Vector2(2, 5));
        criticalPathTiles.Add(new Vector2(2, 6));
        criticalPathTiles.Add(new Vector2(3, 4));
        criticalPathTiles.Add(new Vector2(3, 5));
        criticalPathTiles.Add(new Vector2(4, 4));
        criticalPathTiles.Add(new Vector2(4, 5));
        criticalPathTiles.Add(new Vector2(5, 4));
        criticalPathTiles.Add(new Vector2(5, 5));
        criticalPathTiles.Add(new Vector2(6, 4));
        criticalPathTiles.Add(new Vector2(6, 5));
        criticalPathTiles.Add(new Vector2(7, 3));
        criticalPathTiles.Add(new Vector2(7, 4));
        criticalPathTiles.Add(new Vector2(7, 5));
        criticalPathTiles.Add(new Vector2(7, 6));
        criticalPathTiles.Add(new Vector2(8, 3));
        criticalPathTiles.Add(new Vector2(8, 4));
        criticalPathTiles.Add(new Vector2(8, 5));
        criticalPathTiles.Add(new Vector2(8, 6));
        criticalPathTiles.Add(new Vector2(9, 3));
        criticalPathTiles.Add(new Vector2(9, 4));
        criticalPathTiles.Add(new Vector2(9, 5));
        criticalPathTiles.Add(new Vector2(9, 6));

        if (roomType == RoomType.LeftRightBottom || roomType == RoomType.LeftRightTopBottom)
        {
            criticalPathTiles.Add(new Vector2(3, 0));
            criticalPathTiles.Add(new Vector2(4, 0));
            criticalPathTiles.Add(new Vector2(5, 0));
            criticalPathTiles.Add(new Vector2(6, 0));
            criticalPathTiles.Add(new Vector2(3, 1));
            criticalPathTiles.Add(new Vector2(4, 1));
            criticalPathTiles.Add(new Vector2(5, 1));
            criticalPathTiles.Add(new Vector2(6, 1));
            //criticalPathTiles.Add(new Vector2(3, 2));
            criticalPathTiles.Add(new Vector2(4, 2));
            criticalPathTiles.Add(new Vector2(5, 2));
            //criticalPathTiles.Add(new Vector2(6, 2));
            criticalPathTiles.Add(new Vector2(4, 3));
            criticalPathTiles.Add(new Vector2(5, 3));
        }

        if (roomType == RoomType.LeftRightTop || roomType == RoomType.LeftRightTopBottom)
        {
            criticalPathTiles.Add(new Vector2(3, 9));
            criticalPathTiles.Add(new Vector2(4, 9));
            criticalPathTiles.Add(new Vector2(5, 9));
            criticalPathTiles.Add(new Vector2(6, 9));
            criticalPathTiles.Add(new Vector2(3, 8));
            criticalPathTiles.Add(new Vector2(4, 8));
            criticalPathTiles.Add(new Vector2(5, 8));
            criticalPathTiles.Add(new Vector2(6, 8));
            //criticalPathTiles.Add(new Vector2(3, 7));
            criticalPathTiles.Add(new Vector2(4, 7));
            criticalPathTiles.Add(new Vector2(5, 7));
            //criticalPathTiles.Add(new Vector2(6, 7));
            criticalPathTiles.Add(new Vector2(4, 6));
            criticalPathTiles.Add(new Vector2(5, 6));
        }
        
        RandomFillMap();

        for (int i = 0; i < smoothSteps; i++)
        {
            SmoothMap();
        }
        
        CreateSpawnPoints();
    }

    /// <summary>
    /// Iterates through all tiles and randomly fills them.
    /// </summary>
    private void RandomFillMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (criticalPathTiles.Contains(new Vector2(x, y)))
                {
                    map[x, y] = 0;
                    continue;
                }
                
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = Random.Range(0, 100) < randomFillPercent ? 1 : 0;
                }
            }
        }
    }

    /// <summary>
    /// 'Smooths' the map by checking the surrounding tiles of each tile, and conforming it to its neighbours.
    /// </summary>
    private void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (criticalPathTiles.Contains(new Vector2(x, y)))
                {
                    map[x, y] = 0;
                    continue;
                }
                
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                
                //Vector3 pos = new Vector3(-width / 2f + x + 0.5f, -height / 2f + y + 0.5f, 0f);
                //Debug.Log($"Smooth @ {pos}");

                // Surrounding wall check - If more surrounding tiles are already walls, this tile becomes a wall;
                // if fewer than four are, it becomes empty; if exactly four, it stays as it was
                if (neighbourWallTiles > surroundingWallCheck)
                {
                    map[x, y] = 1;
                }
                else if (neighbourWallTiles < surroundingWallCheck)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    /// <summary>
    /// Creates the spawn points that constitute actual level geometry.
    /// </summary>
    private void CreateSpawnPoints()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    GameObject spawnPoint =
                        Instantiate(spawnPointPrefab, transform.position, Quaternion.identity, transform);

                    Vector3 pos = new Vector3(-width / 2f + x + 0.5f, -height / 2f + y + 0.5f, 0f);
                    spawnPoint.transform.localPosition = pos;
                }
            }
        }
    }

    /// <summary>
    /// Returns the number of tiles surrounding a given tile that are wall tiles.
    /// </summary>
    /// <param name="gridX"></param>
    /// <param name="gridY"></param>
    /// <returns></returns>
    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;

        // Looping through a 3x3 grid centered on grid (X,Y)
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) // Otherwise we're outside the bounds of the map
                {
                    if (neighbourX != gridX || neighbourY != gridY) // Otherwise we're looking at the target grid tile
                    {
                        wallCount += map[neighbourX, neighbourY]; // If map [x, y] is a wall then its value is 1 and wall count will increase
                    }
                }
                else
                {
                    wallCount++; // If we are at the edge of the map, increase wall count to encourage growth of walls around the map border
                }
            }
        }

        return wallCount;
    }
}
