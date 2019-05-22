using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuTeleportController : MonoBehaviour
{
    [SerializeField] private Camera camA, camB, camC;

    [SerializeField]
    private Image flash;

    [SerializeField]
    private float flashDuration = 1.0f;

    [SerializeField]
    private float waitTime = 0.0f;

    [SerializeField]
    private AudioSource teleportAudio;

    [SerializeField]
    private SceneManager sceneManager;

    [SerializeField]
    private Image exitScreen;

    [SerializeField]
    private GameObject[] loadDisable;

    [SerializeField]
    private AudioSource windAudio;

    private bool inTeleport;

    private void Awake()
    {
        exitScreen.CrossFadeAlpha(0, 0, true);
    }

    public void PlayButtonClicked()
    {
        StartCoroutine(Teleport(camA, camB, true));
    }

    public void OptionsButtonClicked()
    {
        StartCoroutine(Teleport(camA, camC, false));
    }

    public void OptionsBackButtonClicked()
    {
        StartCoroutine(Teleport(camC, camA, false));
    }

    private IEnumerator Teleport(Camera from, Camera to, bool loadFlag)
    {
        teleportAudio.Play();
        inTeleport = true;
        flash.enabled = true;
        flash.CrossFadeAlpha(1.0f, flashDuration, false);
        yield return new WaitForSeconds(flashDuration);

        if(loadFlag)
        {
            foreach(GameObject go in loadDisable)
                go.SetActive(false);
        }

        from.gameObject.SetActive(false);
        to.gameObject.SetActive(true);

        yield return new WaitForSeconds(waitTime);
        flash.CrossFadeAlpha(0.0f, flashDuration, false);
        yield return null;

        if(loadFlag)
        {
            StartCoroutine(TransitionToLevel());
        }
    }

    private IEnumerator TransitionToLevel()
    {
        yield return new WaitForSeconds(1);

        float t = 0.0f;
        exitScreen.GetComponent<DelayedFade>().BeginFade();
        while(t < 1.0f)
        {
            t += Time.deltaTime;
            windAudio.volume -= Time.deltaTime * 0.5f;
        }

        yield return new WaitForSeconds(1);

        sceneManager.LoadScene(1);
    }
}