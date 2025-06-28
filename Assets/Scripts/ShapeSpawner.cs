using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    public ShapeData[] shapesToSpawn;
    public int maxShapes = 10;
    public float spawnInterval = 5f;
    public Transform shapesPool;

    private float nextSpawnTime = 0f; 
    private int currentShapeCount = 0; 

    void Start()
    {
        nextSpawnTime = Time.time; 
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && currentShapeCount < maxShapes)
        {
            SpawnShape();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnShape()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-20f, 20f),
            2f, 
            Random.Range(-20f, 20f)
        );

        // only spawn shapes up to current level
        int currentLevel = GameManager.Instance.currentLevel;

        int randomShapeIndex = Random.Range(0, currentLevel);
        Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f,360f), 0f);

        GameObject shape = Instantiate(
            shapesToSpawn[randomShapeIndex].prefab, 
            randomPosition, 
            randomRotation,
            shapesPool
        );
        currentShapeCount++; 
    }
}
