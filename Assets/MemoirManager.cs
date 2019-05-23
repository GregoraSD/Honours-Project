using System.Collections.Generic;
using UnityEngine;

public class MemoirManager : MonoBehaviour
{
    public static int MemoirCount { get; private set; }
    public static int FoundMemoirs { get; private set; }

    [SerializeField] private DelayedFade textFade;
    [SerializeField] private TMPro.TextMeshProUGUI textGUI;
    [SerializeField] private AudioSource foundAudio;
    private Queue<Memoir> memoirQueue = new Queue<Memoir>();

    private void Awake()
    {
        FoundMemoirs = 0;
        MemoirCount = FindObjectsOfType<Memoir>().Length;
    }

    public void DiscoverMemoir(Memoir memoir)
    {
        if(textFade.IsFading)
        {
            memoirQueue.Enqueue(memoir);
        }
        else
        {
            FoundMemoirs++;
            textGUI.text = "Memoir Found: " + FoundMemoirs + "/" + MemoirCount;
            textFade.BeginFade();
            foundAudio.Play();
        }
    }

    private void Update()
    {
        if(memoirQueue.Count > 0)
        {
            if(!textFade.IsFading)
            {
                memoirQueue.Dequeue();
                FoundMemoirs++;
                textGUI.text = "Memoir Found: " + FoundMemoirs + "/" + MemoirCount;
                textFade.BeginFade();
                foundAudio.Play();
            }
        }
    }
}