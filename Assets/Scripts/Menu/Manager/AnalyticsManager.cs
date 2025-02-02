using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsManager : Singleton<AnalyticsManager>
{
    private bool _initialized = false;
    protected override bool DestroyOnLoad => false;
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        _initialized = true;
    }

    public void StartLevel(string levelName)
    {
        if (!_initialized) return;
        CustomEvent newEvent = new CustomEvent("start_level")
        {
            { "level_name", levelName }
        };
        AnalyticsService.Instance.RecordEvent(newEvent);
        AnalyticsService.Instance.Flush();
    }
    
    public void FailLevel(string levelName, float distance)
    {
        if (!_initialized) return;
        CustomEvent newEvent = new CustomEvent("fail_level")
        {
            { "level_name", levelName },
            {"distance", distance}
        };
        AnalyticsService.Instance.RecordEvent(newEvent);
        AnalyticsService.Instance.Flush();
    }
    
    public void CompletedLevel(string levelName)
    {
        if (!_initialized) return;
        CustomEvent newEvent = new CustomEvent("completed_level")
        {
            { "level_name", levelName },
        };
        AnalyticsService.Instance.RecordEvent(newEvent);
        AnalyticsService.Instance.Flush();
    }
}
