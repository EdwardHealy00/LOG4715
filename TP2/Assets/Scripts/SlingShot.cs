using UnityEngine;
using System.Collections;
using System;

public enum SlingshotState
{
    Idle,
    UserPulling,
    BirdFlying
}
public class SlingShot : MonoBehaviour
{
    //a vector that points in the middle between left and right parts of the slingshot

    [HideInInspector]
    public SlingshotState slingshotState;


    //this linerenderer will draw the projected trajectory of the thrown bird
    //public LineRenderer TrajectoryLineRenderer;


    private Vector3 m_LastLocation;

    public float ThrowSpeed;

    [HideInInspector]
    public float TimeSinceThrown;

    // Use this for initialization
    void Start()
    {
        slingshotState = SlingshotState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (slingshotState)
        {
            case SlingshotState.Idle:
                //fix bird's position
                //display the slingshot "strings"
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit raycastHit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out raycastHit, 100f))
                    {
                        if (raycastHit.transform != null)
                        {
                            slingshotState = SlingshotState.UserPulling;
                            Debug.Log("pulling");
                        }
                    }
                }
                break;
            case SlingshotState.UserPulling:

                if (Input.GetMouseButton(0))
                {
                    //get where user is tapping
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //we will let the user pull the bird up to a maximum distance
                    if (Vector3.Distance(location, transform.position) > 1.5f)
                    {
                        //basic vector maths :)
                        var maxPosition = (location - transform.position).normalized * 1.5f + transform.position;
                        m_LastLocation = maxPosition;
                    }
                    else
                    {
                        m_LastLocation = location;
                    }
                    float distance = Vector3.Distance(transform.position, m_LastLocation);
                    //display projected trajectory based on the distance
                    //DisplayTrajectoryLineRenderer2(distance);
                }
                else//user has removed the tap 
                {
                    //SetTrajectoryLineRenderesActive(false);
                    //throw the bird!!!
                    TimeSinceThrown = Time.time;
                    Debug.Log(m_LastLocation);
                    float distance = Vector3.Distance(transform.position, m_LastLocation);
                    if (distance > 1)
                    {
                        slingshotState = SlingshotState.BirdFlying;
                        ThrowBird(distance);
                    }
                    else//not pulled long enough, so reinitiate it
                    {
                        //distance/10 was found with trial and error :)
                        //animate the bird to the wait position
                        //transform.positionTo(distance / 10, //duration
                        //    BirdWaitPosition.transform.position). //final position
                        //    setOnCompleteHandler((x) =>
                        //    {
                        //        x.complete();
                        //        x.destroy();
                        //        
                        //    });
                        slingshotState = SlingshotState.Idle;
                    }
                }
                break;
            case SlingshotState.BirdFlying:
                break;
            default:
                break;
        }

    }

    private void ThrowBird(float distance)
    {
        //get velocity
        Vector3 velocity = transform.position - transform.position;
        //old and alternative way
        //GetComponent<Rigidbody2D>().AddForce
        //    (new Vector2(v2.x, v2.y) * ThrowSpeed * distance * 300 * Time.deltaTime);
        //set the velocity
        GetComponent<Rigidbody>().velocity = new Vector2(velocity.x, velocity.y) * ThrowSpeed * distance;


        //notify interested parties that the bird was thrown
        if (BirdThrown != null)
            BirdThrown(this, EventArgs.Empty);
    }

    public event EventHandler BirdThrown;

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
    //    Vector2 segVelocity = new Vector2(v2.x, v2.y) * ThrowSpeed * distance;

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
