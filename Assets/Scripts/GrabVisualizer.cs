using UnityEngine;

public class GrabVisualizer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform handOrigin;

    private Transform currentTarget;

    void Update()
    {
        if (currentTarget != null)
        {
            lineRenderer.SetPosition(0, handOrigin.position);    
            lineRenderer.SetPosition(1, currentTarget.position);  
        }
    }

    public void SetTarget(Transform target)
    {
        currentTarget = target;
        lineRenderer.enabled = true;
    }

    public void ClearTarget()
    {
        currentTarget = null;
        lineRenderer.enabled = false;
    }
}
