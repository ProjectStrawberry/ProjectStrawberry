using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class SpawnPointController : MonoBehaviour
{

    [SerializeField] StageSO stageInfo;
    List<SpawnPointSaver> spawnPointSavers = new List<SpawnPointSaver>();

    Queue<GameObject> skeletonPrefabList = new Queue<GameObject>();
    Queue<GameObject> floatingPrefabList = new Queue<GameObject>();
    GameObject crystal;
   


    [SerializeField] GameObject skeletonPrefab;
    [SerializeField] GameObject floatingSKullPrefab;
    [SerializeField] GameObject crystalKnightPrefab;
    [SerializeField] private GameObject clearTile;

    Vector3 firstSpawnPosition = new Vector3(-2.7f, -5.0f, 0);

    PlatformSpawner platformSpawner;
    public Vector3 CurrentSpawnPosition {  get; private set; }

    SpawnPointSaver lastActivatedPoint;

    //�����ؾ��� ��Ȳ�� ���� currentspawnposition���� �����Ǹ� ��
    // Start is called before the first frame update
    void Start()
    {
        CurrentSpawnPosition=firstSpawnPosition;
        PlayerManager.Instance.GetSpawnPointController(this);
        spawnPointSavers=GetComponentsInChildren<SpawnPointSaver>().ToList();
        platformSpawner=GetComponentInChildren<PlatformSpawner>();
    }

    // Update is called once per frame

    public void GetSpawnpoint(Vector3 vector3,SpawnPointSaver spawnPointSaver)
    {
        CurrentSpawnPosition = vector3;
        lastActivatedPoint = spawnPointSaver;
    }

    public void SpawnMonsters(int spawnpointNum)
    {
        if(spawnpointNum == 4)
        {
            platformSpawner.StartSpawning();
            StartCoroutine(ClearTileCoroutine());
        }
        StageLevel stageLevel = stageInfo.stages[spawnpointNum-1];


        foreach( var monster in stageLevel.monsters )
        {
            if(monster.type == MonsterType.Skeleton)
            {
                for(int i=0; i<monster.spawnPoints.Length; i++)
                {
                    if(skeletonPrefabList.Count>0)
                    {
                        GameObject go=skeletonPrefabList.Dequeue();
                        go.SetActive(true);
                        go.transform.position=monster.spawnPoints[i];
                       
                        
                    }
                    else
                    {
                        GameObject go= Instantiate(skeletonPrefab, monster.spawnPoints[i], Quaternion.identity);
                        go.SetActive(true) ;
                        
                    }
                        
                }
                
            }
            else if(monster.type == MonsterType.FloatingSkull)
            {

                for (int i = 0; i < monster.spawnPoints.Length; i++)
                {
                    if (floatingPrefabList.Count > 0)
                    {
                        GameObject go = floatingPrefabList.Dequeue();
                        go.SetActive(true);
                        go.transform.position = monster.spawnPoints[i];
                        
                        
                    }
                    else
                    {
                        GameObject go= Instantiate(floatingSKullPrefab, monster.spawnPoints[i], Quaternion.identity);
                        go.SetActive(true);
                        
                    }
                        
                }
            }
            else
            {
                for (int i = 0; i < monster.spawnPoints.Length; i++)
                {
                    if(crystal!=null)
                    {
                        crystal.SetActive(true);
                        crystal.transform.position = monster.spawnPoints[i];

                    }
                    else
                    {
                        GameObject go = Instantiate(crystalKnightPrefab, monster.spawnPoints[i], Quaternion.identity);
                        go.SetActive(true);
                    }
                        
                        
                    
                       
                }
            }
        }
    }


    public void ReturnMonstersToPool()
    {
        Skeleton[] currentactiveSkelletons = FindObjectsOfType<Skeleton>();
        FloatingSkull[] currentfloatingSkulls = FindObjectsOfType<FloatingSkull>();
        foreach (Skeleton skeleton in currentactiveSkelletons)
        {
            skeleton.gameObject.SetActive(false);
            skeletonPrefabList.Enqueue(skeleton.gameObject);
            
            
        }
        foreach(FloatingSkull floatingSkull in currentfloatingSkulls)
        {
            floatingSkull.gameObject.SetActive(false);
            floatingPrefabList.Enqueue(floatingSkull.gameObject);
        }
       
        if(lastActivatedPoint!=null)
        {
            RespawnMonsters();
        }
        
    }

    void RespawnMonsters()
    {
        SpawnMonsters(lastActivatedPoint.savePointNum);
        lastActivatedPoint.IsAlreadyActivated = true;
    }
    public void ResetSpawnPointSavers()
    {
        foreach (var savepoint in spawnPointSavers)
        {
            savepoint.ResetSpawnerCondition();
        }
    }

    private IEnumerator ClearTileCoroutine()
    {
        yield return new WaitForSeconds(0.7f);

        SoundManager.Instance.ChangeBackGroundMusice(SoundManager.Instance.bossBgm);
        Instantiate(clearTile, new Vector3(17.8f, -30.9f, 0f), Quaternion.identity);
    }
}

