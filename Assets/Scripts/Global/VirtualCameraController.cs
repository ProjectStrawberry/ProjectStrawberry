using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour
{
    CinemachineVirtualCamera vcam;

    public int currentPriority = 5;
    public int activeProiority = 20;

    private void Awake()
    {
        vcam = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            vcam.Priority = activeProiority;
            vcam.Follow = collision.gameObject.transform;
            PlayerManager.Instance.player.playerController.vcam = vcam;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            vcam.Priority = currentPriority;
            vcam.Follow = null;
        }
    }

    
}
