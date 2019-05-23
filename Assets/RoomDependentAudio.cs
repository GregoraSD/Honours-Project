using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RoomDependentAudio : MonoBehaviour
{
    [SerializeField] private RoomFader roomFader;
    [SerializeField] private float volumeInside = 0;
    [SerializeField] private float volumeOutside = 1;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    private void Update()
    {
        audioSource.volume = Mathf.Lerp(volumeInside, volumeOutside, roomFader.Depth);
    }
}