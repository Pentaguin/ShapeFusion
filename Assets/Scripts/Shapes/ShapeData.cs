using UnityEngine;

[CreateAssetMenu(menuName = "Shapes/ShapeData")]
public class ShapeData : ScriptableObject
{
    public string shapeName;
    public GameObject prefab;
}