using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWeibo.WP8
{
    public class Constants
    {

        #region User Info Storage Path
        public const string TencentUserInfo = "/Tencent/OAuthedUserInfo.dat";
        public const string SinaUserInfo = "/Sina/OAuthedUserInfo.dat";

        public const string TencentUserName = "TencentUserName";
        public const string SinaUserName = "SinaUserName";
        #endregion

        #region Views Path
        public const string MainPageView = "/Views/MainPage.xaml";
        public const string CreateNewView = "/Views/CreateNew.xaml";
        public const string SettingsView = "/Views/SettingsView.xaml";
        public const string PictureView = "/Views/PictureView.xaml";
        public const string RepostView = "/Views/RepostView.xaml";

        public const string SinaLoginView = "/Views/Sina/Login.xaml";
        public const string SinaTimelineView = "/Views/Sina/Timeline.xaml";
        public const string SinaStatusDetailView = "/Views/Sina/StatusDetail.xaml";

        public const string TencentLoginView = "/Views/Tencent/Login.xaml";
        public const string TencentTimelineView = "/Views/Tencent/Timeline.xaml";
        public const string TencentStatusDetailView = "/Views/Tencent/StatusDetail.xaml";

        #endregion


        #region Timeline Storage Path
        public const string SinaHomeTime = "/Sina/HomeTimeline.dat";
        public const string SinaMentionsTimeline = "/Sina/MentionsTimeline.dat";
        public const string SinaFavoritesTimeline = "/Sina/FavoritesTimeline.dat";

        public const string TencentHomeTimeline = "/Tencent/HomeTimeline.dat";
        public const string TencentMentionsTimeline = "/Tencent/MentionsTimeline.dat";
        public const string TencentFavoritesTimeline = "/Tencent/FavoritesTimeline.dat";

        #endregion

        #region Status Storage Path
        public const string TencentSelectedStatus = "/Tencent/SelectedStatus.dat";
        public const string SinaSelectedStatus = "/Sina/SelectedStatus.dat";
        #endregion

    }
}
