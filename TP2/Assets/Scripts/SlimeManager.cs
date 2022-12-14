using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    
    [Header("Starting Orbs")]
    [SerializeField] private SlimeColor StartingColor = SlimeColor.Green;
    
    [SerializeField] private uint greenOrbsAtStart = 1;
    [SerializeField] private uint yellowOrbsAtStart = 1;
    [SerializeField] private uint blueOrbsAtStart = 1;
    [SerializeField] private uint orangeOrbsAtStart =1;
    [SerializeField] private uint pinkOrbsAtStart = 1;
    
    private float m_GroundedTimer = 0;

    void Awake()
    {
        Orbs = SlimeOrbsGenerator.GenerateOrbs(greenOrbsAtStart, yellowOrbsAtStart, blueOrbsAtStart, orangeOrbsAtStart, pinkOrbsAtStart);
        Rigidbody = GetComponent<Rigidbody>();
        BodyCenter = transform.Find("BodyCenter");
        m_LastCollisionPoint = transform.position;
        m_SkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        m_SphereCollider = GetComponent<SphereCollider>();
        m_Projection = GetComponent<Projection>();
        m_CursorManager = FindObjectOfType<CursorManager>();

        ForceChangeColor(StartingColor);
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
                if (!collisionInfo.transform.CompareTag("PinkWall") && !collisionInfo.transform.CompareTag("GreenWall") &&
                    SlingshotState == SlingshotState.Moving && (m_LastCollisionPoint - transform.position).magnitude > k_NonStickRadius)
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

    public void ChangeColor(SlimeColor selectedOrb, bool fromInventory)
    {
        if (!CanUseColor(selectedOrb))
            return;

        NextColor = selectedOrb;
        m_Projection.SetLineMaterial(Orbs[selectedOrb].Material);
        m_SphereCollider.material = Orbs[selectedOrb].PhysicMaterial;

        if (!Grounded)
        {
            if (fromInventory)
            {
                UseColor();
            }   
            
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
        // Check if inside of a wall specific trigger
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.3f);
        foreach (Collider collider in colliders)
        {
            if (collider.isTrigger)
            {
                if (selectedOrb == SlimeColor.Pink && (collider.transform.CompareTag("YellowWall") || collider.transform.CompareTag("GreenWall")))
                {
                    m_SphereCollider.material = Orbs[SlimeColor.Green].PhysicMaterial;
                    break;
                }
            }
        }
    }

    public bool CanUseColor(SlimeColor color)
    {
        return Orbs[color].Amount > 0;
    }

    public void UseColor()
    {
        if (CanUseColor(NextColor))
        {
            //Debug.Log("Consumed");
            Orbs[NextColor].Amount--;
            ForceChangeColor(NextColor);
        }
        else
        {
            //Debug.LogError("Can't use color");
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
                //Debug.Log("Game Over");
                GameOver = true;
                GameObject.Find("Gym Controls")?.transform.GetChild(0).gameObject.SetActive(true);
                return;
            }

            orbs.Sort((x, y) => x.Amount.CompareTo(y.Amount));
            ChangeColor(orbs[^1].slimeColor, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (CurrentColor)
        {
            case SlimeColor.Pink:
                if (other.transform.CompareTag("YellowWall") || other.transform.CompareTag("GreenWall"))
                {
                    m_SphereCollider.material = Orbs[SlimeColor.Green].PhysicMaterial;
                }
                break;

            case SlimeColor.Yellow:
                if (other.transform.CompareTag("PinkWall") || other.transform.CompareTag("GreenWall"))
                {
                    m_SphereCollider.material = Orbs[SlimeColor.Green].PhysicMaterial;
                }
                break;

            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (CurrentColor)
        {
            case SlimeColor.Pink:
                m_SphereCollider.material = Orbs[SlimeColor.Pink].PhysicMaterial;
                break;

            case SlimeColor.Yellow:
                m_SphereCollider.material = Orbs[SlimeColor.Yellow].PhysicMaterial;
                break;

            default:
                break;
        }
    }
}
