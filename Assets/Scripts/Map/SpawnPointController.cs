using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SpawnPointController : MonoBehaviour
{
    Vector3 firstSpawnPosition = new Vector3(-2.7f, -5.0f, 0);


    public Vector3 CurrentSpawnPosition {  get; private set; }

    //�����ؾ��� ��Ȳ�� ���� currentspawnposition���� �����Ǹ� ��
    // Start is called before the first frame update
    void Start()
    {
        CurrentSpawnPosition=firstSpawnPosition;
        PlayerManager.Instance.GetSpawnPointController(this);
    }

    // Update is called once per frame

    public void GetSpawnpoint(Vector3 vector3)
    {
        CurrentSpawnPosition = vector3;
    }

}

