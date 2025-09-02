using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
}
