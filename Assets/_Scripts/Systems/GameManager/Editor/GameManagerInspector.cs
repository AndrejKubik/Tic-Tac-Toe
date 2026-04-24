using SnekEditor.GUIUtilities;
using SnekEditor.ScriptableObjectManager;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : SnekMonoBehaviourInspectorCustom<GameManager>
{
    private GameManagerInspectorCache _cache;
    private SerializedObject so_Cache;

    private SerializedProperty sp_MainMenuScene;
    private SerializedProperty sp_GameScene;

    private SerializedProperty sp_MainMenuSceneName;
    private SerializedProperty sp_GameSceneName;

    private SnekObjectField<SceneAsset> _mainMenuSceneField;
    private SnekObjectField<SceneAsset> _gameSceneField;

    protected override void OnCreateInspectorInstance()
    {
        _cache = SnekScriptableObjectManager.GetAsset<GameManagerInspectorCache>();
        so_Cache = new SerializedObject(_cache);

        sp_MainMenuScene = so_Cache.FindProperty(nameof(_cache.MainMenuScene));
        sp_GameScene = so_Cache.FindProperty(nameof(_cache.GameScene));

        sp_MainMenuSceneName = GetProperty(nameof(GameManager.MainMenuSceneName));
        sp_GameSceneName = GetProperty(nameof(GameManager.GameSceneName));

        _mainMenuSceneField = new SnekObjectField<SceneAsset>(sp_MainMenuScene, "Main Menu Scene", false);
        _gameSceneField = new SnekObjectField<SceneAsset>(sp_GameScene, "Game Scene", false);

        UpdateSceneNames();
    }

    protected override bool Initialize()
    {
        return _cache != null && base.Initialize();
    }

    protected override void DrawContent()
    {
        so_Cache.Update();

        _mainMenuSceneField.DrawHorizontal();
        _gameSceneField.DrawHorizontal();

        if (so_Cache.ApplyModifiedProperties())
            UpdateSceneNames();
    }

    private void UpdateSceneNames()
    {
        sp_MainMenuSceneName.stringValue = _cache.MainMenuScene == null ?
            string.Empty : _cache.MainMenuScene.name;

        sp_GameSceneName.stringValue = _cache.GameScene == null ?
            string.Empty : _cache.GameScene.name;

        serializedObject.ApplyModifiedProperties();
    }
}
