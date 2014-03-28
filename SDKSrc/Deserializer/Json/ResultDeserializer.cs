using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TencentWeiboSDK.Model;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TencentWeiboSDK.Deserializer.Json
{
    public class ResultDeserializer : BaseDeserializer<Result>
    {
        public ResultDeserializer()
        { }

        public override Result Read(string content)
        {
            if (content == null)
                return new Result();

            var je = JObject.Parse(content);

            return je.ToObject<Result>();
        }

        public override List<Result> ReadList(string content)
        {
            return null;
        }
    }
}
