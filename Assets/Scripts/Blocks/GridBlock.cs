using UnityEngine;

public class GridBlock : BuildBlock
{
    public Material gridMaterial;
    public float scaleRatio = 1;
    public float width = 0.01f;
    public float height = 0.01f;

    Vector2 textureScale = Vector2.one;

    void Start()
    {
        gridMaterial.SetFloat("_Width", width);
        gridMaterial.SetFloat("_Height", height);
    }

    void Update()
    {
        textureScale.x = transform.localScale.x / scaleRatio;
        textureScale.y = transform.localScale.z / scaleRatio;
        gridMaterial.SetTextureScale("_MainTex", textureScale);
    }

    public override bool GetBuildingPosition(RaycastHit hit, out Vector3 buildingPosition)
    {
        buildingPosition = Vector3.zero;

        if (hit.transform != transform)
        {
            return false;
        }

        buildingPosition.x = Mathf.Floor(hit.point.x);
        buildingPosition.z = Mathf.Floor(hit.point.z);

        return true;
    }
}
