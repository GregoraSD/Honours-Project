using UnityEngine;
using UnityEngine.UI;

public class TeleportUIController : MonoBehaviour
{
    [SerializeField]
    private TeleportAbility teleportAbility;

    [SerializeField]
    private Image[] uiElements;

    [SerializeField]
    private float disabledAlpha = 0.6f, enabledAlpha = 1.0f;

    [SerializeField]
    private float enterDuration = 1.0f, exitDuration = 1.0f;

    private bool activated = false;

    private void Update()
    {
        activated = teleportAbility.IsEnabled() ? true : activated;
        if (!activated) return;

        if(!teleportAbility.IsEnabled() || !teleportAbility.IsReady())
        {
            Disable();
        }
        else
        {
            Enable();

        }
    }

    private void Awake()
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].CrossFadeAlpha(0.0f, 0.0f, true);
        }
    }

    public void Enable()
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].CrossFadeAlpha(enabledAlpha, enterDuration, true);
        }
    }

    public void Disable()
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].CrossFadeAlpha(disabledAlpha, exitDuration, true);
        }
    }
}