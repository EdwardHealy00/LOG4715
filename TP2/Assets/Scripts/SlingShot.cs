using UnityEngine;
using System.Collections;
using System;
public class SlingShot : MonoBehaviour
{
    private Vector3 m_StartPullPos;
    private Vector3 m_PullDistance;
    private SlimeManager m_SlimeManager;
    private Projection m_Projection;
    private const float k_MinPullDistance = 1f;

    [Header("SlingShot Settings")]
    [SerializeField] private float m_ThrowSpeed;
    [SerializeField] private float m_MaxPullDistance;
    [SerializeField] private float m_PullDistanceDivider;

    
    void Start()
    {
        m_SlimeManager = GetComponent<SlimeManager>();
        m_SlimeManager.SlingshotState = SlingshotState.Idle;
        m_Projection = GetComponent<Projection>();
    }

    void Update()
    {
        //Debug.Log(m_SlimeManager.Grounded);
        switch (m_SlimeManager.SlingshotState)
        {
            
            case SlingshotState.Idle:
                if (Input.GetMouseButtonDown(0) && m_SlimeManager.Grounded && m_SlimeManager.CanUseColor(m_SlimeManager.NextColor))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f))
                    {
                        if (raycastHit.transform == transform)
                        {
                            m_StartPullPos = Input.mousePosition;
                            m_SlimeManager.SlingshotState = SlingshotState.UserPulling;
                        }
                    }
                    m_Projection.EnableTrajectory(true);
                }
                break;

            case SlingshotState.UserPulling:
                if (Input.GetMouseButtonDown(1))
                {
                    m_SlimeManager.SlingshotState = SlingshotState.Idle;
                    m_Projection.EnableTrajectory(false);
                }
                else if (Input.GetMouseButton(0))
                {
                    m_PullDistance = m_StartPullPos - Input.mousePosition;
                    m_PullDistance /= m_PullDistanceDivider;

                    if (m_PullDistance.magnitude > m_MaxPullDistance)
                    {
                        m_PullDistance = m_PullDistance.normalized * m_MaxPullDistance;
                    }
                    
                    if (m_PullDistance.magnitude < k_MinPullDistance)
                    {
                        m_Projection.EnableTrajectory(false);
                    } 
                    else
                    {
                        m_SlimeManager.SlingshotState = SlingshotState.UserPulling;
                        m_Projection.EnableTrajectory(true);
                        m_Projection.SetLineWidth(m_PullDistance.magnitude / m_MaxPullDistance);
                        m_Projection.DrawProjection(m_SlimeManager.BodyCenter.position, m_PullDistance * m_ThrowSpeed);
                    }

                }
                else if (m_PullDistance.magnitude > k_MinPullDistance)
                {
                    m_SlimeManager.SlingshotState = SlingshotState.Moving;
                    m_SlimeManager.Rigidbody.isKinematic = false;
                    m_SlimeManager.Rigidbody.AddForce(m_PullDistance * m_ThrowSpeed, ForceMode.Impulse);
                    m_SlimeManager.UseColor();
                    m_Projection.EnableTrajectory(false);
                }
                else if (m_PullDistance.magnitude < k_MinPullDistance)
                {
                    m_SlimeManager.SlingshotState = SlingshotState.Idle;
                    m_Projection.EnableTrajectory(false);
                }
                break;

            default:
                break;
        }
    }

    
}
