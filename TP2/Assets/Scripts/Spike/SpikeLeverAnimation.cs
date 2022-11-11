using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeLeverAnimation : MonoBehaviour
{
    private Animator m_pressSpaceIcon;
    private bool leverPressed = false;
    private SlimeManager m_slime;
    private SpikesLever lever;
    private void Start()
    {
        m_pressSpaceIcon = transform.Find("PressSpaceIcon").GetComponent<Animator>();
        m_slime = GameObject.FindGameObjectWithTag("Player").GetComponent<SlimeManager>();
        lever = transform.GetComponent<SpikesLever>();
    }
    private void Update()
    {
        if (CanPressLever())
        {
            m_pressSpaceIcon.SetBool("CanTrade", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                leverPressed = true;
                lever.ActivateLever();
                m_pressSpaceIcon.SetBool("CanTrade", false);
            }
        }
        else
        {
            m_pressSpaceIcon.SetBool("CanTrade", false);
        }
    }

    private bool CanPressLever()
    {
        return !leverPressed && Vector3.Distance(transform.position, m_slime.transform.position) < 2f && m_slime.Grounded;
    }
}
