using UnityEngine;
using System.Collections;
using System;

public enum SlingshotState
{
    Idle,
    UserPulling
}
public class SlingShot : MonoBehaviour
{
    private SlingshotState m_SlingshotState;
    private Vector3 m_StartPullPos;
    private Vector3 m_PullDistance;
    private SlimeManager m_SlimeManager;

    public float m_ThrowSpeed;
    public float m_MaxPullDistance;
    public float m_PullDistanceDivider;

    private Rigidbody m_Rigidbody;
    void Start()
    {
        m_SlingshotState = SlingshotState.Idle;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_SlimeManager = GetComponent<SlimeManager>();
    }

    void Update()
    {
        switch (m_SlingshotState)
        {
            case SlingshotState.Idle:
                if (Input.GetMouseButtonDown(0) && m_SlimeManager.Grounded)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit raycastHit;
                    if (Physics.Raycast(ray, out raycastHit, 100f))
                    {
                        if (raycastHit.transform == transform)
                        {
                            m_StartPullPos = Input.mousePosition;
                            m_SlingshotState = SlingshotState.UserPulling;
                        }
                    }
                }
                break;

            case SlingshotState.UserPulling:
                if (Input.GetMouseButton(0))
                {
                    m_PullDistance = m_StartPullPos - Input.mousePosition;
                    m_PullDistance /= m_PullDistanceDivider;

                    if (m_PullDistance.magnitude > m_MaxPullDistance)
                    {
                        m_PullDistance = m_PullDistance.normalized * m_MaxPullDistance;
                    }
                }
                else
                {
                    m_Rigidbody.AddForce(m_PullDistance * m_ThrowSpeed, ForceMode.Impulse);
                    m_SlingshotState = SlingshotState.Idle;
                }
                break;

            default:
                break;
        }

    }
}
