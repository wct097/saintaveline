# NPC Death State Memory Leak Analysis

## Issue Summary

Dead NPCs are not being destroyed due to a bug in `NPCDeathState.FadeOutAndDestroy()`, causing unbounded native memory growth that eventually crashes Unity (~33GB).

## Evidence

![Memory leak evidence showing ~33GB RAM usage](./images/npc-death-memory-leak-evidence.png)

**Screenshot shows:**
- Unity Editor consuming ~33GB RAM (Task Manager)
- Dead NPCs visible in scene (not fading/disappearing)
- Diagnostic console output showing memory stats

## Memory Analysis

| Metric | Value | Source |
|--------|-------|--------|
| **Task Manager (actual)** | ~33,000 MB | Windows |
| **Managed Memory** | ~1,400-1,500 MB | Unity Profiler |
| **Reserved Memory** | ~2,000 MB | Unity Profiler |
| **Mono Heap** | ~800 MB | Unity Profiler |

### The ~30GB Gap

The massive discrepancy between Task Manager (~33GB) and Unity Profiler (~2GB) represents **native memory** not visible to C# profiling APIs:

- **GPU resources** - SkinnedMeshRenderer buffers, textures
- **NavMeshAgent** - Navigation mesh data per NPC
- **Animator** - Animation state machine data
- **AudioSource** - Audio buffers
- **Physics** - Rigidbody and Collider data
- **D3D12 command buffers** - Graphics API allocations
- **JobTempAlloc** - Unity's job system temporary allocations

## Root Cause

In `Assets/_Game/Scripts/CommonNPC/NPCDeathState.cs`:

```csharp
private IEnumerator FadeOutAndDestroy()
{
    // BUG: Animated NPCs use SkinnedMeshRenderer, not MeshRenderer
    var renderer = this.NPC!.GetComponent<MeshRenderer>();  // Returns NULL
    var material = renderer.material;  // Throws MissingComponentException
    // ...
    UnityEngine.Object.Destroy(NPC.gameObject);  // NEVER REACHED
}
```

1. `GetComponent<MeshRenderer>()` returns null (NPCs use `SkinnedMeshRenderer`)
2. Accessing `renderer.material` throws `MissingComponentException`
3. Coroutine crashes before reaching `Destroy()`
4. NPC and all attached components remain in scene forever
5. Native resources accumulate with each NPC death

## Impact Assessment

### Debug vs Release Builds

| Factor | Debug Build | Release Build |
|--------|-------------|---------------|
| **Native memory leak** | YES | YES |
| **Memory growth rate** | ~70 MB/sec | ~70 MB/sec (same) |
| **Time to crash** | ~8-10 minutes | ~8-10 minutes |
| **Editor overhead** | +2-3 GB | N/A |
| **GC behavior** | Irrelevant | Irrelevant |

**Critical finding:** This is NOT a "debug mode only" issue. Native memory leaks identically in both builds because:

1. Managed memory (C# heap) is stable - GC handles it fine
2. The leak is in **native Unity resources** that are only freed by `Destroy()`
3. Debug/release GC differences do not affect native memory

### Platform Impact

| Platform | RAM Available | Time to Crash |
|----------|---------------|---------------|
| Desktop (32GB) | 32 GB | ~8 minutes |
| Desktop (16GB) | 16 GB | ~4 minutes |
| Console | 8-16 GB | ~2-4 minutes |
| Mobile | 2-4 GB | < 1 minute |

## The Fix

Replace `GetComponent<MeshRenderer>()` with `GetComponentInChildren<Renderer>()`:

```csharp
private IEnumerator FadeOutAndDestroy()
{
    var renderer = this.NPC!.GetComponentInChildren<Renderer>();
    if (renderer == null)
    {
        yield return new WaitForSeconds(_fadeDuration);
        UnityEngine.Object.Destroy(NPC.gameObject);
        yield break;
    }
    // ... fade logic ...
    UnityEngine.Object.Destroy(NPC.gameObject);
}
```

This ensures:
1. Works with any renderer type (SkinnedMeshRenderer, MeshRenderer)
2. Graceful fallback if no renderer found
3. `Destroy()` is always called, releasing native resources

## Related Issues

- Upstream PR #186: Original fix proposal
- Upstream PR #188: Player shooting fix (separate issue)

## Reproduction Steps

1. Start game on `demo/exception-spam` branch
2. Kill any NPC
3. Wait 10+ seconds (fade should start)
4. Observe console: `[BUG DEMO] ... MeshRenderer is NULL!`
5. Monitor Task Manager - memory grows ~70MB/sec
6. After ~8-10 minutes, Unity crashes at ~33GB

---

*Analysis conducted 2026-01-14*
