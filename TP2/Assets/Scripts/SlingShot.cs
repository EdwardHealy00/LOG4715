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
    private Projection m_Projection;
    [SerializeField] private float m_ThrowSpeed;
    [SerializeField] private float m_MaxPullDistance;
    [SerializeField] private float m_PullDistanceDivider;

    [SerializeField] private GameObject _ballPrefab;
    void Start()
    {
        m_SlingshotState = SlingshotState.Idle;
        m_SlimeManager = GetComponent<SlimeManager>();
        m_Projection = GetComponent<Projection>();
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
                    m_Projection.EnableTrajectory(true);
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

                    m_Projection.SimulateTrajectory(_ballPrefab, m_SlimeManager.BodyCenter.position, m_PullDistance * m_ThrowSpeed);
                }
                else
                {
                    m_SlimeManager.Rigidbody.AddForce(m_PullDistance * m_ThrowSpeed, ForceMode.Impulse);
                    m_SlingshotState = SlingshotState.Idle;
                    m_Projection.EnableTrajectory(false);
                }
                break;

            default:
                break;
        }
    }
}
