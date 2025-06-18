using UnityEngine;

[ExecuteAlways]
public class QuadFollower2D : MonoBehaviour
{
    [Tooltip("Which camera to follow/scale. If left empty, will use Camera.main.")]
    public Camera cam;

    [Tooltip("How big should the quad be relative to the screen?")]
    [Range(0.0f, 1.0f)]
    public Vector2 screenSizeFraction = new Vector2(1.0f, 1.0f);
    // (0.5, 0.5) → covers half the screen width & half the screen height

    void LateUpdate()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        // 1) Follow camera (keep your own Z)
        Vector3 cpos = cam.transform.position;
        transform.position = new Vector3(cpos.x, cpos.y, transform.position.z);

        // 2) Compute how many world‑units tall/wide the screen is
        float screenWorldHeight = cam.orthographicSize * 2.0f;
        float screenWorldWidth  = screenWorldHeight * cam.aspect;

        // 3) Set your quad's scale so it fills that fraction of the screen
        float quadWidth  = screenWorldWidth  * screenSizeFraction.x;
        float quadHeight = screenWorldHeight * screenSizeFraction.y;

        // If your quad mesh is 1 unit × 1 unit, this works directly:
        transform.localScale = new Vector3(quadWidth * 2, quadHeight * 2, 1f);
    }
}
