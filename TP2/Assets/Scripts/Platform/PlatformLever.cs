using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLever : MonoBehaviour
{
    [SerializeField] private PlatformController platform;
    private Animator anim;

    private bool onLever = false;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("LeverUp", true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!onLever && other.CompareTag("Player"))
        {
            anim.SetBool("LeverUp", false);
            onLever = true;
            platform.TargetNextWaypoint();
            platform.leverPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //onLever = false;
        }
    }
}
