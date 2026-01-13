#nullable enable
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private RectTransform MinimapUIObject;
    [SerializeField] private MissionSO InitialMission;
    [SerializeField] private MissionOverlayController MissionOverlay;

    // we assume that the minimap camera is a child of the minimap object
    [SerializeField] private Camera MinimapCamera;

    Mission? CurrentMission;
    RunOnce? _runonce;
    RunOnce _init;

    private bool _initializationFailed;

    public void Awake()
    {
        if (MinimapCamera == null)
        {
            Debug.LogError("MissionManager: Minimap camera not assigned. Disabling.");
            _initializationFailed = true;
            enabled = false;
            return;
        }

        if (InitialMission == null)
        {
            Debug.LogError("MissionManager: InitialMission not assigned. Disabling.");
            _initializationFailed = true;
            enabled = false;
            return;
        }

        _init = new RunOnce()
        {
            PreCalls = 1,
            Func = () => Initialization()
        };
    }

    // This function will be called in `Update()`
    void Initialization()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("MissionManager: Player not found. Disabling.");
            _initializationFailed = true;
            enabled = false;
            return;
        }

        var entity = player.GetComponent<CharacterEntity>();
        if (entity == null)
        {
            Debug.LogError("MissionManager: Player has no CharacterEntity. Disabling.");
            _initializationFailed = true;
            enabled = false;
            return;
        }

        MissionConfig config = new()
        {
            Host = entity,
            MinimapCamera = MinimapCamera,
            MinimapParent = MinimapUIObject
        };

        CurrentMission =
            MissionFactory.Instance.CreateMissionFromSO(InitialMission, config);

        if (CurrentMission == null)
        {
            Debug.LogError("MissionManager: Failed to create mission. Disabling.");
            _initializationFailed = true;
            enabled = false;
            return;
        }

        CurrentMission.OnMissionCompleted += MissionCompleteHandler;
        CurrentMission.OnTaskStarted += TaskStartedHandler;
        CurrentMission.OnTaskCompleted += TaskCompletedHandler;

        if (MissionOverlay != null)
        {
            MissionOverlay.AddMission(CurrentMission);
        }

        CurrentMission.StartMission();
    }

    void Update()
    {
        // Skip if initialization failed or _init wasn't created
        if (_initializationFailed || _init == null) return;

        _init.Run();

        if (CurrentMission == null) return;

        CurrentMission.ManualUpdate();
    }

    void MissionCompleteHandler()
    {
        if (CurrentMission == null)
        {
            Debug.LogWarning("MissionManager: CurrentMission is null in MissionCompleteHandler.");
            return;
        }

        // Unsubscribe before nulling
        CurrentMission.OnMissionCompleted -= MissionCompleteHandler;
        CurrentMission.OnTaskStarted -= TaskStartedHandler;
        CurrentMission.OnTaskCompleted -= TaskCompletedHandler;
        CurrentMission = null;
    }

    void TaskStartedHandler(Task task)
    {
        if (MissionOverlay != null)
        {
            MissionOverlay.AddTask(task);
        }
    }

    void TaskCompletedHandler(Task task)
    {
        if (MissionOverlay != null)
        {
            MissionOverlay.CompleteTask(task);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from events to prevent issues during scene transitions
        if (CurrentMission != null)
        {
            CurrentMission.OnMissionCompleted -= MissionCompleteHandler;
            CurrentMission.OnTaskStarted -= TaskStartedHandler;
            CurrentMission.OnTaskCompleted -= TaskCompletedHandler;
        }
    }
}
