using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpItem : MonoBehaviour
{
    //�̸� �ϵ��ڵ��� ���� �ɽ��� ����� �帳�ϴ�.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            PlayerManager.Instance.player.playerController.canDoubleJump = true;
            Destroy(gameObject);
        }
    }
}
