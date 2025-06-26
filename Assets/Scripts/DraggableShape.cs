using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FusionableShape))] 
public class DraggableShape : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private Plane dragPlane;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(mouse.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    isDragging = true;
                    dragPlane = new Plane(Vector3.up, transform.position);
                    dragPlane.Raycast(ray, out float enter);
                    offset = transform.position - ray.GetPoint(enter);
                }
            }
        }

        if (mouse.leftButton.isPressed && isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(mouse.position.ReadValue());
            if (dragPlane.Raycast(ray, out float enter))
            {
                transform.position = ray.GetPoint(enter) + offset;
            }
        }

        if (mouse.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            // Notify the fusion script that dragging ended
            var fusionableShape = GetComponent<FusionableShape>();
            if (fusionableShape != null)
            {
                fusionableShape.TryFusion();
            }
        }
    }
}