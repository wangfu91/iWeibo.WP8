using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WeiboSdk.Models
{
    [DataContract]
    public class StatusIds:BaseModel
    {
        [DataMember(Name="statuses")]
        public List<long> Statuses { get; set; }

        [DataMember(Name="advertises")]
        public List<string> Advertises { get; set; }

        [DataMember(Name = "ad")]
        public List<string> Ad { get; set; }

        [DataMember(Name="hasvisible")]
        public bool HasVisible { get; set; }

        [DataMember(Name="previous_cursor")]
        public long PreviousCursor { get; set; }

        [DataMember(Name="next_cursor")]
        public long NextCursor { get; set; }

        [DataMember(Name = "total_number")]
        public int TotalNumber { get; set; }

        [DataMember(Name="interval")]
        public int Interval { get; set; }
                
    }
}
