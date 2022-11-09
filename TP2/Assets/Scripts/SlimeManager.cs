using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class SlimeManager : MonoBehaviour
{
    public bool Grounded { get; set; }
    public Transform BodyCenter { get; set; }
    public Rigidbody Rigidbody { get; set; }

    public SlimeColor CurrentColor { get; set; }
    public SlimeColor NextColor { get; set; }
    public bool GameOver { get; set; } = false;

    public Dictionary<SlimeColor, SlimeOrb> Orbs;

    public SlingshotState SlingshotState { get; set; }

    private const float k_NonStickRadius = .5f;
    private Vector3 m_LastCollisionPoint;
    private SkinnedMeshRenderer m_SkinnedMeshRenderer;
    private SphereCollider m_SphereCollider;
    private Projection m_Projection;
    private CursorManager m_CursorManager;


    [Header("Grounded Settings")]
    [SerializeField] private float m_NullSlimeVelocity = .1f;
    [SerializeField] private float m_TimeBeforeGrounded = .1f;

    private float m_GroundedTimer = 0;



    void Awake()
    {
        Orbs = SlimeOrbsGenerator.GenerateOrbs();
        Orbs[SlimeColor.Green].Amount = 2;
        Orbs[SlimeColor.Yellow].Amount = 0;
        Orbs[SlimeColor.Pink].Amount = 0;
        Rigidbody = GetComponent<Rigidbody>();
        BodyCenter = transform.Find("BodyCenter");
        m_LastCollisionPoint = transform.position;
        m_SkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        m_SphereCollider = GetComponent<SphereCollider>();
        m_Projection = GetComponent<Projection>();
        m_CursorManager = FindObjectOfType<CursorManager>();

        ForceChangeColor(SlimeColor.Green);
    }

    void Update()
    {
        CheckGrounded();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f) && raycastHit.transform == transform)
        {

            if (GameOver)
            {
                m_CursorManager.SetCursor(CursorState.Busy);
            }
            else if (SlingshotState == SlingshotState.Idle)
            {
                m_CursorManager.SetCursor(CursorState.Grab);
            }
            else if (SlingshotState == SlingshotState.UserPulling)
            {
                m_CursorManager.SetCursor(CursorState.Grabbing);
            }
            else if (SlingshotState == SlingshotState.Moving)
            {
                m_CursorManager.SetCursor(CursorState.Busy);
            }
        }
        else
        {
            if (SlingshotState != SlingshotState.UserPulling)
                m_CursorManager.ResetCursor();
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        m_LastCollisionPoint = transform.position;
    }

    private void CheckGrounded()
    {
        if (Rigidbody.velocity.magnitude < m_NullSlimeVelocity)
        {
            if (Grounded) return;

            m_GroundedTimer += Time.deltaTime;
            if (m_GroundedTimer < m_TimeBeforeGrounded)
            {
                Grounded = false;
                return;
            }
            m_GroundedTimer = 0;

            Grounded = true;
            SlingshotState = SlingshotState.Idle;
            m_CursorManager.SetCursor(CursorState.Pointer);
            AutoSetNextColor();
        }
        else
        {
            Grounded = false;
            m_GroundedTimer = 0;
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
        CheckGrounded();
    }

    public void ChangeColor(SlimeColor selectedOrb)
    {
        if (!CanUseColor(selectedOrb))
            return;

        NextColor = selectedOrb;
        m_Projection.SetLineMaterial(Orbs[selectedOrb].Material);
        m_SphereCollider.material = Orbs[selectedOrb].PhysicMaterial;
        if (!Grounded)
        {
            CurrentColor = selectedOrb;
            m_SkinnedMeshRenderer.material = Orbs[selectedOrb].Material;
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

    public bool CanUseColor(SlimeColor color)
    {
        return Orbs[color].Amount > 0;
    }

    public void UseColor()
    {
        if (CanUseColor(NextColor))
        {
            Orbs[NextColor].Amount--;
            ForceChangeColor(NextColor);
        }
        else
        {
            Debug.LogError("Can't use color");
        }
    }

    public void AutoSetNextColor()
    {
        if (Orbs[NextColor].Amount == 0)
        {
            //sort orbs by amount
            var orbs = new List<SlimeOrb>(Orbs.Values);
            if (orbs.All(x => x.Amount <= 0))
            {
                Debug.Log("Game Over");
                GameOver = true;
            }

            orbs.Sort((x, y) => x.Amount.CompareTo(y.Amount));
            NextColor = orbs[^1].slimeColor;
        }

        ChangeColor(NextColor);
    }
}
