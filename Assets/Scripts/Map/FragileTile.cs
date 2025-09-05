using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileTile : MonoBehaviour
{
    private Coroutine _fragileCoroutine;
    [SerializeField] private float fragileTimer = 6f;
    [SerializeField] private List<GameObject> squareList = new List<GameObject>();

    private void Awake()
    {
        if (_fragileCoroutine != null)
        {
            StopCoroutine(_fragileCoroutine);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_fragileCoroutine == null)
        {
            _fragileCoroutine = StartCoroutine(StartFragile());
        }
    }

    private IEnumerator StartFragile()
    {
        float curTime = 0f;
        Color origin = squareList[0].GetComponent<SpriteRenderer>().color;
        
        while (curTime < fragileTimer)
        {
            curTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, curTime / fragileTimer);
            
            foreach (var square in squareList)
            {
                square.GetComponent<SpriteRenderer>().color = new Color(origin.r, origin.g, origin.b, alpha);
            }

            yield return null;
        }
        
        yield return null;

        Destroy(this.gameObject);
    }
}
