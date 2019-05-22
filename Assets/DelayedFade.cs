using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DelayedFade : MonoBehaviour
{
    public bool IsFading { get; private set; }

    [SerializeField]
    private float inDelay = 1;

    [SerializeField]
    private float inSpeed, outSpeed;

    [SerializeField]
    private AnimationCurve inCurve, outCurve;

    [SerializeField]
    private float waitAfterEntry = 1.0f;

    [SerializeField]
    private CanvasRenderer[] uiElements;

    [SerializeField]
    private bool fadeOnAwake = false;

    private void Awake()
    {
        foreach (CanvasRenderer cr in uiElements)
            cr.SetAlpha(0);

        if (fadeOnAwake) BeginFade();
    }

    public void BeginFade()
    {
        StartCoroutine(Fade());
    }

    public void SetOutSpeed(float amount)
    {
        outSpeed = amount;
    }

    private IEnumerator Fade()
    {
        IsFading = true;
        yield return new WaitForSeconds(inDelay);

        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime * inSpeed;
            float c = inCurve.Evaluate(t);
            foreach (CanvasRenderer cr in uiElements)
                cr.SetAlpha(c);
            yield return null;
        }

        yield return new WaitForSeconds(waitAfterEntry);

        while(t > 0.0f)
        {
            t -= Time.deltaTime * outSpeed;
            float c = outCurve.Evaluate(t);
            foreach (CanvasRenderer cr in uiElements)
                cr.SetAlpha(c);
            yield return null;
        }

        IsFading = false;
    }
}