using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] List<PlatformPatternSO> platformPaternList=new List<PlatformPatternSO>();
    [SerializeField] GameObject semiSolidPrefab;
    [SerializeField] GameObject solidPrefab;


    private float spawnInterval = 2f;
    private WaitForSeconds waitingtime;

    Queue<GameObject> semiSolidPool = new Queue<GameObject>();
    Queue<GameObject> solidPool= new Queue<GameObject>();


    private float platformHeight = 1f;
    private void Start()
    {
        waitingtime = new WaitForSeconds(spawnInterval);
        StartCoroutine(SetSpawnData());

    }

    IEnumerator SetSpawnData()
    {
        for(int i = 0; i < platformPaternList.Count; i++)
        {
            foreach(var platforminfo in platformPaternList[i].platformDatas)
            {
                yield return waitingtime;
                SpawnPlatform(platforminfo);
            }
        }
    }

    private void SpawnPlatform(PlatformData platformData)
    {
        GameObject go;
        if(platformData.platformType==PlatformType.SemiSolid)
        {
            Debug.Log("세미솔리드 발판");
            if(semiSolidPool.Count > 0)
            {
                go=semiSolidPool.Dequeue();
                go.transform.position=platformData.platformPosition;
                go.SetActive(true);
                Debug.Log($"{go.name}세미솔리드 발판 풀에서 생성");
            }
            else
            {
                go = Instantiate(semiSolidPrefab, platformData.platformPosition, Quaternion.identity);
                
            }
            
        }
        else
        {
            if(solidPool.Count > 0)
            {
                go=solidPool.Dequeue();
                go.transform.position = platformData.platformPosition;
                go.SetActive(true);
            }
            else
            {
                go = Instantiate(solidPrefab, platformData.platformPosition, Quaternion.identity);
            }
               
        }
        var spriteRenderer = go.GetComponent<SpriteRenderer>();
        spriteRenderer.size = new Vector2(platformData.platformWidth, platformHeight);
        var collider = go.GetComponent<BoxCollider2D>();
        collider.size = spriteRenderer.size;
    }

  


    public void ReturnToSolidPool(GameObject gameObject)
    {
        solidPool.Enqueue(gameObject);
        Debug.Log("솔리드 풀로 돌아감");
        gameObject.SetActive(false);
    }

    public void ReturnToSemiSolidPool(GameObject gameObject)
    {
        semiSolidPool.Enqueue(gameObject);
        gameObject.SetActive(false);
    }

}
