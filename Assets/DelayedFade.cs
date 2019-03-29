using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DelayedFade : MonoBehaviour
{
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

    private void Awake()
    {
        foreach (CanvasRenderer cr in uiElements)
            cr.SetAlpha(0);
    }

    public void BeginFade()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
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
    }
}