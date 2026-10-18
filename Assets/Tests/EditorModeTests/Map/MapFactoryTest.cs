using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class MapFactoryTest
{
  private GameObject go;
  private GameObject cubeCellPrefab;
  private MapFactory mapFactory;
  private GridManager gridManager;

  [SetUp]
  public void SetUp()
  {
    gridManager = ScriptableObject.CreateInstance<GridManager>();
    gridManager.CreateGrid();

    go = new GameObject("MapFactory");
    mapFactory = go.AddComponent<MapFactory>();

    cubeCellPrefab = new GameObject("cubeCellPrefab");
    cubeCellPrefab.transform.SetParent(go.transform);
    cubeCellPrefab.AddComponent<BoxCollider>();
    FieldInfo field = typeof(MapFactory).GetField("CubeCellPrefab", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.IsNotNull(field, "MapFactory.CubeCellPrefab not found; was it renamed?");
    field.SetValue(mapFactory, cubeCellPrefab);

    mapFactory.SetUp(gridManager);
  }

  [TearDown]
  public void TearDown()
  {
    // not Destroy(): it doesn't work in EditMode
    Object.DestroyImmediate(go);
    Object.DestroyImmediate(gridManager);
  }

  [Test]
  public void CellColliderAdjusted()
  {
    GameObject spawnedCell = Object.Instantiate(cubeCellPrefab, go.transform);
    BoxCollider collider = spawnedCell.GetComponent<BoxCollider>();

    Assert.That(collider.size.x, Is.EqualTo(1 + GridManager.SpacingX / 2).Within(0.0001f));
    Assert.That(collider.size.y, Is.EqualTo(1).Within(0.0001f));
    Assert.That(collider.size.z, Is.EqualTo(1 + GridManager.SpacingZ / 2).Within(0.0001f));
    Assert.That(collider.center.x, Is.EqualTo(GridManager.SpacingX / 4).Within(0.0001f));
    Assert.That(collider.center.y, Is.EqualTo(0).Within(0.0001f));
    Assert.That(collider.center.z, Is.EqualTo(GridManager.SpacingZ / 4).Within(0.0001f));
  }

  [Test]
  public void CellsAreRandomized()
  {
    Random.State randomState = Random.state;
    try
    {
      Random.InitState(12345);
      AssertArrayIsRandomized(gridManager.GetLocationArrayPhase1());
      AssertArrayIsRandomized(gridManager.GetLocationArrayPhase2());
      AssertArrayIsRandomized(gridManager.GetLocationArrayPhase3());
    }
    finally
    {
      Random.state = randomState;
    }
  }

  private static void AssertArrayIsRandomized(int[] initialArray)
  {
    int[] shuffledArray = MapFactory.ShuffleArray((int[])initialArray.Clone());

    CollectionAssert.AreEquivalent(initialArray, shuffledArray);
    CollectionAssert.AreNotEqual(initialArray, shuffledArray);
  }
}
