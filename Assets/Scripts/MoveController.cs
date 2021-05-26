using UnityEngine;
using Valve.VR;

public class MoveController : MonoBehaviour
{
    public SteamVR_Behaviour_Pose behaviour_Pose;
    public SteamVR_Action_Boolean grabGripAction;
    public Transform cameraRig;
    public float speed = 1;
    public Transform aabbMin;
    public Transform aabbMax;

    SteamVR_Input_Sources handType;
    Transform handTransform;
    Vector3 _aabbMin;
    Vector3 _aabbMax;

    void Start()
    {
        handType = behaviour_Pose.inputSource;
        handTransform = behaviour_Pose.transform;

        var aabbMinPos = aabbMin.position;
        var aabbMaxPos = aabbMax.position;
        _aabbMin = new Vector3(Mathf.Min(aabbMinPos.x, aabbMaxPos.x), Mathf.Min(aabbMinPos.y, aabbMaxPos.y), Mathf.Min(aabbMinPos.z, aabbMaxPos.z));
        _aabbMax = new Vector3(Mathf.Max(aabbMinPos.x, aabbMaxPos.x), Mathf.Max(aabbMinPos.y, aabbMaxPos.y), Mathf.Max(aabbMinPos.z, aabbMaxPos.z));
    }

    void Update()
    {
        if (grabGripAction.GetState(handType))
        {
            var cameraRigDelta = handTransform.forward * speed;
            var cameraRigPosition = cameraRig.position;

            cameraRigPosition.x = Mathf.Clamp(cameraRigPosition.x + cameraRigDelta.x, _aabbMin.x, _aabbMax.x);
            cameraRigPosition.y = Mathf.Clamp(cameraRigPosition.y + cameraRigDelta.y, _aabbMin.y, _aabbMax.y);
            cameraRigPosition.z = Mathf.Clamp(cameraRigPosition.z + cameraRigDelta.z, _aabbMin.z, _aabbMax.z);

            cameraRig.position = cameraRigPosition;
        }
    }
}
