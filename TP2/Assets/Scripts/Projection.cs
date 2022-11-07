using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Inspired by: https://github.com/Matthew-J-Spencer/Trajectory-Line-Unity 
public class Projection : MonoBehaviour
{
    public LineRenderer Line;
    [SerializeField] private int m_MaxPhysicsFrameIterations = 100;
    [SerializeField] private Transform _obstaclesParent;

    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    private readonly Dictionary<Transform, Transform> _spawnedObjects = new Dictionary<Transform, Transform>();

    private void Awake()
    {
        Line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        CreatePhysicsScene();
    }

    private void CreatePhysicsScene()
    {
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();

        foreach (Transform obj in _obstaclesParent)
        {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
            if (!ghostObj.isStatic) _spawnedObjects.Add(obj, ghostObj.transform);
        }
    }

    private void Update()
    {
        foreach (var item in _spawnedObjects)
        {
            item.Value.position = item.Key.position;
            item.Value.rotation = item.Key.rotation;
        }
    }

    public void SimulateTrajectory(GameObject ballPrefab, Vector3 pos, Vector3 velocity)
    {
        var ghostObj = Instantiate(ballPrefab, pos, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, _simulationScene);
       
        ghostObj.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);

        Line.positionCount = m_MaxPhysicsFrameIterations;

        for (var i = 0; i < m_MaxPhysicsFrameIterations; i++)
        {
            _physicsScene.Simulate(Time.fixedDeltaTime);
            Line.SetPosition(i, ghostObj.transform.position);
        }

        Destroy(ghostObj.gameObject);
    }
    
    public void EnableTrajectory(bool enable)
    {
        Line.positionCount = 0;
        Line.enabled = enable;
    }
}
