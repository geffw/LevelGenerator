using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject outerBorderPrefab;
    [SerializeField] private GameObject[] roomPrefabs; // 0 = LR, 1 = LRB, 2 = LRT, 3 = LRTB
    
    [SerializeField] private int gridSize = 4;
    
    private List<GameObject> roomList = new List<GameObject>();
    private GameObject rooms;

    private void Start()
    {
        SpawnOuterBorders();

        rooms = Instantiate(new GameObject(), transform);
        rooms.transform.name = "Rooms";

        int randStartPos = Random.Range(0, gridSize);

        SpawnFirstRoom(randStartPos);
    }

    private void SpawnOuterBorders()
    {
        GameObject outerBorders = Instantiate(new GameObject(), transform);
        outerBorders.transform.name = "OuterBorders";

        Vector3 outerBorderBottomPos = new Vector3(transform.position.x + gridSize * (10f / 2f) + 0.5f,
            transform.position.y - 0.5f, transform.position.z);
        GameObject outerBorderBottom = Instantiate(outerBorderPrefab, outerBorderBottomPos, Quaternion.identity);
        outerBorderBottom.transform.localScale = new Vector3(gridSize * 10f + 1f, 1, 1);
        outerBorderBottom.transform.parent = outerBorders.transform;

        Vector3 outerBorderRightPos = new Vector3(transform.position.x + gridSize * 10f + 0.5f,
            transform.position.y + gridSize * (10f / 2f) + 0.5f, transform.position.z);
        GameObject outerBorderRight =
            Instantiate(outerBorderPrefab, outerBorderRightPos, Quaternion.Euler(new Vector3(0f, 0f, 90f)));
        outerBorderRight.transform.localScale = new Vector3(gridSize * 10f + 1f, 1, 1);
        outerBorderRight.transform.parent = outerBorders.transform;

        Vector3 outerBorderTopPos = new Vector3(transform.position.x + gridSize * (10f / 2f) - 0.5f,
            transform.position.y + gridSize * 10f + 0.5f, transform.position.z);
        GameObject outerBorderTop = Instantiate(outerBorderPrefab, outerBorderTopPos, Quaternion.identity);
        outerBorderTop.transform.localScale = new Vector3(gridSize * 10f + 1f, 1, 1);
        outerBorderTop.transform.parent = outerBorders.transform;

        Vector3 outerBorderLeftPos = new Vector3(transform.position.x - 0.5f,
            transform.position.y + gridSize * (10f / 2f) - 0.5f, transform.position.z);
        GameObject outerBorderLeft =
            Instantiate(outerBorderPrefab, outerBorderLeftPos, Quaternion.Euler(new Vector3(0f, 0f, 90f)));
        outerBorderLeft.transform.localScale = new Vector3(gridSize * 10f + 1f, 1, 1);
        outerBorderLeft.transform.parent = outerBorders.transform;
    }

    private void SpawnFirstRoom(int pos)
    {
        GameObject room = Instantiate(roomPrefabs[0], new Vector3(transform.position.x + pos * 10f + 5f, 5f, 0f), Quaternion.identity);
        roomList.Add(room);
        room.transform.parent = rooms.transform;
        room.transform.name = $"Room{roomList.IndexOf(room)}";
    }
}
