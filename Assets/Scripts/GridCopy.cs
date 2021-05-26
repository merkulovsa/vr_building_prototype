using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GridCopy : MonoBehaviour
{
    public SteamVR_Behaviour_Pose behaviour_Pose;
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Action_Vector2 trackpadPosAction;
    public float minDistance = 2;
    public float maxDistance = 20;
    public int minSize = 1;
    public int maxSize = 3;
    public Transform offsetTransform;
    public Transform gridTransform;
    public Material gridMaterial;
    public float gridScaleRatio = 1;
    public BuildController buildController;

    SteamVR_Input_Sources handType;
    Vector2 axis = Vector2.zero;
    Vector2 textureScale = Vector2.zero;
    Vector3 discretePosition = Vector3.zero;

    void Start()
    {
        handType = behaviour_Pose.inputSource;
    }

    void Update()
    {
        UpdateDiscretePosition();
        offsetTransform.position = discretePosition;

        if (teleportAction.GetState(handType)) 
        {
            var delta = trackpadPosAction.GetAxisDelta(handType);

            if (delta.magnitude > 1e-2) {
                axis.x = Mathf.Clamp(axis.x + delta.x, -1, 1);
                axis.y = Mathf.Clamp(axis.y + delta.y, -1, 1);

                UpdatePosition((axis.y + 1) / 2);
                UpdateSize((axis.x + 1) / 2);
            }
        }

        if (teleportAction.GetStateUp(handType)) 
        {
            UpdatePrefab();
            gridTransform.gameObject.SetActive(false);
        } 
        else if (teleportAction.GetStateDown(handType)) 
        {
            gridTransform.gameObject.SetActive(true);
        }
    }

    void UpdateDiscretePosition() {
        var position = transform.position;
        discretePosition.x = Mathf.Floor(position.x);
        discretePosition.y = Mathf.Clamp(Mathf.Floor(position.y), 0, Mathf.Infinity);
        discretePosition.z = Mathf.Floor(position.z);
    }

    void UpdatePosition(float k) {
        var localPosition = transform.localPosition;
        localPosition.z = Mathf.Lerp(minDistance, maxDistance, k);
        transform.localPosition = localPosition;
    }

    void UpdateSize(float k) {
        gridTransform.localScale = Vector3.one * Mathf.Round(Mathf.Lerp(minSize, maxSize, k));
        gridTransform.localPosition = gridTransform.localScale * 0.5f;

        textureScale.x = gridTransform.localScale.x / gridScaleRatio;
        textureScale.y = gridTransform.localScale.z / gridScaleRatio;
        gridMaterial.SetTextureScale("_MainTex", textureScale);
    }

    void UpdatePrefab() {
        var prefab = Instantiate(buildController.cubePrefab);
        var parent = prefab.transform.GetChild(0);
        var gridRenderer = gridTransform.GetComponent<MeshRenderer>();
        var cubeRenderers = buildController.cubeRenderers;
        bool isPrefabClear = false;
        
        foreach (var renderer in cubeRenderers) {
            if (gridRenderer.bounds.Intersects(renderer.bounds)) {
                if (!isPrefabClear) {
                    for (int i = 0; i < parent.childCount; ++i) {
                        Destroy(parent.GetChild(i).gameObject);
                    }
                    isPrefabClear = true;
                    prefab.SetActive(false);
                    buildController.cubePrefab = prefab;
                }
                
                var cube = Instantiate(renderer.gameObject);
                cube.transform.parent = parent;

                var cubeLocalPosition = renderer.transform.position - offsetTransform.position;
                cubeLocalPosition.x *= 1 / gridTransform.localScale.x;
                cubeLocalPosition.y *= 1 / gridTransform.localScale.y;
                cubeLocalPosition.z *= 1 / gridTransform.localScale.z;
                cube.transform.localPosition = cubeLocalPosition;

                var cubeLocalScale = renderer.transform.localScale;
                cubeLocalScale.x *= 1 / gridTransform.localScale.x;
                cubeLocalScale.y *= 1 / gridTransform.localScale.y;
                cubeLocalScale.z *= 1 / gridTransform.localScale.z;
                cube.transform.localScale = cubeLocalScale;
            }
        }

        if (!isPrefabClear) {
            for (int i = 0; i < parent.childCount; ++i) {
                if (i == 0) {
                    parent.GetChild(i).localPosition = Vector3.one * 0.5f;
                    parent.GetChild(i).localScale = Vector3.one;
                } else {
                    Destroy(parent.GetChild(i).gameObject);
                }
            }
            prefab.SetActive(false);
            buildController.cubePrefab = prefab;
        }
    }
}
