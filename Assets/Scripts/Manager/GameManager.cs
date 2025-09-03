using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    

    public void StartGame()
    {

    }


    public void StopGameTemporarily()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        UIManager.Instance.OpenUI<UIGameOver>();
    }

    public void ReStart()
    {
        Player player=PlayerManager.Instance.player;
        player.transform.position = PlayerManager.Instance.spawnPointController.CurrentSpawnPosition;
        UIManager.Instance.CloseUI<UIGameOver>();

    }

}
