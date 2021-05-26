using UnityEngine;

public abstract class BuildBlock : MonoBehaviour
{
    public abstract bool GetBuildingPosition(RaycastHit hit, out Vector3 buildingPosition);
}
