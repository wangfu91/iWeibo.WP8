using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WeiboSdk.Models
{
    [DataContract]
    public abstract class WCollection:BaseModel
    {
        [DataMember(Name="next_cursor")]
        public long NextCursor { get; set; }

        //[DataMember(Name="previous_cursor")]
        //public long PreviousCursor { get; set; }

        [DataMember(Name="total_number")]
        public int TotalNumber { get; set; }

    }

    [DataContract]
    public class WUserCollection : WCollection
    {
        [DataMember(Name = "users")]
        public List<WUser> Users { get; set; }
    }

    [DataContract]
    public class WStatusCollection : WCollection
    {
        [DataMember(Name = "statuses")]
        public List<WStatus> Statuses { get; set; }

        public long PreviousCursor
        {
            get
            {
                return this.Statuses.FirstOrDefault().Id;
            }
        }
        

    }

    [DataContract]
    public class WCommentCollection : WCollection
    {
        [DataMember(Name="comments")]
        public List<WStatus> Comments { get; set; }

        public long PreviousCursor
        {
            get
            {
                return this.Comments.FirstOrDefault().Id;
            }
        }

    }

    [DataContract]
    public class WFavoriteCollection : WCollection
    {
        [DataMember(Name="favorites")]
        public List<WStatus> Favorites { get; set; }

        public long PreviousCursor
        {
            get
            {
                return this.Favorites.FirstOrDefault().Id;
            }
        }

    }
}
