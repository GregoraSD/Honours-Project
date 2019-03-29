using UnityEngine;

[CreateAssetMenu(fileName = "Step Cycle Group")]
public class StepCycleGroup : ScriptableObject
{
    public AudioClip[] clips;
    public string tag;

    public AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}