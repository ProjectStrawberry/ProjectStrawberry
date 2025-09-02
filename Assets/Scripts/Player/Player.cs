using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerCondition playerCondition;
    public PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerCondition = GetComponent<PlayerCondition>();

        PlayerManager.Instance.player = this;
    }
    void Start()
    {
        
    }
}
