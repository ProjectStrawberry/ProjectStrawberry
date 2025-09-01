using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BackgroundLooper : MonoBehaviour
{
    public GameObject[] tilemaps=new GameObject[2];
    private List<GameObject> backgrounds=new List<GameObject>();
    private float movingSPeed = 1f;
    private float height;

    void Start()
    {
        

        var background_1 = Instantiate(tilemaps[0]);
        var background_2= Instantiate(tilemaps[1]);
        backgrounds.Add(background_1);
        backgrounds.Add(background_2);
        height = background_1.GetComponentInChildren<TilemapRenderer>().bounds.size.y;
        background_1.transform.position = Vector3.zero;
        background_2.transform.position = background_1.transform.position + Vector3.up * height;
    }

    
    private void Move()
    {
        foreach(GameObject background in backgrounds)
        {
            background.transform.Translate(Vector2.down * movingSPeed * Time.deltaTime);
            if (background.transform.position.y < -height)
            {
                background.transform.position += Vector3.up * height * 2;
            }
        }
        


        
    }
    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
