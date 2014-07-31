using iWeibo.Services;
using iWeibo.WP8.Models.Sina;
using Microsoft.Practices.Prism.ViewModel;
using Newtonsoft.Json;
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
    public class StatusViewModel : NotificationObject
    {
        private StatusDataContext statusDB;

        private WStatusService statusService = new WStatusService(TokenIsoStorage.SinaTokenStorage.LoadData<SinaAccessToken>());

        public StatusViewModel(string statusDBConnectionString)
        {
            statusDB = new StatusDataContext(statusDBConnectionString);
        }


        public async Task<WStatus> GetStatusByIdAsync(long statusId)
        {
            var status = LoadStatusFromDB(statusId);
            if (status == null)
            {
                var source = new TaskCompletionSource<Callback<string>>();
                statusService.GetStatusContent(statusId, callback => source.TrySetResult(callback));
                var result = await source.Task;
                if (result.Succeed)
                {
                    if (!string.IsNullOrEmpty(result.Data))
                    {
                        var len = result.Data.Length;
                        var statusContent=new StatusContent
                        {
                            Id=statusId,
                            JsonContent=result.Data
                        };
                        InsertStatusToDB(statusContent);

                        status = ConvertContentToStatus(result.Data);
                    }
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(result.ErrorMsg));
                }
            }

            return status;
        }

        private WStatus LoadStatusFromDB(long statusId)
        {
            var statusContent = (from StatusContent s in statusDB.StatusContents
                              where s.Id == statusId
                              select s.JsonContent).FirstOrDefault();

            return ConvertContentToStatus(statusContent);
        }

        private WStatus ConvertContentToStatus(string content)
        {
            WStatus status = null;

            if (!string.IsNullOrEmpty(content))
            {
                status = JsonConvert.DeserializeObject<WStatus>(content);
            }

            return status;
        }

        public void InsertStatusToDB(StatusContent content)
        {
            statusDB.StatusContents.InsertOnSubmit(content);

            statusDB.SubmitChanges();
        }

        public void DeleteStatusFromDB(StatusContent content)
        {
            statusDB.StatusContents.DeleteOnSubmit(content);
            statusDB.SubmitChanges();
        }

        public void DeleteDB()
        {
            statusDB.DeleteDatabase();
            statusDB.SubmitChanges();
        }
    }
}
