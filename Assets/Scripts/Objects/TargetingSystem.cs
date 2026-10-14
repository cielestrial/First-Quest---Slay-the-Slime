using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "TargetingSystem", menuName = "Scriptable Objects/TargetingSystem")]
public class TargetingSystem : ScriptableObject
{
  [SerializeField] private int MaxRaycastDistance = 50;
  [SerializeField] private LayerMask GridLayerMask;
  [SerializeField] private LayerMask RangeLayerMask;
  private Camera mainCamera = null;
  private Ray rayOrigin;
  private RaycastHit hitInfo;
  private Vector2 mousePos;

  public void SetCamera()
  {
    mainCamera = Camera.main;
  }

  public Vector3? GetClickLocation()
  {
    if (mainCamera == null)
    {
      SetCamera();
      if (mainCamera == null)
        return null;
    }
    mousePos = Mouse.current.position.ReadValue();
    rayOrigin = mainCamera.ScreenPointToRay(mousePos);
    if (Physics.Raycast(rayOrigin, out hitInfo, MaxRaycastDistance, GridLayerMask))
    {
      Debug.Log($"world pos: {hitInfo.transform.position}");
      return hitInfo.transform.position;
    }
    return null;
  }

  public bool IsClickWithinRange(Vector3 targetLocation)
  {
    if (Physics.Raycast(targetLocation, Vector3.up, out hitInfo, 1, RangeLayerMask))
      return true;
    return false;
  }

  public Transform GetTargetRange(Vector3 clickLocation)
  {
    if (Physics.Raycast(clickLocation, Vector3.up, out hitInfo, 1, RangeLayerMask))
    {
      Debug.Log(hitInfo.transform.tag);
      return hitInfo.transform;
    }
    return null;
  }

}
