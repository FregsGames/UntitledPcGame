using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckerGUID : MonoBehaviour
{
    [PropertySpace(10, 0)]
    [Button("Check GUIDs", ButtonSizes.Gigantic)]
    public void Check()
    {
        UniqueID[] uniqueIDs = FindObjectsOfType<UniqueID>(true);

        List<UniqueID> repeatedIDs = uniqueIDs.Where(u => uniqueIDs.Any(other => other != u && other.ID == u.ID)).ToList();

        ClearConsole();

        if (repeatedIDs.Count > 0)
        {
            foreach (var repeatedID in repeatedIDs)
            {
                Debug.LogWarning("Repeated id: " + repeatedID.gameObject + "(" + repeatedID.ID + ")");
            }
        }
        else
        {
            Debug.Log("All GUIDs ok!");
        }

    }

    static void ClearConsole()
    {
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");

        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

        clearMethod.Invoke(null, null);
    }
}
