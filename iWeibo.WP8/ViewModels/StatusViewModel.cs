using iWeibo.Services;
using iWeibo.WP8.Models.Sina;
using Microsoft.Practices.Prism.ViewModel;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeiboSdk.Models;
using WeiboSdk.Services;

namespace iWeibo.WP8.ViewModels
{
    public class StatusViewModel:NotificationObject
    {
        private StatusDataContext statusDB;

        private WStatusService statusService = new WStatusService(TokenIsoStorage.SinaTokenStorage.LoadData<SinaAccessToken>());

        public StatusViewModel(string statusDBConnectionString)
        {
            statusDB = new StatusDataContext(statusDBConnectionString);
        }

        public void SaveChangesToDB()
        {
            statusDB.SubmitChanges();
        }

        public async Task<WStatus> GetStatusByIdAsync(long statusId)
        {
            var status = LoadStatusFromDB(statusId);
            if(status==null)
            {
                var source=new TaskCompletionSource<Callback<WStatus>>();
                statusService.GetStatus(statusId.ToString(),callback=>source.SetResult(callback));
                var result =await source.Task;
                if(result.Succeed)
                {
                    status = result.Data;
                }
                else
                {
                    MessageBox.Show(result.ErrorMsg);
                }
            }

            return status;
        }

        private WStatus LoadStatusFromDB(long statusId)
        {
            var statusInDB = (from WStatus status in statusDB.StatusTable
                              where status.Id == statusId
                              select status).FirstOrDefault();

            return statusInDB;
        }


        public void AddStatusToDB(WStatus status)
        {
            statusDB.StatusTable.InsertOnSubmit(status);

            statusDB.SubmitChanges();
        }

        public void DeleteStatusFromDB(WStatus status)
        {
            statusDB.StatusTable.DeleteOnSubmit(status);
        }

        public void DeleteDB()
        {
            statusDB.DeleteDatabase();
        }
    }
}
