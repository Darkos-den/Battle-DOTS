using UnityEditor;
using UnityEngine;

public class GameStateEditor : EditorWindow
{

    [MenuItem("Window/BattleDots/GameState")]
    public static void ShowWindow() {
        GetWindow<GameStateEditor>("Game state");
    }

    private void OnGUI() {
        GUILayout.Label("Sample", EditorStyles.boldLabel);

        //GUILayout.Box(new Rect(0,0,10,5));
    }
}
