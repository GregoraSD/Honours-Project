using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportUITrigger : MonoBehaviour
{
    [SerializeField]
    private TeleportUIController teleportUIController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            teleportUIController.Enable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            teleportUIController.Disable();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            teleportUIController.Enable();
        }
    }
}