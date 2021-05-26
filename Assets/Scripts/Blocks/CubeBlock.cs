using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBlock : BuildBlock
{
    public Tween spawnTween;

    public bool isSpawnCompleted { get; private set; } = false;

    bool isSpawnStarted = false;

    void Update()
    {
        if (!isSpawnStarted && spawnTween != null)
        {
            isSpawnStarted = true;

            spawnTween.onComplete += onSpawnComplete;
            spawnTween.onUpdate += onSpawnUpdate;
            spawnTween.Play();
        }
    }

    public override bool GetBuildingPosition(RaycastHit hit, out Vector3 buildingPosition)
    {
        buildingPosition = Vector3.zero;

        if (hit.transform != transform)
        {
            return false;
        }

        buildingPosition.x = Mathf.Floor(hit.point.x + hit.normal.x / 2);
        buildingPosition.y = Mathf.Floor(hit.point.y + hit.normal.y / 2);
        buildingPosition.z = Mathf.Floor(hit.point.z + hit.normal.z / 2);

        return true;
    }

    void onSpawnComplete()
    {
        isSpawnCompleted = true;
    }

    void onSpawnUpdate(float[] values)
    {
        spawnTween.transform.localScale = Vector3.one * values[0];
    }
}
