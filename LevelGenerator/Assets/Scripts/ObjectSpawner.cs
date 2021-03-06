﻿using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;

    private void Start()
    {
        int rand = Random.Range(0, objects.Length);

        GameObject obj = Instantiate(objects[rand], transform.position, Quaternion.identity);
        obj.transform.parent = transform;
    }
}
