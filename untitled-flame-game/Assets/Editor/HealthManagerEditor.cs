using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HealthManager))]
public class HealthManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // Draws the default inspector

        HealthManager healthManager = (HealthManager)target;

        // Test buttons
        if (GUILayout.Button("Take Damage"))
        {
            healthManager.TakeDamage(1); // You can change the damage value here
        }

        if (GUILayout.Button("Heal"))
        {
            healthManager.Heal(1); // You can change the heal value here
        }
    }
}
