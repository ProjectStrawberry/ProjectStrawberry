using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    public StatData normalStatData;
    public StatData hardStatData;
    private Dictionary<StatType, float> currentStats = new Dictionary<StatType, float>();

    private void Awake()
    {
        InitializeStats(Difficulty.Normal);
        GameManager.Instance.SubscribeOnDifficultyChange(InitializeStats);
    }

    private void InitializeStats(Difficulty difficulty)
    {
        StatData statData;
        if (difficulty == Difficulty.Normal)
            statData = normalStatData;
        else
            statData = hardStatData;
        foreach (StatEntry entry in statData.stats)
        {
            currentStats[entry.statType] = entry.baseValue;
        }
    }

    public float GetStat(StatType statType)
    {
        return currentStats.ContainsKey(statType) ? currentStats[statType] : 0;
    }

    //스탯 영구 상승 혹은 일시 상승 메서드들 혹시 쓰나?
    //public void ModifyStat(StatType statType, float amount, bool isPermanent = true, float duration = 0)
    //{
    //    if (!currentStats.ContainsKey(statType)) return;

    //    currentStats[statType] += amount;

    //    if (!isPermanent)
    //    {
    //        StartCoroutine(RemoveStatAfterDuration(statType, amount, duration));
    //    }
    //}

    //private IEnumerator RemoveStatAfterDuration(StatType statType, float amount, float duration)
    //{
    //    yield return new WaitForSeconds(duration);
    //    currentStats[statType] -= amount;
    //}

}
