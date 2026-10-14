using UnityEngine;
using EventSys.Events;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "EventSystem", menuName = "Scriptable Objects/EventSystem")]
public class EventSystem : ScriptableObject
{
  public RangeEvent MyRangeEvent { get; private set; }
  public TurnEvent MyTurnEvent { get; private set; }
  public UIEvent MyUIEvent { get; private set; }

  [SerializeField] private List<Object> RangeEventListeners;
  [SerializeField] private List<Object> TurnEventListeners;
  [SerializeField] private List<Object> UIEventListeners;

  void OnEnable()
  {
    MyRangeEvent = new(1);
    RangeEventListeners = new(1);
    MyTurnEvent = new(1);
    TurnEventListeners = new(1);
    MyUIEvent = new(1);
    UIEventListeners = new(1);
  }

  public void Snapshot()
  {
    RangeEventListeners.Clear();
    foreach (RangeManager rm in MyRangeEvent.Listeners)
      RangeEventListeners.Add(rm.GameObject());
    TurnEventListeners.Clear();
    foreach (TurnManager tm in MyTurnEvent.Listeners)
      TurnEventListeners.Add(tm.GameObject());
    UIEventListeners.Clear();
    foreach (UIManager uim in MyUIEvent.Listeners)
      UIEventListeners.Add(uim);
  }

  void OnDisable()
  {
    MyRangeEvent.ClearListeners();
    MyRangeEvent = null;

    MyTurnEvent.ClearListeners();
    MyTurnEvent = null;

    MyUIEvent.ClearListeners();
    MyUIEvent = null;
  }

}

[CustomEditor(typeof(EventSystemEditor))]
public class EventSystemEditor : Editor
{
  SerializedProperty rangeListeners;
  SerializedProperty turnListeners;
  SerializedProperty uiListeners;


  private void OnEnable()
  {
    rangeListeners = serializedObject.FindProperty("RangeEvent");
    turnListeners = serializedObject.FindProperty("TurnEvent");
    uiListeners = serializedObject.FindProperty("UIEvent");
  }

  public override void OnInspectorGUI()
  {
    serializedObject.Update();

    DisplayListeners(rangeListeners);
    DisplayListeners(turnListeners);
    DisplayListeners(uiListeners);

    serializedObject.ApplyModifiedProperties();
  }

  public void DisplayListeners(SerializedProperty listeners)
  {
    EditorGUILayout.PropertyField(listeners);

    int length = listeners.arraySize;
    for (int i = 0; i < length; i++)
    {
      // Get the element SerializedProperty
      var element = listeners.GetArrayElementAtIndex(i);

      // The field exists but no object is referenced
      if (!element.objectReferenceValue)
      {
        continue;
      }

      // otherwise get a serialized object for this reference
      var sObj = new SerializedObject(element.objectReferenceValue);

      // stats is now a property of that serialized object
      var stats = sObj.FindProperty("Listeners");
    }

    EditorGUILayout.Space();
  }
}