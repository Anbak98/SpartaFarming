using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestInventory))]
public class TestInventoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TestInventory testInventory = (TestInventory)target;
        if (GUILayout.Button("AddItem"))
        {
            testInventory.AddItem();
        }
        else if (GUILayout.Button("RemoveItem"))
        {
            testInventory.RemoveItem();
        }
    }
}
