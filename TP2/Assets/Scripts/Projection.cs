using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Inspired by: https://github.com/llamacademy/projectile-trajectory/
public class Projection : MonoBehaviour
{
    private LineRenderer m_Line;
    [Header("Trajectory Line Length")]
    [SerializeField] private uint m_Length = 2;
    private float m_TimeStep = 0.1f;

    [Header("Trajectory Line Width")]
    [SerializeField] private float m_MinWidth = 0.01f;
    [SerializeField] private float m_MaxWidth = 0.1f;

    [Header("Parent Object of All Obstacles")]
    [SerializeField] private Transform m_ObstaclesParent;

    [Header("GameObject Simulating the Trajectory")]
    [SerializeField] private GameObject m_ballPrefab;

    private LayerMask ObstacleLayerMask;

    private void Awake()
    {
        m_Line = GetComponent<LineRenderer>();
        m_Line.enabled = false;
        ObstacleLayerMask = LayerMask.GetMask("Obstacle");
    }

    public void DrawProjection(Vector3 pos, Vector3 velocity)
    {
        m_Line.enabled = true;
        m_Line.positionCount = Mathf.CeilToInt(m_Length / m_TimeStep) + 1;
        Vector3 startPosition = pos;
        Vector3 startVelocity = velocity;
        int i = 0;
        m_Line.SetPosition(i, startPosition);
        for (float time = 0; time < m_Length; time += m_TimeStep)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            m_Line.SetPosition(i, point);

            Vector3 lastPosition = m_Line.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition,
                (point - lastPosition).normalized,
                out RaycastHit hit,
                (point - lastPosition).magnitude, ObstacleLayerMask) && hit.transform != transform)
            {
                m_Line.SetPosition(i, hit.point);
                m_Line.positionCount = i + 1;
                return;
            }
        }
    }

    public void EnableTrajectory(bool enable)
    {
        m_Line.positionCount = 0;
        m_Line.enabled = enable;
    }

    public void SetLineWidth(float width)
    {
        width = Mathf.Lerp(m_MinWidth, m_MaxWidth, width);
        m_Line.startWidth = width;
        m_Line.endWidth = width;
    }

    public void SetLineMaterial(Material material)
    {
        m_Line.material = material;
    }

}
