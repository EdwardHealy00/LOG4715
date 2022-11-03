using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    [SerializeField] private LayerMask m_WhatIsGround;
    private Rigidbody m_Rigidbody;
    const float k_GroundedRadius = .3f;

    public bool Grounded { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        //m_Rigidbody.centerOfMass = new Vector3(0, -.25f, 0);
    }

    private void FixedUpdate()
    {
        Grounded = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                Grounded = true;
        }
    }
}
