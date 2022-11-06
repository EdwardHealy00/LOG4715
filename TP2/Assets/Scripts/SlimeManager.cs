using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SlimeManager : MonoBehaviour
{
    [SerializeField] private LayerMask m_WhatIsGround;
    private const float k_Epsilon = .1f;

    public bool Grounded { get; set; }
    public Transform BodyCenter { get; set; }
    public Rigidbody Rigidbody { get; set; }

    public SlimeColor Color { get; set; }

    public Dictionary<SlimeColor, int> OrbCount { get; set; }

    public SlingshotState SlingshotState { get; set; }

    private const float k_NonStickRadius = .5f;
    private Vector3 m_LastCollisionPoint;


    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        BodyCenter = transform.Find("BodyCenter");
        Color = SlimeColor.Green;
        m_LastCollisionPoint = transform.position;
    }
    void OnCollisionStay(Collision collisionInfo)
    {
        CheckGrounded(collisionInfo);
        m_LastCollisionPoint = transform.position;
    }

    private void CheckGrounded(Collision collisionInfo)
    {
        //Debug.Log(collisionInfo.gameObject.name);
        Grounded = false;
        
        if (Rigidbody.velocity.magnitude < k_Epsilon && m_WhatIsGround == (m_WhatIsGround | (1 << collisionInfo.gameObject.layer)))
        {
            Grounded = true;
            SlingshotState = SlingshotState.Idle;
        }
    }
    
    void OnCollisionEnter(Collision collisionInfo)
    {
        switch (Color)
        {
            case SlimeColor.Yellow:
                if (SlingshotState == SlingshotState.Moving && (m_LastCollisionPoint - transform.position).magnitude > k_NonStickRadius)
                {
                    m_LastCollisionPoint = transform.position;
                    SlingshotState = SlingshotState.Idle;
                    Rigidbody.velocity = Vector3.zero;
                    Rigidbody.isKinematic = true;
                    Grounded = false;
                }
                break;

            default:
                break;
        }
        CheckGrounded(collisionInfo);
    }
}
