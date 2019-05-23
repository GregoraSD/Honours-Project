using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

    [SerializeField]
    private AudioSource teleportAudio;

    [SerializeField]
    private AudioSource[] pastAudio;

    [SerializeField]
    private GameObject[] pastObjects;

    [SerializeField]
    private AudioSource[] presentAudio;

    [SerializeField]
    private GameObject[] presentObjects;

    [SerializeField]
    private UnityEvent OnFirstTeleport;

    [SerializeField]
    private PauseMenu pauseMenu;

    [SerializeField]
    private RoomFader roomFader;

    [SerializeField]
    private AudioSource teleportReady, teleportFail;

    private float cooldownTimer = 0.0f;

    private bool isEnabled = false;
    private bool inPast = false;
    private bool inTeleport = false;
    private bool hasTeleportedBefore = false;

    public event System.Action OnTeleportWithCooldown;

    private void Start()
    {
        cooldownTimer = cooldown;
        flash.CrossFadeAlpha(0.0f, 0.0f, true);    
    }

    public void Tick()
    {
        cooldownTimer += Time.deltaTime;
    }

    private void Update()
    {
        if (pauseMenu.gameObject.activeInHierarchy) return;

        bool checkReady = cooldownTimer < cooldown;
        cooldownTimer += Time.deltaTime;

        if (checkReady && cooldownTimer > cooldown && isEnabled) teleportReady.Play();

        if (isEnabled)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (cooldownTimer > cooldown)
                {
                    cooldownTimer = 0.0f;
                    StartCoroutine(Teleport());
                    teleportAudio.Play();
                }
                else
                {
                    teleportFail.Play();
                }
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (cooldownTimer < cooldown || !isEnabled)
            {
                if (OnTeleportWithCooldown != null)
                    OnTeleportWithCooldown();
            }
        }
    }

    private IEnumerator Teleport()
    {
        if (!hasTeleportedBefore)
        {
            OnFirstTeleport.Invoke();
            hasTeleportedBefore = true;
        }

        inTeleport = true;
        flash.enabled = true;
        flash.CrossFadeAlpha(1.0f, flashDuration, false);
        yield return new WaitForSeconds(flashDuration);
        inPast = !inPast;
        if(inPast)
        {
            DisableAudioGroup(presentAudio);
            DisableObjectGroup(presentObjects);
            EnableAudioGroup(pastAudio);
            EnableObjectGroup(pastObjects);
            transform.position += b.position - a.position;
        }
        else
        {
            DisableAudioGroup(pastAudio);
            DisableObjectGroup(pastObjects);
            EnableAudioGroup(presentAudio);
            EnableObjectGroup(presentObjects);
            transform.position += a.position - b.position;
        }

        yield return new WaitForSeconds(waitTime);
        flash.CrossFadeAlpha(0.0f, flashDuration, false);
        roomFader.DetermineClosestRoom();
        yield return null;
    }

    public float GetCooldown() { return cooldown; }
    public float GetCurrentCooldownTime() { return cooldownTimer; }
    public bool IsReady() { return cooldownTimer > cooldown; }
    public bool IsEnabled() { return isEnabled; }
    public void SetEnabled(bool enabled) { isEnabled = enabled; }

    private void EnableAudioGroup(AudioSource[] group)
    {
        for(int i = 0; i < group.Length; i++)
        {
            group[i].Play();
        }
    }

    private void EnableObjectGroup(GameObject[] group)
    {
        for(int i = 0; i < group.Length; i++)
        {
            group[i].SetActive(true);
        }
    }

    private void DisableAudioGroup(AudioSource[] group)
    {
        for (int i = 0; i < group.Length; i++)
        {
            group[i].Stop();
        }
    }

    private void DisableObjectGroup(GameObject[] group)
    {
        for(int i = 0; i < group.Length; i++)
        {
            group[i].SetActive(false);
        }
    }
}