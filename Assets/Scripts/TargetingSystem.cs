using UnityEngine;
using UnityEngine.InputSystem;

public class TargetingSystem : MonoBehaviour
{
  // [SerializeField] private GridManager MyGridManager;
  [SerializeField] private int MaxRaycastDistance;
  [SerializeField] private LayerMask GridLayerMask;
  [SerializeField] private LayerMask RangeLayerMask;
  private Camera mainCamera;
  private Ray rayOrigin;
  private RaycastHit hitInfo;
  private Vector2 mousePos;

  void Awake()
  {
    mainCamera = Camera.main;
  }

  public Vector3? GetTargetLocation()
  {
    mousePos = Mouse.current.position.ReadValue();
    rayOrigin = mainCamera.ScreenPointToRay(mousePos);
    if (Physics.Raycast(rayOrigin, out hitInfo, MaxRaycastDistance, GridLayerMask))
    {
      Debug.Log($"world pos: {hitInfo.transform.position}");
      return ValidateTargetLocation(hitInfo.transform.position);
    }
    else return null;
  }

  Vector3? ValidateTargetLocation(Vector3 clickLocation)
  {
    if (Physics.Raycast(clickLocation, Vector3.up, out hitInfo, 1, RangeLayerMask))
    {
      Debug.Log($"within range");
      return clickLocation;
    }
    return null;
  }

}
