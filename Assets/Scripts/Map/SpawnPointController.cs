using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class SpawnPointController : MonoBehaviour
{

    [SerializeField] StageSO stageInfo;
    List<SpawnPointSaver> spawnPointSavers = new List<SpawnPointSaver>();
    [SerializeField] GameObject skeletonPrefab;
    [SerializeField] GameObject floatingSKullPrefab;
    [SerializeField] GameObject crystalKnight;

    Vector3 firstSpawnPosition = new Vector3(-2.7f, -5.0f, 0);

    PlatformSpawner platformSpawner;
    public Vector3 CurrentSpawnPosition {  get; private set; }

    //스폰해야할 상황이 오면 currentspawnposition으로 스폰되면 됨
    // Start is called before the first frame update
    void Start()
    {
        CurrentSpawnPosition=firstSpawnPosition;
        PlayerManager.Instance.GetSpawnPointController(this);
        spawnPointSavers=GetComponentsInChildren<SpawnPointSaver>().ToList();
        platformSpawner=GetComponentInChildren<PlatformSpawner>();
    }

    // Update is called once per frame

    public void GetSpawnpoint(Vector3 vector3)
    {
        CurrentSpawnPosition = vector3;
    }

    public void SpawnMonsters(int spawnpointNum)
    {
        if(spawnpointNum ==4)
        {
            platformSpawner.StartSpawning();
        }
        StageLevel stageLevel = stageInfo.stages[spawnpointNum-1];


        foreach( var monster in stageLevel.monsters )
        {
            if(monster.type==MonsterType.Skeleton)
            {
                for(int i=0; i<monster.spawnPoints.Length; i++)
                {
                    Instantiate(skeletonPrefab,monster.spawnPoints[i],Quaternion.identity);
                }
                
            }
            else if(monster.type == MonsterType.FloatingSkull)
            {
                for (int i = 0; i < monster.spawnPoints.Length; i++)
                {
                    Instantiate(floatingSKullPrefab, monster.spawnPoints[i], Quaternion.identity);
                }
            }
            else
            {
                for (int i = 0; i < monster.spawnPoints.Length; i++)
                {
                    Instantiate(crystalKnight, monster.spawnPoints[i], Quaternion.identity);
                }
            }
        }
    }

    public void ResetSpawnPointSavers()
    {
        foreach (var savepoint in spawnPointSavers)
        {
            savepoint.ResetSpawnerCondition();
        }
    }

}

