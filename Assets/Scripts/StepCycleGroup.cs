using UnityEngine;

[CreateAssetMenu(fileName = "Step Cycle Group")]
public class StepCycleGroup : ScriptableObject
{
    public AudioClip[] clips;
    public string tag;
    public float volume = 0.05f;

    public AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}