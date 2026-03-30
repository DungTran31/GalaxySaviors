#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class PlayModeStateHandler
{
    static PlayModeStateHandler()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            ResetScriptableObjects();
        }
    }

    private static void ResetScriptableObjects()
    {
        // Find all instances of PlayerBulletSO and reset their values
        var bulletSOs = Resources.FindObjectsOfTypeAll<DungTran31.GamePlay.Player.SO.PlayerBulletSO>();
        foreach (var bulletSO in bulletSOs)
        {
            bulletSO.ResetToOriginalValues();
        }
    }
}
#endif
