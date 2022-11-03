using UnityEngine;


public class SlimeManager : MonoBehaviour
{
    [SerializeField] private LayerMask m_WhatIsGround;
    private const float k_Epsilon = 1f;

    public bool Grounded { get; set; }
    public Transform BodyCenter { get; set; }
    public Rigidbody Rigidbody { get; set; }

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        BodyCenter = transform.Find("BodyCenter");
    }
    void OnCollisionStay(Collision collisionInfo)
    {
        Grounded = false;
        if (Rigidbody.velocity.magnitude < k_Epsilon && m_WhatIsGround == (m_WhatIsGround | (1 << collisionInfo.gameObject.layer)))
        {
            Grounded = true; 
        }
        Debug.Log(Grounded);
    }
}
