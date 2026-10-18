using UnityEngine;
using UnityEngine.InputSystem;

public interface IFocusable
{
  Vector3 FocusPosition { get; }
  Vector2Int GridPosition { get; }
}

[CreateAssetMenu(fileName = "TargetingSystem", menuName = "Scriptable Objects/TargetingSystem")]
public class TargetingSystem : ScriptableObject
{
  [SerializeField] private int MaxRaycastDistance = 50;
  [SerializeField] private LayerMask GridLayerMask;
  [SerializeField] private LayerMask RangeLayerMask;
  private Camera mainCamera = null;
  private EventBus eventBus;
  private Ray rayOrigin;
  private RaycastHit hitInfo;
  private Vector2 mousePos;
  const float CameraOffset = 25.2f - 9.8f;

  public void SetUp(Camera mainCamera, EventBus eventBus)
  {
    this.mainCamera = mainCamera;
    this.eventBus = eventBus;
    AddListener();
  }
  public void TearDown()
  {
    RemoveListener();
    eventBus = null;
    mainCamera = null;
  }
  void AddListener()
  {
    eventBus.MyTargetEvent?.AddListener(this);
  }
  void RemoveListener()
  {
    if (eventBus != null)
      eventBus.MyTargetEvent?.RemoveListener(this);
  }

  private bool HasCamera()
  {
    if (mainCamera == null)
    {
      Debug.Log("No camera");
      return false;
    }
    return true;
  }
  public Vector3? GetClickLocation()
  {
    if (!HasCamera()) return null;
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

  public void FocusCameraOnTarget(IFocusable target)
  {
    if (!HasCamera()) return;
    mainCamera.transform.position = new Vector3(
      target.FocusPosition.x,
      mainCamera.transform.position.y,
      target.FocusPosition.z - CameraOffset
    );
  }

}
