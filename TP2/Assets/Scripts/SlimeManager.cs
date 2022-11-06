using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SlimeManager : MonoBehaviour
{
    public bool Grounded { get; set; }
    public Transform BodyCenter { get; set; }
    public Rigidbody Rigidbody { get; set; }

    public SlimeColor Color { get; set; }

    public Dictionary<SlimeColor, int> OrbCount { get; set; }

    public SlingshotState SlingshotState { get; set; }

    private const float k_NonStickRadius = .5f;
    private Vector3 m_LastCollisionPoint;

    [Header("Slime Materials")]
    [SerializeField] private Material m_GreenMaterial;
    [SerializeField] private Material m_PinkMaterial;
    [SerializeField] private Material m_YellowMaterial;
    [SerializeField] private Material m_BlueMaterial;
    [SerializeField] private Material m_OrangeMaterial;

    [Header("Ground")]
    [SerializeField] private LayerMask m_WhatIsGround;

    [Header("Null Slime Veclocity")]
    [SerializeField] private  float m_Epsilon = .1f;

    public Dictionary<SlimeColor, Material> SlimeMaterials { get; set; }


    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        BodyCenter = transform.Find("BodyCenter");
        Color = SlimeColor.Green;
        m_LastCollisionPoint = transform.position;
        SlimeMaterials = new Dictionary<SlimeColor, Material>
        {
            {SlimeColor.Green, m_GreenMaterial},
            {SlimeColor.Pink, m_PinkMaterial},
            {SlimeColor.Yellow, m_YellowMaterial},
            {SlimeColor.Blue, m_BlueMaterial},
            {SlimeColor.Orange, m_OrangeMaterial}
        };

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

        if (Rigidbody.velocity.magnitude < m_Epsilon && m_WhatIsGround == (m_WhatIsGround | (1 << collisionInfo.gameObject.layer)))
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
