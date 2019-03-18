using UnityEngine;

public class TeleportCooldownUI : MonoBehaviour
{
    [SerializeField]
    private TeleportAbility teleportAbility;

    private RectTransform rectTransform;
    private float maxHeight;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        maxHeight = rectTransform.rect.height;
    }

    private void Update()
    {
        float teleportScale = Mathf.Clamp01(teleportAbility.GetCurrentCooldownTime() / teleportAbility.GetCooldown());
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, maxHeight * teleportScale);
    }
}