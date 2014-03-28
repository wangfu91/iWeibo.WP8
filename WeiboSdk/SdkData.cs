
namespace WeiboSdk
{
     public class SdkData
    {
         static public string RedirectUri = "";

         static public string UserAgent { get; set; }
         static public  string AppKey { get; set; }
         static public  string AppSecret { get; set; }


         ////默认OAuth2.0
         static readonly internal EumAuth AuthOption = EumAuth.OAUTH2_0;
    }
}
