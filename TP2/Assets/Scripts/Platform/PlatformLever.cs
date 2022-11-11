using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLever : MonoBehaviour
{
    [SerializeField] private PlatformController platform;
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("LeverUp", true);
    }
    public void ActivateLever()
    {
        anim.SetBool("LeverUp", false);
        platform.TargetNextWaypoint();
        platform.leverPressed = true;
    }
}
