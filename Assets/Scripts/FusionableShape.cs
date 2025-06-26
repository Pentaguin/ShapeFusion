using UnityEngine;

public class FusionableShape : MonoBehaviour
{
    public ShapeData shapeData;
    public float fusionRadius = 1f;

    public void TryFusion()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, fusionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == gameObject) continue;

            FusionableShape otherShape = hitCollider.GetComponent<FusionableShape>();
            if (otherShape != null)
            {
                if (shapeData != null && otherShape.shapeData != null)
                {
                    if (shapeData.name == otherShape.shapeData.name)
                    {
                        FuseWith(otherShape);
                        break;
                    }
                }
            }
        }
    }

    void FuseWith(FusionableShape otherShape)
    {
        Vector3 fusionPosition = (transform.position + otherShape.transform.position) / 2;

        if (!int.TryParse(shapeData.name, out int shapeNumber))
        {
            Debug.LogError("Shape name is not a valid number: " + shapeData.name);
            return;
        }
        shapeNumber += 1;

        int currentLevel = GameManager.Instance.currentLevel;

        if(shapeNumber > currentLevel)
        {
            GameManager.Instance.LevelUp();
        }

        GameObject evolvedPrefab = FusionManager.Instance.GetPrefabByName(shapeNumber.ToString());
        if (evolvedPrefab != null)
        {
            Destroy(otherShape.gameObject);
            Destroy(gameObject);
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            GameObject newShape = Instantiate(
                evolvedPrefab,
                fusionPosition,
                randomRotation,
                FusionManager.Instance.shapesPool
            );
        }
        else
        {
            Debug.LogWarning("Evolved prefab for " + shapeNumber + " not found!");
        }
    }
}