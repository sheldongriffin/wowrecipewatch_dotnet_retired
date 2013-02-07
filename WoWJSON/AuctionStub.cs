using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WoWJSON
{
    [DataContract]
    public class AuctionStub
    {
        [DataMember(Name = "files")]
        public List<AuctionUpdate> Files;
    }

    [DataContract]
    public class AuctionUpdate
    {
        [DataMember(Name = "url")]
        public string Url;
        [DataMember(Name = "lastModified")]
        public long LastModified;
    }
}
