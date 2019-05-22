using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SpatialShift : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve blendCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [SerializeField]
    private float targetBlend = 0.0f;

    [SerializeField]
    private float blendSpeed = 1.0f;

    [SerializeField]
    private bool workOnAwake = false;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (workOnAwake) BeginShift();
    }

    public void BeginShift()
    {
        StartCoroutine(Shift());
    }

    private IEnumerator Shift()
    {
        float startBlend = audioSource.volume;
        float t = 0.0f;

        while(t < 1.0f)
        {
            t += Time.deltaTime * blendSpeed;
            float c = blendCurve.Evaluate(t);
            audioSource.volume = Mathf.Lerp(startBlend, targetBlend, c);
            yield return null;
        }
    }
}