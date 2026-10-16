using UnityEngine;
using EventSys.Events;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "EventBus", menuName = "Scriptable Objects/EventBus")]
public class EventBus : ScriptableObject
{
  public RangeEvent MyRangeEvent { get; private set; }
  public TurnEvent MyTurnEvent { get; private set; }
  public UIEvent MyUIEvent { get; private set; }
  public TargetEvent MyTargetEvent { get; private set; }
  public BattleEvent MyBattleEvent { get; private set; }

  [SerializeField] private List<Object> RangeEventListeners;
  [SerializeField] private List<Object> TurnEventListeners;
  [SerializeField] private List<Object> UIEventListeners;
  [SerializeField] private List<Object> TargetEventListeners;
  [SerializeField] private List<Object> BattleEventListeners;

  public void AddEvents()
  {
    MyRangeEvent = new(1);
    RangeEventListeners = new(1);

    MyTurnEvent = new(1);
    TurnEventListeners = new(1);

    MyUIEvent = new(1);
    UIEventListeners = new(1);

    MyTargetEvent = new(1);
    TargetEventListeners = new(1);

    MyBattleEvent = new(1);
    BattleEventListeners = new(1);
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

    TargetEventListeners.Clear();
    foreach (TargetingSystem ts in MyTargetEvent.Listeners)
      TargetEventListeners.Add(ts);

    // Either units or parties
    BattleEventListeners.Clear();
    foreach (PartyStatus<UnitController> unit in MyBattleEvent.Listeners)
      BattleEventListeners.Add(unit);
  }

  public void ClearEvents()
  {
    MyRangeEvent.ClearListeners();
    MyRangeEvent = null;

    MyTurnEvent.ClearListeners();
    MyTurnEvent = null;

    MyUIEvent.ClearListeners();
    MyUIEvent = null;

    MyTargetEvent.ClearListeners();
    MyTargetEvent = null;

    MyBattleEvent.ClearListeners();
    MyBattleEvent = null;
  }

}

// [CustomEditor(typeof(EventBusEditor))]
// public class EventBusEditor : Editor
// {
//   SerializedProperty rangeListeners;
//   SerializedProperty turnListeners;
//   SerializedProperty uiListeners;


//   private void OnEnable()
//   {
//     rangeListeners = serializedObject.FindProperty("RangeEvent");
//     turnListeners = serializedObject.FindProperty("TurnEvent");
//     uiListeners = serializedObject.FindProperty("UIEvent");
//   }

//   public override void OnInspectorGUI()
//   {
//     serializedObject.Update();

//     DisplayListeners(rangeListeners);
//     DisplayListeners(turnListeners);
//     DisplayListeners(uiListeners);

//     serializedObject.ApplyModifiedProperties();
//   }

//   public void DisplayListeners(SerializedProperty listeners)
//   {
//     EditorGUILayout.PropertyField(listeners);

//     int length = listeners.arraySize;
//     for (int i = 0; i < length; i++)
//     {
//       // Get the element SerializedProperty
//       var element = listeners.GetArrayElementAtIndex(i);

//       // The field exists but no object is referenced
//       if (!element.objectReferenceValue)
//       {
//         continue;
//       }

//       // otherwise get a serialized object for this reference
//       var sObj = new SerializedObject(element.objectReferenceValue);

//       // stats is now a property of that serialized object
//       var stats = sObj.FindProperty("Listeners");
//     }

//     EditorGUILayout.Space();
//   }
// }