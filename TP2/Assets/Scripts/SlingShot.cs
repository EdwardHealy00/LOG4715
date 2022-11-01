using UnityEngine;
using System.Collections;
using System;

public enum SlingshotState
{
    Idle,
    UserPulling,
    SlimeFlying
}
public class SlingShot : MonoBehaviour
{
    private SlingshotState m_SlingshotState;


    //this linerenderer will draw the projected trajectory of the thrown slime
    //public LineRenderer TrajectoryLineRenderer;


    private Vector3 m_LastLocation;

    private const float k_MaxPullDistance = 2f;

    public float m_ThrowSpeed;


    // Use this for initialization
    void Start()
    {
        m_SlingshotState = SlingshotState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_SlingshotState)
        {
            case SlingshotState.Idle:
                //fix slime's position
                //display the slingshot "strings"
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit raycastHit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out raycastHit, 100f))
                    {
                        if (raycastHit.transform == transform)
                        {
                            m_SlingshotState = SlingshotState.UserPulling;
                            Debug.Log("pulling");
                        }
                    }
                }
                break;
            case SlingshotState.UserPulling:

                if (Input.GetMouseButton(0))
                {
                    var location = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z)) * -1;
                    location.z = 0;
                    location.y += Camera.main.transform.position.y;

                    if (Vector3.Distance(location, transform.position) > k_MaxPullDistance)
                    {
                        var maxPosition = (location - transform.position).normalized * k_MaxPullDistance + transform.position;
                        m_LastLocation = maxPosition;
                    }
                    else
                    {
                        m_LastLocation = location;
                    }
                    float distance = Vector3.Distance(transform.position, m_LastLocation);
                    Debug.Log($"d:{distance}, l:{m_LastLocation}");
                    //DisplayTrajectoryLineRenderer2(distance);
                }
                else
                {
                    //SetTrajectoryLineRenderesActive(false);
                    float distance = Vector3.Distance(transform.position, m_LastLocation);
                    Debug.Log(distance);
                    if (distance > 1)
                    {
                        m_SlingshotState = SlingshotState.SlimeFlying;
                        ThrowSlime(distance);
                    }

                    m_SlingshotState = SlingshotState.Idle;
                }
                break;
            case SlingshotState.SlimeFlying:
                break;
            default:
                break;
        }

    }

    private void ThrowSlime(float distance)
    {
        Debug.Log("throwing");
        //get velocity
        Vector3 velocity = m_LastLocation - transform.position;
        velocity.Normalize();
        
        Debug.Log(-velocity * m_ThrowSpeed * distance);
        //set the velocity
        GetComponent<Rigidbody>().AddForce(-velocity * m_ThrowSpeed * distance, ForceMode.Impulse);

        //notify interested parties that the slime was thrown
        if (SlimeThrown != null)
            SlimeThrown(this, EventArgs.Empty);
    }

    public event EventHandler SlimeThrown;

    //void SetTrajectoryLineRenderesActive(bool active)
    //{
    //    TrajectoryLineRenderer.enabled = active;
    //}


    /// <summary>
    /// Another solution (a great one) can be found here
    /// http://wiki.unity3d.com/index.php?title=Trajectory_Simulation
    /// </summary>
    /// <param name="distance"></param>
    //void DisplayTrajectoryLineRenderer2(float distance)
    //{
    //    SetTrajectoryLineRenderesActive(true);
    //    Vector3 v2 = transform.position - transform.position;
    //    int segmentCount = 15;
    //    float segmentScale = 2;
    //    Vector2[] segments = new Vector2[segmentCount];

    //    // The first line point is wherever the player's cannon, etc is
    //    segments[0] = transform.position;

    //    // The initial velocity
    //    Vector2 segVelocity = new Vector2(v2.x, v2.y) * m_ThrowSpeed * distance;

    //    float angle = Vector2.Angle(segVelocity, new Vector2(1, 0));
    //    float time = segmentScale / segVelocity.magnitude;
    //    for (int i = 1; i < segmentCount; i++)
    //    {
    //        //x axis: spaceX = initialSpaceX + velocityX * time
    //        //y axis: spaceY = initialSpaceY + velocityY * time + 1/2 * accelerationY * time ^ 2
    //        //both (vector) space = initialSpace + velocity * time + 1/2 * acceleration * time ^ 2
    //        float time2 = i * Time.fixedDeltaTime * 5;
    //        segments[i] = segments[0] + segVelocity * time2 + 0.5f * Physics2D.gravity * Mathf.Pow(time2, 2);
    //    }

    //    TrajectoryLineRenderer.SetVertexCount(segmentCount);
    //    for (int i = 0; i < segmentCount; i++)
    //        TrajectoryLineRenderer.SetPosition(i, segments[i]);
    //}
}
