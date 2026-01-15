#nullable enable

using UnityEngine;
using System.Collections;
using UnityEngine.Profiling;

public class NPCDeathState : NPCState
{
    // DIAGNOSTIC: Track memory and allocations across all dead NPCs
    private static long _lastMemoryCheck = 0;
    private static long _baselineMemory = 0;
    private static int _baselineGCCount = 0;

    public NPCDeathState(BaseNPC baseNpc) : base(baseNpc)
    {
        // nothing to do
    }

    public override void Enter()
    {
        Debug.Log($"{this.NPC!.name} has died.");
        Rigidbody rb = this.NPC!.GetComponent<Rigidbody>();
        if  (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None; 
            rb.linearDamping = 2f;  
            rb.angularDamping = 1f;  
            rb.AddTorque(Vector3.right * 5f, ForceMode.Impulse);
        }

        UnityEngine.AI.NavMeshAgent navAgent = this.NPC!.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navAgent != null)
        {
            navAgent.enabled = false;
        }
    }

    public override void Exit()
    {
        // Nothing to do in the death state
    }

    private float _delayBeforeFade = 10f;   // AI: Seconds to wait before starting fade
    private float _stateTimer = 0f;         // AI: Tracks time in this state
    private bool _fadeStarted = false;      // AI: Ensures fade starts once
    private float _fadeDuration = 2f;       // AI: Fade duration

    public override NPCStateReturnValue? Update()
    {
        _stateTimer += Time.deltaTime;                                 // AI: Accumulate time

        if (!_fadeStarted && _stateTimer >= _delayBeforeFade)
        {
            _fadeStarted = true;                                      // AI: Prevent multiple starts
            this.NPC!.StartCoroutine(FadeOutAndDestroy());
        }

        // DIAGNOSTIC: Log every 5 seconds after fade should have completed to show
        // that dead NPCs remain in scene when the fade coroutine crashes
        if (_fadeStarted && _stateTimer > _delayBeforeFade + _fadeDuration + 5f)
        {
            if (Mathf.FloorToInt(_stateTimer) % 5 == 0 && Time.frameCount % 60 == 0)
            {
                long currentMemory = Profiler.GetTotalAllocatedMemoryLong();
                if (_baselineMemory == 0)
                {
                    _baselineMemory = currentMemory;
                    _baselineGCCount = System.GC.CollectionCount(0);
                }
                long delta = currentMemory - _lastMemoryCheck;
                long totalGrowth = currentMemory - _baselineMemory;
                _lastMemoryCheck = currentMemory;

                // Count objects to show accumulation
                int npcCount = Object.FindObjectsOfType<BaseNPC>().Length;
                int gcCollections = System.GC.CollectionCount(0) - _baselineGCCount;
                long reservedMemory = Profiler.GetTotalReservedMemoryLong();
                long monoHeap = Profiler.GetMonoHeapSizeLong();

                Debug.LogWarning($"[BUG DEMO] {NPC?.name ?? "Unknown"} STILL in scene! " +
                    $"Time: {_stateTimer:F0}s | NPCs: {npcCount} | GC: +{gcCollections} | " +
                    $"Managed: {currentMemory / 1024 / 1024}MB | " +
                    $"Reserved: {reservedMemory / 1024 / 1024}MB | " +
                    $"Mono: {monoHeap / 1024 / 1024}MB");
            }
        }

        return null;
    }

    private IEnumerator FadeOutAndDestroy()
    {
        // DIAGNOSTIC: This demonstrates the bug - animated NPCs use SkinnedMeshRenderer,
        // not MeshRenderer. When GetComponent returns null, the next line throws
        // MissingComponentException. The coroutine crashes, NPC never gets destroyed,
        // and accumulates in the scene consuming resources.
        var renderer = this.NPC!.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            Debug.LogError($"[BUG DEMO] {NPC.name}: MeshRenderer is NULL! NPC will never be destroyed. " +
                "This is the bug - animated NPCs use SkinnedMeshRenderer, not MeshRenderer.");
        }
        var material = renderer.material;  // This line throws MissingComponentException
        var originalColor = material.color;

        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime; // AI: Accumulate fade time
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsed / _fadeDuration);
            var c = originalColor;
            c.a = alpha;
            material.color = c;
            yield return null;
        }

        UnityEngine.Object.Destroy(NPC.gameObject);
    }
}
