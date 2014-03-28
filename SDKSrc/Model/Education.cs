
using System.Runtime.Serialization;
namespace TencentWeiboSDK.Model
{
    [DataContract]
    public class Education:BaseModel
    {
        public Education()
        {
        }

        [DataMember(Name = "departmentid")]
        public int DepartmentId { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "level")]
        public int Level { get; set; }

        [DataMember(Name = "schoolid")]
        public int SchoolId { get; set; }

        [DataMember(Name="year")]
        public int Year { get; set; }

    }
}
