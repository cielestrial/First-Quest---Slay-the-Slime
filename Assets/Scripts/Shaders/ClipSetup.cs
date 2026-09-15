using UnityEngine;

public class ClipSetup : MonoBehaviour
{
  // [SerializeField] private Renderer groundRenderer;
  // [SerializeField] private GameObject rangeRoot; // parent whose children get clipped

  public static void SetupClipBounds(Renderer groundRenderer, GameObject rangeParent)
  {
    Bounds groundBounds = groundRenderer.bounds;

    Vector2 minBounds = new(groundBounds.min.x, groundBounds.min.z);
    Vector2 maxBounds = new(groundBounds.max.x, groundBounds.max.z);

    ApplyBoundsToChildren(rangeParent, minBounds, maxBounds);
  }

  static void ApplyBoundsToChildren(GameObject root, Vector2 min, Vector2 max)
  {
    Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
    foreach (Renderer r in renderers)
    {
      r.material.SetVector("_GroundMin", min);
      r.material.SetVector("_GroundMax", max);
    }
  }
}
