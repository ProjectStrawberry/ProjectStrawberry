using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    public StatData statData;
    private Dictionary<StatType, float> currentStats = new Dictionary<StatType, float>();

    private void Awake()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        foreach (StatEntry entry in statData.stats)
        {
            currentStats[entry.statType] = entry.baseValue;
        }
    }

    public float GetStat(StatType statType)
    {
        return currentStats.ContainsKey(statType) ? currentStats[statType] : 0;
    }

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
