using UnityEngine;

public class Room : MonoBehaviour
{
    public int type;

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
