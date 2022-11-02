using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.centerOfMass = new Vector3(0, -.25f, 0);
    }
}
