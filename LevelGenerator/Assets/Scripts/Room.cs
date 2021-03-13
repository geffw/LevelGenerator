using UnityEngine;

public enum RoomType
{
    LeftRight = 0,
    LeftRightBottom = 1,
    LeftRightTop = 2,
    LeftRightTopBottom = 3
}

public class Room : MonoBehaviour
{
    public RoomType type;

    public int difficulty;

    private void Start()
    {
        difficulty = (int)transform.position.y;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
