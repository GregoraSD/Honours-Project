using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TeleportAbility : MonoBehaviour
{
    [SerializeField]
    private Transform a, b;

    [SerializeField]
    private Image flash;

    [SerializeField]
    private float flashDuration = 1.0f;

    [SerializeField]
    private float waitTime = 0.0f;

    [SerializeField]
    private float cooldown = 5.0f;

    private float cooldownTimer = 0.0f;
    private bool inPast = false;
    private bool inTeleport = false;

    private void Start()
    {
        cooldownTimer = cooldown;
        flash.CrossFadeAlpha(0.0f, 0.0f, true);    
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if(Input.GetButtonDown("Jump"))
        {
            if(cooldownTimer > cooldown)
            {
                cooldownTimer = 0.0f;
                StartCoroutine(Teleport());
            }
            
        }
    }

    private IEnumerator Teleport()
    {
        inTeleport = true;
        flash.enabled = true;
        flash.CrossFadeAlpha(1.0f, flashDuration, false);
        yield return new WaitForSeconds(flashDuration);
        inPast = !inPast;
        if(inPast)
        {
            transform.position += b.position - a.position;
        }
        else
        {
            transform.position += a.position - b.position;
        }
        yield return new WaitForSeconds(waitTime);
        flash.CrossFadeAlpha(0.0f, flashDuration, false);
        yield return null;
    }
}