using Snek.Utilities;
using UnityEditor;

[SnekAutoGenerateAsset("Assets/_Scripts/Systems/GameManager/Editor", nameof(GameManagerInspectorCache))]
[UseSnekInspector(true)]
public class GameManagerInspectorCache : SnekScriptableObject
{
    public SceneAsset MainMenuScene;
    public SceneAsset GameScene;
}
