using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            GetComponent<PlayableDirector>().Play();
        }
    }
}