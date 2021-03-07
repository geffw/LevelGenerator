using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject outerBorderPrefab;
    [SerializeField] private GameObject[] roomPrefabs; // 0 = LR, 1 = LRB, 2 = LRT, 3 = LRTB
    
    [SerializeField] private int gridSize = 4;
    
    private List<GameObject> roomList = new List<GameObject>();
    private GameObject rooms;

    private bool generating;
    
    private int direction;
    private float roomSpawnRate = 0.25f;
    private float roomSpawnTimer;

    private void Start()
    {
        SpawnOuterBorders();

        rooms = new GameObject();
        rooms.transform.name = "Rooms";

        int randStartPos = Random.Range(0, gridSize);
        transform.position = new Vector2(transform.position.x + randStartPos * 10f + 5f, 5f);

        direction = Random.Range(1, 6);
        generating = true;

        Debug.Log($"LevelGenerator - Start pos: {transform.position}");
    }

    private void Update()
    {
        if (roomSpawnTimer <= 0f && generating)
        {
            SpawnRoom();
            Move();
            roomSpawnTimer = roomSpawnRate;
        }
        else
        {
            roomSpawnTimer -= Time.deltaTime;
        }
    }

    private void Move()
    {
        if (direction == 1 || direction == 2)
        {
            if (transform.position.x > 5f)
            {
                Vector2 newPos = new Vector2(transform.position.x - 10f, transform.position.y);
                transform.position = newPos;

                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    direction = 2;
                }
                else if (direction == 4)
                {
                    direction = 5;
                }
            }
            else
            {
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4)
        {
            if (transform.position.x < gridSize * 10f - 5f)
            {
                Vector2 newPos = new Vector2(transform.position.x + 10f, transform.position.y);
                transform.position = newPos;

                direction = Random.Range(3, 6);
            }
            else
            {
                direction = 5;
            }
        }
        else if (direction == 5)
        {
            if (transform.position.y < gridSize * 10f - 5f)
            {
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y + 10f);
                transform.position = newPos;

                direction = Random.Range(1, 6);
            }
            else
            {
                generating = false;
                Debug.Log("End reached!");
            }
        }
    }

    private void SpawnRoom()
    {
        GameObject room = Instantiate(roomPrefabs[0], transform.position, Quaternion.identity);
        roomList.Add(room);
        room.transform.parent = rooms.transform;
        room.transform.name = $"Room{roomList.IndexOf(room)}";
        
        Debug.Log($"LevelGenerator - Room spawned at: {transform.position}");
    }

    private void SpawnOuterBorders()
    {
        GameObject outerBorders = new GameObject();
        outerBorders.transform.name = "OuterBorders";

        Vector2 outerBorderBottomPos = new Vector2(transform.position.x + gridSize * (10f / 2f) + 0.5f, transform.position.y - 0.5f);
        GameObject outerBorderBottom = Instantiate(outerBorderPrefab, outerBorderBottomPos, Quaternion.identity);
        outerBorderBottom.transform.localScale = new Vector3(gridSize * 10f + 1f, 1, 1);
        outerBorderBottom.transform.parent = outerBorders.transform;

        Vector2 outerBorderRightPos = new Vector2(transform.position.x + gridSize * 10f + 0.5f, transform.position.y + gridSize * (10f / 2f) + 0.5f);
        GameObject outerBorderRight =
            Instantiate(outerBorderPrefab, outerBorderRightPos, Quaternion.Euler(new Vector3(0f, 0f, 90f)));
        outerBorderRight.transform.localScale = new Vector3(gridSize * 10f + 1f, 1, 1);
        outerBorderRight.transform.parent = outerBorders.transform;

        Vector2 outerBorderTopPos = new Vector2(transform.position.x + gridSize * (10f / 2f) - 0.5f, transform.position.y + gridSize * 10f + 0.5f);
        GameObject outerBorderTop = Instantiate(outerBorderPrefab, outerBorderTopPos, Quaternion.identity);
        outerBorderTop.transform.localScale = new Vector3(gridSize * 10f + 1f, 1, 1);
        outerBorderTop.transform.parent = outerBorders.transform;

        Vector2 outerBorderLeftPos = new Vector2(transform.position.x - 0.5f, transform.position.y + gridSize * (10f / 2f) - 0.5f);
        GameObject outerBorderLeft =
            Instantiate(outerBorderPrefab, outerBorderLeftPos, Quaternion.Euler(new Vector3(0f, 0f, 90f)));
        outerBorderLeft.transform.localScale = new Vector3(gridSize * 10f + 1f, 1, 1);
        outerBorderLeft.transform.parent = outerBorders.transform;
    }
}
