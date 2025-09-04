using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeforeThunderLaser : MonoBehaviour
{
    private Animator Animator;
    public string animationName;
    public GameObject thunderLaser;
    private int damage;
    
    public void Init(float time, GameObject thunderLaserGo, int damage)
    {
        Animator = GetComponent<Animator>();
        this.thunderLaser = thunderLaserGo;
        this.damage = damage;
        
        AnimationClip clip = null;
        foreach (var anim in Animator.runtimeAnimatorController.animationClips)
        {
            if (anim.name == animationName)
            {
                clip = anim;
                break;
            }
        }

        float speed = clip.length / time;
        Animator.Play(animationName, -1, 0f);

        StartCoroutine(SpawnThunderLaser(time));
    }

    private IEnumerator SpawnThunderLaser(float time)
    {
        yield return new WaitForSeconds(time);

        var thunder = Instantiate(thunderLaser, this.transform.position, Quaternion.identity);
        thunder.GetComponent<ThunderLaser>().Init(damage);
        
        Destroy(this.gameObject);
    }
}
