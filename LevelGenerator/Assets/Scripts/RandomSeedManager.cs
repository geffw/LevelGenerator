using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class RandomSeedManager : MonoBehaviour
{
    [SerializeField] private bool useSeed;
    [SerializeField] private int seed;
    
    private void Awake()
    {
        if (!useSeed) return;
        
        Random.InitState(seed);

        Debug.Log($"Random seed value set ({seed}). Test random range: {Random.Range(0, 1000000)}");
    }
}
