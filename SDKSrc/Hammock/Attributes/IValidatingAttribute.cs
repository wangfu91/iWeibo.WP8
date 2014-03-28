using System.Reflection;

namespace TencentWeiboSDK.Hammock.Attributes
{
    public interface IValidatingAttribute
    {
        string TransformValue(PropertyInfo property, object value);
    }
}