using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WoodCreak : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips = null;
    [SerializeField] private float minTime = 3;
    [SerializeField] private float maxTime = 7;
    [SerializeField] [Range(0f, 3f)] private float minPitch = 0.8f;
    [SerializeField] [Range(0f, 3f)] private float maxPitch = 1.2f;
    [SerializeField] [Range(0f, 1f)] private float minVolume = 0.3f;
    [SerializeField] [Range(0f, 1f)] private float maxVolume = 0.4f;

    private float timer;
    private float currentCooldown;
    private AudioSource audioSource;
    private AudioReverbZone test;

    private void Awake()
    {
        AssignNewCooldown();
        audioSource = GetComponent<AudioSource>();
    }

    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

    private void AssignNewCooldown()
    {
        currentCooldown = Random.Range(minTime, maxTime);
    }

    private void PlayRandomSound()
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume = Random.Range(minVolume, maxVolume);
        audioSource.PlayOneShot(GetRandomClip());
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > currentCooldown)
        {
            timer = 0.0f;
            AssignNewCooldown();
            PlayRandomSound();
        }
    }
}