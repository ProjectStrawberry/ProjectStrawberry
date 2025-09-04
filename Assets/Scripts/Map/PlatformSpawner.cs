using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] List<PlatformPatternSO> platformPaternList=new List<PlatformPatternSO>();
    [SerializeField] GameObject semiSolidPrefab;
    [SerializeField] GameObject solidPrefab;


    private float spawnInterval = 4f;
    private WaitForSeconds waitingtime;

    private float platformSizeMultiplyRate = 2f;

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
            Debug.Log($"{i+1}번 패턴 시작");
            foreach (var platforminfo in platformPaternList[i].platformDatas)
            {
                
                SpawnPlatform(platforminfo);
                if (platforminfo.platformPlacement == PlatformPlacement.Double)
                {
                    continue;
                }
                yield return waitingtime;
            } 
        }
        while (true)
        {
            PlatformPatternSO platformPattern=ChooseRandomPattern();
            Debug.Log($"{platformPattern.name} 패턴 시작");
            foreach (var platforminfo in platformPattern.platformDatas)
            {

                SpawnPlatform(platforminfo);
                if (platforminfo.platformPlacement == PlatformPlacement.Double)
                {
                    continue;
                }
                yield return waitingtime;
            }
        }

        //기본 패턴 끝나고 랜덤 패턴
        


    }

    private PlatformPatternSO ChooseRandomPattern()
    {
        int randomNumber = Random.Range(0, platformPaternList.Count - 1);
        return platformPaternList [randomNumber];
    }



    private void SpawnPlatform(PlatformData platformData)
    {
        GameObject go;
        if(platformData.platformType==PlatformType.SemiSolid)
        {
           
            if(semiSolidPool.Count > 0)
            {
                go=semiSolidPool.Dequeue();
                //go.transform.position=platformData.platformPosition;
                go.transform.SetParent(this.transform);
                go.transform.localPosition=platformData.platformPosition;
                go.SetActive(true);

            }
            else
            {
                go = Instantiate(semiSolidPrefab);
                go.transform.SetParent(this.transform);
                go.transform.localPosition = platformData.platformPosition;
                
            }
            
        }
        else
        {
            if(solidPool.Count > 0)
            {
                go=solidPool.Dequeue();
                //go.transform.position = platformData.platformPosition;
                go.transform.SetParent(this.transform);
                go.transform.localPosition = platformData.platformPosition;
                go.SetActive(true);
            }
            else
            {
                go = Instantiate(solidPrefab);
                go.transform.SetParent(this.transform);
                go.transform.localPosition = platformData.platformPosition;
            }
               
        }
        var spriteRenderer = go.GetComponent<SpriteRenderer>();
        spriteRenderer.size = new Vector2(platformData.platformWidth*platformSizeMultiplyRate, platformHeight);
        Vector3 goPosition = go.transform.position;

        //밀어줘야하는 양 계산
        float pushPivotX=spriteRenderer.size.x*0.5f;
        float pushPivotY = spriteRenderer.size.y*0.5f;

        goPosition.x = go.transform.position.x*platformSizeMultiplyRate + pushPivotX;

        go.transform.position=goPosition; //추가
        var collider = go.GetComponent<BoxCollider2D>();
        collider.size = spriteRenderer.size;
    }

  


    public void ReturnToSolidPool(GameObject gameObject)
    {
        solidPool.Enqueue(gameObject);
        gameObject.SetActive(false);
    }

    public void ReturnToSemiSolidPool(GameObject gameObject)
    {
        semiSolidPool.Enqueue(gameObject);
        gameObject.SetActive(false);
    }

}
