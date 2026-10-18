

using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace EventSysTest.Events
{
  public class TargetEventTest
  {
    private TargetingSystem targetingSystem;
    private Camera mainCamera;
    const float CameraOffset = 25.2f - 9.8f;
    class FakeFocusable : IFocusable
    {
      public Vector3 FocusPosition { get; set; }
      public Vector2Int GridPosition { get; set; }
    }

    [SetUp]
    public void SetUp()
    {
      mainCamera = new GameObject("Main Camera").AddComponent<Camera>();
      mainCamera.tag = "MainCamera";

      targetingSystem = ScriptableObject.CreateInstance<TargetingSystem>();
      targetingSystem.SetUp(mainCamera, EventBusTest.SharedEventBus);
    }

    [TearDown]
    public void TearDown()
    {
      targetingSystem.TearDown();
      // not Destroy(): it doesn't work in EditMode
      Object.DestroyImmediate(targetingSystem);
      Object.DestroyImmediate(mainCamera);
    }

    [Test]
    public void CameraFocused()
    {
      Random.State randomState = Random.state;
      try
      {
        Random.InitState(12345);
        FakeFocusable target = new() { FocusPosition = RandomPosition() };
        mainCamera.transform.position = RandomPosition();


        Vector3 expected = new(
        target.FocusPosition.x,
        mainCamera.transform.position.y,
        target.FocusPosition.z - CameraOffset
      );

        EventBusTest.SharedEventBus.MyTargetEvent.FocusCameraOn(target);
        Assert.That(mainCamera.transform.position, Is.EqualTo(expected).Using(new Vector3EqualityComparer(0.001f)));
      }
      finally
      {
        Random.state = randomState;
      }
    }

    private Vector3 RandomPosition() => new(
      Random.Range(-100f, 100f),
      Random.Range(-100f, 100f),
      Random.Range(-100f, 100f)
    );
  }
}