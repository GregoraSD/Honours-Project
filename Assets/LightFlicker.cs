using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [Header("Intensity Parameters")]
    public float averageIntensity = 1.0f;
    public Vector2 intensityDistance = Vector2.one;
    public Vector2 intensitySpeed = Vector2.one;

    [Header("Range Parameters")]
    public float averageRange = 1.0f;
    public Vector2 rangeDistance = Vector2.one;
    public Vector2 rangeSpeed = Vector2.one;

    private Light l;

    private void Start()
    {
        l = GetComponent<Light>();

        StartCoroutine(RangeFlicker());
        StartCoroutine(IntensityFlicker());
    }

    private IEnumerator IntensityFlicker()
    {
        float t = 0.0f;
        float speed = Random.Range(intensitySpeed.x, intensitySpeed.y);
        float distance = Random.Range(intensityDistance.x, intensityDistance.y);

        while (t < 2 * Mathf.PI)
        {
            l.intensity = averageIntensity + Mathf.Sin(t) * distance;
            t += Time.deltaTime * speed;
            yield return null;
        }

        StartCoroutine(IntensityFlicker());
    }

    private IEnumerator RangeFlicker()
    {
        float t = 0.0f;
        float speed = Random.Range(rangeSpeed.x, rangeSpeed.y);
        float distance = Random.Range(rangeDistance.x, rangeDistance.y);

        while (t < 2 * Mathf.PI)
        {
            l.range = averageRange + Mathf.Sin(t) * distance;
            t += Time.deltaTime * speed;
            yield return null;
        }

        StartCoroutine(RangeFlicker());
    }
}