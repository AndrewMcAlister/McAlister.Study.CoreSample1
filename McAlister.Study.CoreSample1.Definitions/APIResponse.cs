using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace McAlister.Study.CoreSample1.Definitions
{
    [DataContract]
    public class APIResponse
    {
        [DataMember]
        public int Status { get; set; } = 200;
        [DataMember]
        public Boolean Successful { get; set; }
        [DataMember]
        public Object Payload { get; set; }
        [DataMember]
        public String Message { get; set; }
    }
}
