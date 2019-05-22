using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class ShadowFlicker : MonoBehaviour
{
    [SerializeField] private Vector2 shadowStrengthRange = new Vector2(0, 1);
    [SerializeField] private float speed = 1;

    private float midpoint;
    private float distance;

    private Light l;

    private void Awake()
    {
        l = GetComponent<Light>();

        distance = shadowStrengthRange.y - shadowStrengthRange.x;
        midpoint = shadowStrengthRange.x + distance * 0.5f;
    }

    private void Update()
    {
        Debug.Log(midpoint + (Mathf.Sin(Time.time) * (distance * 0.5f)));
    }
}