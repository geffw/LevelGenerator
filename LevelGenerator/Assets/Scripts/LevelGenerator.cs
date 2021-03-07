using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject outerBorderPrefab;
    [SerializeField] private GameObject roomPositionPrefab;
    [SerializeField] private GameObject[] roomPrefabs; // 0 = LR, 1 = LRB, 2 = LRT, 3 = LRTB

    [SerializeField] private int gridSize = 4;

    public static event Action<LevelGenerator> OnGenerationComplete;
    
    private List<GameObject> roomList = new List<GameObject>();
    private GameObject rooms;

    private bool generating;
    
    private int direction;
    private int currentLayer;
    private int downMoveCounter;
    
    private float roomSpawnRate = 0.25f;
    private float roomSpawnTimer;

    private void Start()
    {
        SpawnOuterBorders();
        SpawnRoomPositions();
        StartGeneration();
    }

    private void Update()
    {
        if (roomSpawnTimer <= 0f && generating)
        {
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
            downMoveCounter = 0;
            
            if (transform.position.x > 5f)
            {
                Vector2 newPos = new Vector2(transform.position.x - 10f, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, roomPrefabs.Length);
                SpawnRoom(rand, transform.position);

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
            downMoveCounter = 0;
            
            if (transform.position.x < gridSize * 10f - 5f)
            {
                Vector2 newPos = new Vector2(transform.position.x + 10f, transform.position.y);
                transform.position = newPos;
                
                int rand = Random.Range(0, roomPrefabs.Length);
                SpawnRoom(rand, transform.position);

                direction = Random.Range(3, 6);
            }
            else
            {
                direction = 5;
            }
        }
        else if (direction == 5)
        {
            downMoveCounter++;
            currentLayer++;
            
            if (transform.position.y < gridSize * 10f - 5f)
            {
                Room previousRoom = roomList[roomList.Count - 1].GetComponent<Room>();
                
                if (downMoveCounter >= 2)
                {
                    previousRoom.Destroy();
                    SpawnRoom(3, transform.position);
                }
                else
                {
                    if (previousRoom.type != 2 && previousRoom.type != 3)
                    {
                        previousRoom.Destroy();
                    
                        int randTopRoom = Random.Range(2, 4);
                        SpawnRoom(randTopRoom, transform.position);
                    }
                }

                Vector2 newPos = new Vector2(transform.position.x, transform.position.y + 10f);
                transform.position = newPos;

                int rand = Random.Range(1, 4);
                if (rand == 2)
                {
                    rand = 1;
                }
                SpawnRoom(rand, transform.position);

                direction = Random.Range(1, 6);
            }
            else
            {
                generating = false;
                //Debug.Log("End reached!");
                
                OnGenerationComplete?.Invoke(this);
            }
        }
    }

    public void SpawnRoom(int type, Vector3 pos)
    {
        GameObject room = Instantiate(roomPrefabs[type], pos, Quaternion.identity);
        roomList.Add(room);
        room.transform.parent = rooms.transform;
        room.transform.name = $"Room{roomList.IndexOf(room)}";
        room.GetComponent<Room>().difficulty = currentLayer;

        //Debug.Log($"LevelGenerator - Room (type: {room.GetComponent<Room>().type}) spawned at: {transform.position}");
    }

    private void StartGeneration()
    {
        rooms = new GameObject();
        rooms.transform.name = "Rooms";

        int randStartPos = Random.Range(0, gridSize);
        transform.position = new Vector2(transform.position.x + randStartPos * 10f + 5f, 5f);

        SpawnRoom(0, transform.position);

        direction = Random.Range(1, 6);
        generating = true;

        //Debug.Log($"LevelGenerator - Start pos: {transform.position}");
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

    private void SpawnRoomPositions()
    {
        GameObject roomPositions = new GameObject();
        roomPositions.transform.name = "RoomPositions";

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector2 roomPos = new Vector2(transform.position.x + 10f * x + 5f, transform.position.y + 10f * y + 5f);
                GameObject roomPosObj = Instantiate(roomPositionPrefab, roomPositions.transform, true);
                roomPosObj.transform.position = roomPos;
                roomPosObj.transform.name = $"RoomPosition ({y}, {x})";
            }
        }
    }
}
