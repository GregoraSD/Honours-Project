using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportUIPulse : MonoBehaviour
{
    [SerializeField]
    private TeleportAbility teleportAbility;

    [SerializeField]
    private float pulseSpeed = 1;

    [SerializeField]
    private AnimationCurve pulseCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(0.5f, 1.0f), new Keyframe(1.0f, 0.0f));

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        teleportAbility.OnTeleportWithCooldown += DoPulse;
    }

    private void OnDisable()
    {
        teleportAbility.OnTeleportWithCooldown -= DoPulse;
    }

    private void DoPulse()
    {
        StartCoroutine(Pulse());
    }

    private IEnumerator Pulse()
    {
        float t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime * pulseSpeed;
            float c = pulseCurve.Evaluate(t);
            transform.localScale = originalScale + new Vector3(c, c, c);
            yield return null;
        }

        transform.localScale = originalScale;
    }
}