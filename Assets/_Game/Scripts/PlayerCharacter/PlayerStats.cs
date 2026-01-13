using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : CharacterEntity
{
    private Dictionary<string, Vector3> _labeledPoints = new Dictionary<string, Vector3>();
    public Dictionary<string, Vector3> LabeledPoints { get => _labeledPoints; set => _labeledPoints = value; }

    // Camera shake settings
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 0.1f;
    private Camera _mainCamera;
    private Coroutine _shakeCoroutine;

    public override void Awake()
    {
        base.Awake();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    public override float TakeDamage(float amount)
    {
        float result = base.TakeDamage(amount);

        // Trigger camera shake on damage
        if (_mainCamera != null && amount > 0)
        {
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
            }
            _shakeCoroutine = StartCoroutine(CameraShake());
        }

        return result;
    }

    private IEnumerator CameraShake()
    {
        Vector3 originalLocalPos = _mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;

            _mainCamera.transform.localPosition = originalLocalPos + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _mainCamera.transform.localPosition = originalLocalPos;
        _shakeCoroutine = null;
    }
}
