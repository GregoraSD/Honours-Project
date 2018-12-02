using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [Header("Intensity Parameters")]
    public float averageIntensity = 1.0f;
    public Vector2 intensityDistance = Vector2.one;
    public Vector2 intensitySpeed = Vector2.one;
    public Vector2 intensityPause = Vector2.one;
    public Vector2Int intensitySwap = Vector2Int.one;

    [Header("Range Parameters")]
    public float averageRange = 1.0f;
    public Vector2 rangeDistance = Vector2.one;
    public Vector2 rangeSpeed = Vector2.one;
    public Vector2 rangePause = Vector2.one;
    public Vector2Int rangeSwap = Vector2Int.one;

    private Light l;
    private int intensitySwapCount = 0;
    private int rangeSwapCount = 0;

    private float currentIntensitySpeed, currentIntensityDistance;
    private int currentIntensitySwap;

    private float currentRangeSpeed, currentRangeDistance;
    private int currentRangeSwap;

    private void Start()
    {
        l = GetComponent<Light>();

        Init();
        StartCoroutine(RangeFlicker());
        StartCoroutine(IntensityFlicker());
    }

    private void Init()
    {
        currentIntensitySpeed = Random.Range(intensitySpeed.x, intensitySpeed.y);
        currentIntensityDistance = Random.Range(intensityDistance.x, intensityDistance.y);
        currentIntensitySwap = Random.Range(intensitySwap.x, intensitySwap.y);

        currentRangeSpeed = Random.Range(rangeSpeed.x, rangeSpeed.y);
        currentRangeDistance = Random.Range(rangeDistance.x, rangeDistance.y);
        currentRangeSwap = Random.Range(rangeSwap.x, rangeSwap.y);
    }

    private IEnumerator IntensityFlicker()
    {
        intensitySwapCount++;
        float t = 0.0f;
        
        if(intensitySwapCount > currentIntensitySwap)
        {
            currentIntensitySpeed = Random.Range(intensitySpeed.x, intensitySpeed.y);
            currentIntensityDistance = Random.Range(intensityDistance.x, intensityDistance.y);
            currentIntensitySwap = Random.Range(intensitySwap.x, intensitySwap.y);
            intensitySwapCount = 0;
        }

        while(t < 2 * Mathf.PI)
        {
            l.intensity = averageIntensity + Mathf.Sin(t) * currentIntensityDistance;
            t += Time.deltaTime * currentIntensitySpeed;
            yield return null;
        }

        StartCoroutine(IntensityFlicker());
    }

    private IEnumerator RangeFlicker()
    {
        rangeSwapCount++;
        float t = 0.0f;

        if (rangeSwapCount > currentRangeSwap)
        {
            currentRangeSpeed = Random.Range(rangeSpeed.x, rangeSpeed.y);
            currentRangeDistance = Random.Range(rangeDistance.x, rangeDistance.y);
            currentRangeSwap = Random.Range(rangeSwap.x, rangeSwap.y);
            rangeSwapCount = 0;
        }

        while (t < 2 * Mathf.PI)
        {
            l.range = averageRange + Mathf.Sin(t) * currentRangeDistance;
            t += Time.deltaTime * currentRangeSpeed;
            yield return null;
        }

        StartCoroutine(RangeFlicker());
    }
}