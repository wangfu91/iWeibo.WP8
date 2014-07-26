using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using WeiboSdk.Models;
using System.Data.Linq.Mapping;

namespace iWeibo.WP8.Models.Sina
{
    public class StatusDataContext:DataContext
    {
        public StatusDataContext(string connectionString)
            : base(connectionString) { }

        public Table<WStatus> StatusTable;
    }

}
