using UnityEngine;

public class MemoirManager : MonoBehaviour
{
    public static int MemoirCount { get; private set; }
    public static int FoundMemoirs { get; private set; }

    [SerializeField] private DelayedFade textFade;
    [SerializeField] private TMPro.TextMeshProUGUI textGUI;

    private void Awake()
    {
        FoundMemoirs = 0;
        MemoirCount = FindObjectsOfType<Memoir>().Length;
    }

    public void DiscoverMemoir(Memoir memoir)
    {
        FoundMemoirs++;
        textGUI.text = "Memoir Found: " + FoundMemoirs + "/" + MemoirCount;

        if(!textFade.IsFading)
            textFade.BeginFade();
    }
}