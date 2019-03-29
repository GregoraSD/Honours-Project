using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class CutsceneEndListener : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector director;

    [SerializeField]
    private float endTime;

    [SerializeField]
    private UnityEvent OnCutsceneEnd;

    private void Update()
    {
        if(director.time > endTime)
        {
            OnCutsceneEnd.Invoke();
            Destroy(this);
        }
    }
}