using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class PlayerAwakeEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnPlayerAwake;

    [SerializeField]
    private PlayableDirector introCutscene;

    void Update()
    {
        if(introCutscene.time > 32)
        {
            OnPlayerAwake.Invoke();
            Destroy(this);
        }
    }
}