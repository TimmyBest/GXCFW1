using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class MapMulti
    {
        public string Type { get; set; }
        public FieldsMulti Fields { get; set; }
    }
}