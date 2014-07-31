using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboSdk.Models
{
    [Table]
    public class StatusContent : BaseModel
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = false, CanBeNull = false)]
        public long Id { get; set; }

        [Column(DbType="NTEXT")]
        public string JsonContent { get; set; }

    }
}
