using UnityEngine;

public class RoomFootstepVolume : MonoBehaviour
{
    [SerializeField] private RoomFader roomFader;
    [SerializeField] private StepCycleGroup step;
    [SerializeField] private float volumeInside = 0f;
    [SerializeField] private float volumeOutside = 1f;

    private void Update()
    {
        step.volume = Mathf.Lerp(volumeInside, volumeOutside, roomFader.Depth);
    }
}