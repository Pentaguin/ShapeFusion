using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FusionableShape))] 
public class DraggableShape : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private Plane dragPlane;
    private GrabVisualizer grabVisualizer;

    void Start()
    {
        mainCamera = Camera.main;
        grabVisualizer = FindAnyObjectByType<GrabVisualizer>(); 
    }

    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    isDragging = true;
                    dragPlane = new Plane(Vector3.up, transform.position);
                    dragPlane.Raycast(ray, out float enter);
                    offset = transform.position - ray.GetPoint(enter);

                    if (grabVisualizer != null)
                        grabVisualizer.SetTarget(transform);
                }
            }
        }

        if (mouse.leftButton.isPressed && isDragging)
        {
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f)); 
            if (dragPlane.Raycast(ray, out float enter))
            {
                transform.position = ray.GetPoint(enter) + offset;
            }
        }

        if (mouse.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            if (grabVisualizer != null)
                grabVisualizer.ClearTarget();

            var fusionableShape = GetComponent<FusionableShape>();
            if (fusionableShape != null)
            {
                fusionableShape.TryFusion();
            }
        }
    }
}