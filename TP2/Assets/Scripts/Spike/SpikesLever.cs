using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesLever : MonoBehaviour
{
    [SerializeField] private List<SpikesController> spikes;
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("LeverUp", true);
    }
    public void ActivateLever()
    {
        anim.SetBool("LeverUp", false);
        foreach(SpikesController spike in spikes)
        {
            if (spike.gameObject.activeInHierarchy)
            {
                spike.gameObject.SetActive(false);
            }
            else
            {
                spike.gameObject.SetActive(true);
            }
        }
    }
}
