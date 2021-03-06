﻿using Newtonsoft.Json;
using System;

namespace Reddit.Things
{
    [Serializable]
    public class SubredditName
    {
        [JsonProperty("name")]
        public string Name;

        public SubredditName(string name)
        {
            Name = name;
        }

        public SubredditName() { }
    }
}
