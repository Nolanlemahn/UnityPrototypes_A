using System.Collections.Generic;
using UnityEngine;
using NTBUtils;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class GameRegistration
{
    public string GameName = "null";
    public Sprite ButtonImage = null;
    public NTBUtils.SceneString Scene;
}

[CreateAssetMenu(fileName = "GameMenuAssetList", menuName = "GameMenuAssetList", order = 1001)]
public class GameMenuAssetList : SingletonScriptableObject<GameMenuAssetList>
{
    public List<GameRegistration> RegisteredGames;

    private Dictionary<string, GameRegistration> _loadDict;

    public void Init()
    {
        this._loadDict = new Dictionary<string, GameRegistration>();

        foreach (GameRegistration registeredGame in this.RegisteredGames)
        {
            this._loadDict[registeredGame.GameName] = registeredGame;
        }
    }

    public GameRegistration Registrar(string name)
    {
        GameRegistration ret = null;
        this._loadDict.TryGetValue(name, out ret);
        return ret;
    }
}

#if UNITY_EDITOR
public class GameMenuAssetListMaster
{
    [MenuItem("GamePackage/Re-Initialize GameMenuAssetList", false, 0)]
    public static GameMenuAssetList Create()
    {
        GameMenuAssetList asset = ScriptableObject.CreateInstance<GameMenuAssetList>();

        AssetDatabase.CreateAsset(asset, "Assets/Resources/GameMenuAssetList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
#endif
