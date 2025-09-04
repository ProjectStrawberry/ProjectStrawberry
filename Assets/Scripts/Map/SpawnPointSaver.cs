using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointSaver : MonoBehaviour
{
    SpawnPointController controller;
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] Transform spawnPosition;
    private void Start()
    {
        controller = GetComponentInParent<SpawnPointController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            controller.GetSpawnpoint(spawnPoint);
            Debug.Log("스폰포인트 설정");
        }
    }


}
