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
        if(platformData.platformType==PlatformType.SemiSolid)
        {
            GameObject go= Instantiate(semiSolidPrefab,platformData.platformPosition, Quaternion.identity);
            var spriteRenderer = go.GetComponent<SpriteRenderer>();
            spriteRenderer.size = new Vector2(platformData.platformWidth, platformHeight);
            var collider = go.GetComponent<BoxCollider2D>();
            collider.size = spriteRenderer.size;
        }
        else
        {
            GameObject go=Instantiate(solidPrefab,platformData.platformPosition,Quaternion.identity);
            var spriteRenderer = go.GetComponent<SpriteRenderer>();
            spriteRenderer.size = new Vector2(platformData.platformWidth, platformHeight);
            var collider = go.GetComponent<BoxCollider2D>();
            collider.size = spriteRenderer.size;
        }


    }

}
