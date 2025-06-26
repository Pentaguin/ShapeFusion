using System.Collections.Generic;
using UnityEngine;

public class FusionManager : MonoBehaviour
{
    public static FusionManager Instance { get; private set; }
    public Transform shapesPool;

    public List<ShapeData> shapeDatas; 

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject GetPrefabByName(string name)
    {
        foreach (var shapeData in shapeDatas)
        {
            if (shapeData.prefab.name == name)
                return shapeData.prefab;
        }
        return null;
    }
}
