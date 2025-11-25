#nullable enable
using UnityEngine;

[CreateAssetMenu(fileName = "NewGoal", menuName = "Game/Goals/Goal")]
public class TaskSO : ScriptableObject
{
    public string Name = null!;
    [TextArea] public string Description = null!;
    public string StartMessage;
    public string SuccessMessage;
    public string FailureMessage;

    public Vector3 Location;
    public GameObject? MinimapIcon = null;
}