using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointSaver : MonoBehaviour
{
    SpawnPointController controller;
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] Transform spawnPosition;
    [SerializeField] public int savePointNum;

    public bool IsAlreadyActivated { get; set; } = false;
    private void Start()
    {
        controller = GetComponentInParent<SpawnPointController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            controller.GetSpawnpoint(spawnPoint,this);
           
            if (IsAlreadyActivated == false)
            {
                controller.SpawnMonsters(savePointNum);
                controller.ResetSpawnPointSavers();
                controller.ReturnMonstersToPool();
            }
            IsAlreadyActivated = true;

            if (savePointNum == 4)
            {
                controller.SpawnClearTile();
            }
        }
    }

    public void ResetSpawnerCondition()
    {
        IsAlreadyActivated=false;
    }


}
