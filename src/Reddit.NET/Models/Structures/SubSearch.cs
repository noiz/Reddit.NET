﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Reddit.NET.Models.Structures
{
    [Serializable]
    public class SubSearch
    {
        [JsonProperty("subreddits")]
        public List<SubSearchResult> Subreddits;
    }
}
