using UnityEngine;

public class RoomFader : MonoBehaviour
{
    public float Depth { get; private set; }
    [SerializeField] private Transform target;
    [SerializeField] private AudioReverbZone[] rooms;
    [SerializeField] private float refreshTimer = 3;

    private AudioReverbZone closestRoom;
    private float previousRefreshTime;

    private void Awake()
    {
        DetermineClosestRoom();    
    }

    public void DetermineClosestRoom()
    {
        int currentClosest = 0;
        float currentClosestDistance = Vector3.SqrMagnitude(rooms[currentClosest].transform.position - target.position);

        for(int i = 1; i < rooms.Length; i++)
        {
            float d = Vector3.SqrMagnitude(rooms[i].transform.position - target.position);

            if (d < currentClosestDistance)
            {
                currentClosest = i;
                currentClosestDistance = d;
            }
        }

        closestRoom = rooms[currentClosest];
    }

    private void Update()
    {
        if(Time.time - previousRefreshTime > refreshTimer)
        {
            DetermineClosestRoom();
            previousRefreshTime = Time.time;
        }

        float distance = Vector3.SqrMagnitude(closestRoom.transform.position - target.position);
        float min = closestRoom.minDistance * closestRoom.minDistance;
        float max = closestRoom.maxDistance * closestRoom.maxDistance;

        if (distance < min) Depth = 0;
        else if (distance > max) Depth = 1;
        else Depth = (distance - min) / (max - min);
    }
}