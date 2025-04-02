using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWrap : MonoBehaviour
{
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        WrapObject();
    }
    void WrapObject()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null) return; // Exit if no Collider2D is attached

        Vector3 screenPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Get half the size of the object based on its collider bounds
        float halfWidth = collider.bounds.extents.x / 2 / mainCamera.orthographicSize;
        float halfHeight = collider.bounds.extents.y / 2 / mainCamera.orthographicSize;

        // Account for collider size when checking screen wrapping
        if (screenPosition.x > 1 + halfWidth) screenPosition.x = -halfWidth;
        if (screenPosition.x < 0 - halfWidth) screenPosition.x = 1 + halfWidth;
        if (screenPosition.y > 1 + halfHeight) screenPosition.y = -halfHeight;
        if (screenPosition.y < 0 - halfHeight) screenPosition.y = 1 + halfHeight;

        transform.position = mainCamera.ViewportToWorldPoint(screenPosition);
    }
}
