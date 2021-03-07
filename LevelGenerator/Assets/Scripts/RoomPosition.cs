using UnityEngine;

public class RoomPosition : MonoBehaviour
{
    [SerializeField] private LayerMask roomMask;
    
    private void OnEnable()
    {
        LevelGenerator.OnGenerationComplete += SpawnFillRoomIfEmpty;
    }

    private void OnDisable()
    {
        LevelGenerator.OnGenerationComplete -= SpawnFillRoomIfEmpty;
    }

    private void SpawnFillRoomIfEmpty(LevelGenerator generator)
    {
        Collider2D roomDetect = Physics2D.OverlapCircle(transform.position, 1f, roomMask);

        if (roomDetect == null)
        {
            int rand = Random.Range(0, 4);
            generator.SpawnRoom(rand, transform.position);
        }
        
        //Debug.Log($"Filled room @ {transform.position}");
    }
}
