using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesLever : MonoBehaviour
{
    [SerializeField] private SpikesController spikes;

    private bool onLever = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!onLever && other.CompareTag("Player"))
        {
            onLever = true;
            if (spikes.gameObject.activeInHierarchy)
            {
                spikes.gameObject.SetActive(false);
            }
            else
            {
                spikes.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onLever = false;
        }
    }
}
