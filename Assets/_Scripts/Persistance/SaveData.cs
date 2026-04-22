using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public struct SaveData
{
    [JsonProperty] public List<RoundData> PlayedRounds;
}
