using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BuildController : MonoBehaviour
{
    public SteamVR_Behaviour_Pose behaviour_Pose;
    public SteamVR_Action_Boolean interactUIAction;
    public Laser laser;
    public Transform ghost;
    public GameObject cubePrefab;

    SteamVR_Input_Sources handType;
    CubeBlock lastCubeBlock;

    void Start()
    {
        handType = behaviour_Pose.inputSource;
        laser.OnHit += OnHit;
    }

    void Update()
    {
        if (interactUIAction.GetState(handType))
        {
            if ((lastCubeBlock == null) || lastCubeBlock.isSpawnCompleted)
            {
                lastCubeBlock = Instantiate(cubePrefab).GetComponent<CubeBlock>();
                lastCubeBlock.transform.position = ghost.position;
            }
        }
        else
        {
            lastCubeBlock = null;
        }
    }

    void OnHit(RaycastHit hit)
    {
        var buildBlock = hit.transform.GetComponent<BuildBlock>();
        if (buildBlock == null)
        {
            return;
        }

        Vector3 buildingPosition;
        if (buildBlock.GetBuildingPosition(hit, out buildingPosition))
        {
            ghost.position = buildingPosition;
        }
    }
}
