using NUnit.Framework;
using UnityEngine;


namespace EventSysTest.Events
{
  [SetUpFixture]
  public class EventBusTest
  {
    public static EventBus SharedEventBus { get; private set; }

    [OneTimeSetUp]
    public virtual void CreateBus()
    {
      SharedEventBus = ScriptableObject.CreateInstance<EventBus>();
      SharedEventBus.SetUp();
    }

    [OneTimeTearDown]
    public virtual void DestroyBus()
    {
      SharedEventBus.TearDown();
      Object.DestroyImmediate(SharedEventBus);
      SharedEventBus = null;
    }
  }
}