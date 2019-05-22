using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject[] forceActive;
    [SerializeField] private GameObject[] forceInactive;
    [SerializeField] private AudioSource pauseAudio;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private bool ignoreCancel = false;
    [SerializeField] private GameObject backButton;

    private Resolution[] resolutions;

    private void Awake()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolutions[i].width + " x " + resolutions[i].height));

            if (Screen.currentResolution.width == resolutions[i].width && Screen.currentResolution.height == resolutions[i].height)
            {
                resolutionDropdown.value = i;
                resolutionDropdown.RefreshShownValue();
            }
        }

        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();

        fullscreenToggle.isOn = Screen.fullScreen;

        float v;
        audioMixer.GetFloat("volume", out v);
        volumeSlider.value = v;

        eventSystem.SetSelectedGameObject(null);
    }

    private void Update()
    {
        Scroll(resolutionDropdown);
        Scroll(qualityDropdown);

        if (!ignoreCancel)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (!options.activeInHierarchy)
                {
                    Toggle();
                }
            }
        }

        if(eventSystem.currentSelectedGameObject == null)
        {
            if(Input.GetAxisRaw("Vertical") > 0)
            {
                if (options.activeInHierarchy) eventSystem.SetSelectedGameObject(backButton);
                else eventSystem.SetSelectedGameObject(forceActive[2]);
            }
            else if(Input.GetAxisRaw("Vertical") < 0)
            {
                if (options.activeInHierarchy) eventSystem.SetSelectedGameObject(fullscreenToggle.gameObject);
                else eventSystem.SetSelectedGameObject(forceActive[0]);
            }
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }

    public void SetFullscreen(bool fullScreen)
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, fullScreen);
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    public void Toggle()
    {
        if(options.activeInHierarchy)
        {
            return;
        }
        else
        {
            pauseAudio.pitch = Random.Range(2.5f, 3);
            pauseAudio.Play();

            if (isActiveAndEnabled)
            {
                eventSystem.SetSelectedGameObject(null);
                gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
            }
            else
            {
                gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;

                ForceMenuActive();
            }
        }
    }

    public void ForceMenuActive()
    {
        foreach (GameObject go in forceActive)
            go.SetActive(true);

        foreach (GameObject go in forceInactive)
            go.SetActive(false);

        eventSystem.SetSelectedGameObject(forceActive[0]);
    }

    public void ForceOptionsActive()
    {
        foreach (GameObject go in forceActive)
            go.SetActive(false);

        foreach (GameObject go in forceInactive)
            go.SetActive(true);
    }

    private void Scroll(Dropdown dropdown)
    {
        if (eventSystem.currentSelectedGameObject == null) return;

        if (eventSystem.currentSelectedGameObject == dropdown.gameObject)
        {
            if (Input.GetButtonUp("Vertical"))
            {
                Transform dropdownListTransform = dropdown.gameObject.transform.Find("Dropdown List");
                if (dropdownListTransform == null)
                {
                    // Show the dropdown when the user hits the arrow keys if the dropdown is not already showing
                    dropdown.Show();
                }
            }
        }
        else
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(eventSystem);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            eventSystem.RaycastAll(eventDataCurrentPosition, results);
            if (results.Count > 0)
            {
                if (results[0].gameObject.transform.IsChildOf(dropdown.gameObject.transform))
                {
                    // Pointer over the list, use default behavior
                    return;
                }
            }

            // Autoscroll list as the selected object is changed from the arrow keys
            if (eventSystem.currentSelectedGameObject.transform.IsChildOf(dropdown.gameObject.transform))
            {
                if (eventSystem.currentSelectedGameObject.name.StartsWith("Item "))
                {
                    // Skip disabled items
                    Transform parent = eventSystem.currentSelectedGameObject.transform.parent;
                    int activeChildren = 0;
                    int totalChildren = parent.childCount;
                    for (int childIndex = 0; childIndex < totalChildren; childIndex++)
                    {
                        if (parent.GetChild(childIndex).gameObject.activeInHierarchy)
                        {
                            activeChildren++;
                        }
                    }
                    int myActiveIndex = 0;
                    for (int childIndex = 0; childIndex < totalChildren; childIndex++)
                    {
                        if (parent.GetChild(childIndex).gameObject == eventSystem.currentSelectedGameObject)
                        {
                            break;
                        }
                        else if (parent.GetChild(childIndex).gameObject.activeInHierarchy)
                        {
                            myActiveIndex++;
                        }
                    }

                    if (activeChildren > 1)
                    {
                        GameObject scrollbarGameObject = GameObject.Find("Scrollbar");
                        if (scrollbarGameObject != null && scrollbarGameObject.activeInHierarchy)
                        {
                            Scrollbar scrollbar = scrollbarGameObject.GetComponent<Scrollbar>();
                            if (scrollbar.direction == Scrollbar.Direction.TopToBottom)
                                scrollbar.value = (float)myActiveIndex / (float)(activeChildren - 1);
                            else
                                scrollbar.value = 1.0f - (float)myActiveIndex / (float)(activeChildren - 1);
                        }
                    }
                }
            }
        }
    }
}