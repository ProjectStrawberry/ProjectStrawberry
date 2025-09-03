using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public Player player;
    public SpawnPointController spawnPointController;
    public void GetSpawnPointController(SpawnPointController spawnPointController)
    {
        this.spawnPointController=spawnPointController;
    }
}
