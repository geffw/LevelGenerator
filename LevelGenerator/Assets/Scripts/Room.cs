using UnityEngine;

public class Room : MonoBehaviour
{
    public int type;

    public int difficulty;

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
