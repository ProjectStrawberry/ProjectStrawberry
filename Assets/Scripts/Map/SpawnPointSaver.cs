using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointSaver : MonoBehaviour
{
    SpawnPointController controller;
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] Transform spawnPosition;
    [SerializeField] int savePointNum;

    public bool IsAlreadyActivated { get; private set; }=false;
    private void Start()
    {
        controller = GetComponentInParent<SpawnPointController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            controller.GetSpawnpoint(spawnPoint);
            Debug.Log("��������Ʈ ����");
            if(IsAlreadyActivated==false)
            {
                Debug.Log("���� ��ȯ ����");
                controller.SpawnMonsters(savePointNum);
                
            }
            IsAlreadyActivated = true;

        }
    }

    public void ResetSpawnerCondition()
    {
        IsAlreadyActivated=false;
    }


}
