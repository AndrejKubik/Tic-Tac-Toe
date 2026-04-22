using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class PersistenceManager : SnekMonoSingleton
{
    private const string SaveFileName = "history.json";

    public void SavePlayedRounds(List<RoundData> playedRounds)
    {
        var data = new SaveData
        {
            PlayedRounds = playedRounds
        };

        string jsonFile = JsonConvert.SerializeObject(data, Formatting.Indented);

        File.WriteAllText(GetSavePath(), jsonFile);
        WebGLFileSystem.Sync();
    }

    public List<RoundData> LoadPlayedRounds()
    {
        if (!File.Exists(GetSavePath()))
            return new List<RoundData>();

        string jsonFile = File.ReadAllText(GetSavePath());
        var data = JsonConvert.DeserializeObject<SaveData>(jsonFile, GetDeserializerSettings());

        if(data.PlayedRounds == null)
            return new List<RoundData>();

        return new List<RoundData>(data.PlayedRounds);
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, SaveFileName);
    }

    private JsonSerializerSettings GetDeserializerSettings()
    {
        return new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}
