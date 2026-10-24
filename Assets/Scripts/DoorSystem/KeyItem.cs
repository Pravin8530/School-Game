
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public string keyID = "key_01";

    private static HashSet<string> collectedKeys = new HashSet<string>();

    public static void RegisterKey(string id)
    {
        collectedKeys.Add(id);
    }

    public static bool HasKey(string id)
    {
        return collectedKeys.Contains(id);
    }
}
