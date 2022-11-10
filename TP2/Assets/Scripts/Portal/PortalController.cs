using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] private PortalController otherPortal;
    private GameObject player;
    private bool teleported = false;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!teleported)
        {
            otherPortal.teleported = true;
            player.transform.position = otherPortal.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        teleported = false;
    }
}
