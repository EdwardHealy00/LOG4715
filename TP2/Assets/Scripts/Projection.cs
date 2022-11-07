using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Inspired by: https://github.com/Matthew-J-Spencer/Trajectory-Line-Unity 
public class Projection : MonoBehaviour
{
    private LineRenderer m_line;
    [Header("Trajectory Line Length")]
    [SerializeField] private uint m_MaxPhysicsFrameIterations = 30;

    [Header("Trajectory Line Width")]
    [SerializeField] private float m_MinWidth = 0.01f;
    [SerializeField] private float m_MaxWidth = 0.1f;

    [Header("Parent Object of All Obstacles")]
    [SerializeField] private Transform m_ObstaclesParent;

    [Header("GameObject Simulating the Trajectory")]
    [SerializeField] private GameObject m_ballPrefab;

    private Scene m_SimulationScene;
    private PhysicsScene m_PhysicsScene;
    private readonly Dictionary<Transform, Transform> m_SpawnedObjects = new Dictionary<Transform, Transform>();

    private void Awake()
    {
        m_line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        CreatePhysicsScene();
    }

    private void CreatePhysicsScene()
    {
        m_SimulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        m_PhysicsScene = m_SimulationScene.GetPhysicsScene();

        foreach (Transform obj in m_ObstaclesParent)
        {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, m_SimulationScene);
            if (!ghostObj.isStatic) m_SpawnedObjects.Add(obj, ghostObj.transform);
        }
    }

    private void Update()
    {
        foreach (var item in m_SpawnedObjects)
        {
            item.Value.SetPositionAndRotation(item.Key.position, item.Key.rotation);
        }
    }

    public void SimulateTrajectory(Vector3 pos, Vector3 velocity)
    {
        var ghostObj = Instantiate(m_ballPrefab, pos, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, m_SimulationScene);
       
        ghostObj.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);

        m_line.positionCount = (int)m_MaxPhysicsFrameIterations;

        for (var i = 0; i < m_MaxPhysicsFrameIterations; i++)
        {
            m_PhysicsScene.Simulate(Time.fixedDeltaTime);
            m_line.SetPosition(i, ghostObj.transform.position);
        }

        Destroy(ghostObj.gameObject);
    }
    
    public void EnableTrajectory(bool enable)
    {
        m_line.positionCount = 0;
        m_line.enabled = enable;
    }

    public void SetLineWidth(float width)
    {
        width = Mathf.Lerp(m_MinWidth, m_MaxWidth, width);
        m_line.startWidth = width;
        m_line.endWidth = width;
    }

    public void SetLineMaterial(Material material)
    {
        m_line.material = material;
    }

}
