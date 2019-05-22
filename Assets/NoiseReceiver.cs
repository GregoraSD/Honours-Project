using UnityEngine;

public class NoiseReceiver : MonoBehaviour
{
    [SerializeField] private Vector3 scale = Vector3.one;
    [SerializeField] private float speed = 1;

    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        float x = Mathf.PerlinNoise(Time.time * speed, 0) * scale.x;
        float y = Mathf.PerlinNoise(Time.time * speed, 1000) * scale.y;
        float z = Mathf.PerlinNoise(Time.time * speed, 2000) * scale.z;

        transform.position = originalPosition + new Vector3(x, y, z);
    }
}