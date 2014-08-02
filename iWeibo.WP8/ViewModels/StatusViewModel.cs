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
                var source = new TaskCompletionSource<Callback<WStatus>>();
                statusService.GetStatus(statusId.ToString(), callback => source.TrySetResult(callback));
                var result = await source.Task;
                if (result.Succeed)
                {
                    status = result.Data;
                    if (!ExistInDB(status))
                        InsertStatusToDB(status);
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
            var status = (from WStatus s in statusDB.Statuses
                          where s.Id == statusId
                          select s).FirstOrDefault();

            try
            {
                if (status != null && status.RetweetedStatusId != null)
                {
                    var retweedStatus = (from WStatus rs in statusDB.Statuses
                                         where rs.Id == status.RetweetedStatusId
                                         select rs).SingleOrDefault();

                    status.RetweetedStatus = retweedStatus;

                    if(retweedStatus.UserId!=null)
                    {
                        var user = (from WUser u in statusDB.Users
                                    where u.Id == retweedStatus.UserId
                                    select u).SingleOrDefault();
                        retweedStatus.User = user;
                    }
                }

                if (status != null && status.UserId != null)
                {
                    var user = (from WUser u in statusDB.Users
                                where u.Id == status.UserId
                                select u).SingleOrDefault();

                    status.User = user;
                }

                if (status != null && !string.IsNullOrEmpty(status.PicsStr))
                {
                    var urls = status.PicsStr.Split(',');
                    var picUrls = new List<PicUrl>();
                    foreach (var url in urls)
                    {
                        if (!string.IsNullOrEmpty(url))
                        {
                            var picUrl = new PicUrl { ThumbnailPic = url };
                            picUrls.Add(picUrl);
                        }
                    }

                    status.PicUrls = picUrls;
                }
            }
            catch (Exception e)
            {

            }

            return status;
        }

        //private WStatus ConvertContentToStatus(string content)
        //{
        //    WStatus status = null;

        //    if (!string.IsNullOrEmpty(content))
        //    {
        //        status = JsonConvert.DeserializeObject<WStatus>(content);
        //    }

        //    return status;
        //}


        public bool ExistInDB(WStatus status)
        {

            var statusInDB = (from WStatus s in statusDB.Statuses
                              where s.Id == status.Id
                              select s).SingleOrDefault();

            return statusInDB == null ? false : true;

        }

        public void InsertStatusToDB(WStatus status)
        {
            var userVM = App.UserViewModel;

            try
            {
                statusDB.Statuses.InsertOnSubmit(status);

                if (status.RetweetedStatus != null)
                {
                    status.RetweetedStatusId = status.RetweetedStatus.Id;

                    if (!ExistInDB(status.RetweetedStatus))
                        statusDB.Statuses.InsertOnSubmit(status.RetweetedStatus);

                    if(status.RetweetedStatus.User!=null)
                    {
                        status.RetweetedStatus.UserId = status.RetweetedStatus.User.Id;
                        if(!userVM.ExistInDB(status.RetweetedStatus.User))
                        {
                            userVM.InsertUserToDB(status.RetweetedStatus.User);
                        }
                    }
                }

                if (status.User != null)
                {
                    status.UserId = status.User.Id;

                    userVM = App.UserViewModel;

                    if (!userVM.ExistInDB(status.User))
                    {
                        userVM.InsertUserToDB(status.User);
                    }
                }

                if(status.PicUrls!=null)
                {
                    var urls = string.Empty;
                    foreach (var item in status.PicUrls)
                    {
                        urls += item.ThumbnailPic + ",";
                    }

                    status.PicsStr = urls;
                }

                statusDB.SubmitChanges();

            }
            catch (Exception e)
            {

            }
        }

        public void DeleteStatusFromDB(WStatus content)
        {
            statusDB.Statuses.DeleteOnSubmit(content);
            statusDB.SubmitChanges();
        }

    }
}
