using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Normal,
    Hard
}

public class GameManager : MonoSingleton<GameManager>
{
    public Difficulty currentDifficulty=Difficulty.Normal;

    private Action<Difficulty> OnDifficultyChange;
    public void StartGame()
    {
        
        UIManager.Instance.OpenUI<UIGame>();
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


    public void ChangeDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty;
        OnDifficultyChange?.Invoke(currentDifficulty);
    }

    public void SubscribeOnDifficultyChange(Action<Difficulty> action)
    {
        OnDifficultyChange += action;
    }
}
