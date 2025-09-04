using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//public class FollowBar : MonoBehaviour
//{
//    [SerializeField] Transform targetTransform;

//    Vector3 offset=new Vector3(-1f,-0.5f,0);
//    Camera cam;
//    float characterMaxHealth = 6f;
//    float characterCurrentHealth = 3f;

//    CanvasGroup canvasGroup;

//    List<Image> healthObjects= new List<Image>();
//    void Start()
//    {
//        canvasGroup = GetComponent<CanvasGroup>();

//        cam = Camera.main;
//        foreach(Transform child in transform)
//        {
//            var image= child.GetComponent<Image>();
//            healthObjects.Add(image);
//            image.enabled = false;
//        }
//        canvasGroup.alpha = 0f;

//        UpdateHealthBar(characterCurrentHealth);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Vector3 worldPosition = targetTransform.position + offset;
//        Vector3 screenPosition=cam.WorldToScreenPoint(worldPosition);
//        transform.position = screenPosition;
//    }

//    void TakeDamage()
//    {

//    }

//    void UpdateHealthBar(float currenthealth)
//    {
        
//        for(int i = 0; i < currenthealth; i++)
//        {
//            healthObjects[i].enabled = true;
//        }
//        StartCoroutine(FadeEffect());
//    }


//    IEnumerator FadeEffect()
//    {
//        float lerpDuration = 1f;
//        float time = 0f;
//        while (time<lerpDuration)
//        {
//            time += Time.deltaTime;
//            canvasGroup.alpha=Mathf.Lerp(0,1,time/lerpDuration);
//            yield return null;
           
//        }
//        yield return new WaitForSeconds(3f);
//        time = 0f;
//        lerpDuration = 1f;
//        while(time < lerpDuration)
//        {
//            time += Time.deltaTime;
//            canvasGroup.alpha = Mathf.Lerp(1, 0, time / lerpDuration);
//            yield return null;
//        }
//    }

     
//}
