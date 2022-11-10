using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesLever : MonoBehaviour
{
    [SerializeField] private List<SpikesController> spikes;
    private Animator anim;
    private bool onLever = false;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("LeverUp", true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!onLever && other.CompareTag("Player"))
        {
            anim.SetBool("LeverUp", false);
            onLever = true;
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //anim.SetBool("LeverUp", true);
            //onLever = false;
        }
    }
}
