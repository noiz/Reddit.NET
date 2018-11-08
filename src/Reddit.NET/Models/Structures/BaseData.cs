﻿using Newtonsoft.Json;
using System;

namespace Reddit.NET.Models.Structures
{
    [Serializable]
    public abstract class BaseData
    {
        [JsonProperty("modhash")]
        public string Modhash;

        [JsonProperty("dist")]
        public int? Dist;

        [JsonProperty("after")]
        public string after;

        [JsonProperty("before")]
        public string before;
    }
}
