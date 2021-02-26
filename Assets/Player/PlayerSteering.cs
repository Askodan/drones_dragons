using UnityEngine;
using System.Collections;
abstract public class PlayerSteering : MonoBehaviour
{
    public LayerMask mask;
    protected PlayerSpecs specs;
    [HideInInspector] public SteeringDrone steeringDrone;

    [HideInInspector] public RobotLegsWheelsSteering robotWheelsSteering;
    [HideInInspector] public RobotGrabber robotGrabber;

    [HideInInspector] public SteeringTractor tractorScriptEasy;
    [HideInInspector] public TrailerTrolley trailerTrolley;

    [HideInInspector] public SimpleCamera simpleCamera;
    [HideInInspector] public SimpleSmooth simpleSmooth;
    [HideInInspector] public RobotSimpleSmoothHelper smoothHelper;
    void Awake()
    {
        specs = GetComponent<PlayerSpecs>();
    }
    void Start()
    {
        SteeringStart();
    }
    void Update()
    {
        SteeringUpdate();
    }
    void LateUpdate()
    {
        SteeringLateUpdate();
    }

    abstract protected void SteeringStart();
    abstract protected void SteeringUpdate();
    abstract protected void SteeringLateUpdate();
}
