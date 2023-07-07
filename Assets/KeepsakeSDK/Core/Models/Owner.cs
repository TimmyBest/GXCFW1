using System;

namespace KeepsakeSDK.Core.Models
{
    [Serializable]
    public class Owner
    {
        public string ObjectOwner { get; set; }
        public Shared Shared { get; set; }
        public string AddressOwner { get; set; }
    }
}
