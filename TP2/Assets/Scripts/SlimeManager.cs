using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SlimeManager : MonoBehaviour
{
    public bool Grounded { get; set; }
    public Transform BodyCenter { get; set; }
    public Rigidbody Rigidbody { get; set; }

    public SlimeColor CurrentColor { get; set; }
    public SlimeColor NextColor { get; set; }

    public Dictionary<SlimeColor, SlimeOrb> Orbs;

    public Dictionary<SlimeColor, int> OrbCount { get; set; }

    public SlingshotState SlingshotState { get; set; }

    private const float k_NonStickRadius = .5f;
    private Vector3 m_LastCollisionPoint;
    private SkinnedMeshRenderer m_SkinnedMeshRenderer;
    private SphereCollider m_SphereCollider;
    private Projection m_Projection;

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
        Orbs = SlimeOrbsGenerator.GenerateOrbs();
        Orbs[SlimeColor.Green].Amount = 1;
        Rigidbody = GetComponent<Rigidbody>();
        BodyCenter = transform.Find("BodyCenter");
        m_LastCollisionPoint = transform.position;
        m_SkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        m_SphereCollider = GetComponent<SphereCollider>();
        m_Projection = GetComponent<Projection>();
        
        ForceChangeColor(SlimeColor.Green);
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
        switch (CurrentColor)
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

    public void ChangeColorFromWheel(SlimeColor selectedOrb)
    {
        NextColor = selectedOrb;
        m_Projection.SetLineMaterial(Orbs[selectedOrb].Material);
        if (!Grounded)
        {
            CurrentColor = selectedOrb;
            m_SkinnedMeshRenderer.material = Orbs[selectedOrb].Material;
            m_SphereCollider.material = Orbs[selectedOrb].PhysicMaterial;
        }
    }
    
    public void ForceChangeColor(SlimeColor selectedOrb)
    {
        CurrentColor = selectedOrb;
        NextColor = selectedOrb;
        m_SkinnedMeshRenderer.material = Orbs[selectedOrb].Material;
        m_Projection.SetLineMaterial(Orbs[selectedOrb].Material);
        m_SphereCollider.material = Orbs[selectedOrb].PhysicMaterial;
       
    }
}
