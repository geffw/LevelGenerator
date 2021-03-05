using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject outerBorderPrefab;
    [SerializeField] private GameObject roomPosPrefab;
    
    [SerializeField] private int gridSize = 4;
    
    [SerializeField] private Transform[] startingPositions;

    private void Start()
    {
        SpawnOuterBorders();

        int randStartPos = Random.Range(0, startingPositions.Length);

        transform.position = startingPositions[randStartPos].position;
    }

    private void SpawnOuterBorders()
    {
        Vector3 outerBorderBottomPos = new Vector3(transform.position.x + gridSize * (10f / 2f) + 0.5f,
            transform.position.y - 0.5f, transform.position.z);
        GameObject outerBorderBottom = Instantiate(outerBorderPrefab, outerBorderBottomPos, Quaternion.identity);
        outerBorderBottom.transform.localScale = new Vector3(gridSize * 10f + 1f, 1, 1);

        Vector3 outerBorderRightPos = new Vector3(transform.position.x + gridSize * 10f + 0.5f,
            transform.position.y + gridSize * (10f / 2f) + 0.5f, transform.position.z);
        GameObject outerBorderRight =
            Instantiate(outerBorderPrefab, outerBorderRightPos, Quaternion.Euler(new Vector3(0f, 0f, 90f)));
        outerBorderRight.transform.localScale = new Vector3(gridSize * 10f + 1f, 1, 1);

        Vector3 outerBorderTopPos = new Vector3(transform.position.x + gridSize * (10f / 2f) - 0.5f,
            transform.position.y + gridSize * 10f + 0.5f, transform.position.z);
        GameObject outerBorderTop = Instantiate(outerBorderPrefab, outerBorderTopPos, Quaternion.identity);
        outerBorderTop.transform.localScale = new Vector3(gridSize * 10f + 1f, 1, 1);

        Vector3 outerBorderLeftPos = new Vector3(transform.position.x - 0.5f,
            transform.position.y + gridSize * (10f / 2f) - 0.5f, transform.position.z);
        GameObject outerBorderLeft =
            Instantiate(outerBorderPrefab, outerBorderLeftPos, Quaternion.Euler(new Vector3(0f, 0f, 90f)));
        outerBorderLeft.transform.localScale = new Vector3(gridSize * 10f + 1f, 1, 1);
    }
}
