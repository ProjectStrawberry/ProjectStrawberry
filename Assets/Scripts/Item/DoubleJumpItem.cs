using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpItem : MonoBehaviour
{
    //미리 하드코딩인 점에 심심한 사과를 드립니다.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            PlayerManager.Instance.player.playerController.canDoubleJump = true;
            Destroy(gameObject);
        }
    }
}
