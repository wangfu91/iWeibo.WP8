
namespace TencentWeiboSDK.Model
{
    /// <summary>
    /// 公司信息
    /// </summary>
    public class Company:BaseModel
    {
        private int begin_year;
        /// <summary>
        /// 开始年
        /// </summary>
        public int Begin_Year
        {
            get { return begin_year; }
            set { begin_year = value; }
        }

        private string company_name;
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Company_Name
        {
            get { return company_name; }
            set
            {
                if (value != company_name)
                {
                    company_name = value;
                    NotifyPropertyChanged("Company_Name");
                }
            }
        }

        private string department_name;
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Department_Name
        {
            get { return department_name; }
            set
            {
                if (value != department_name)
                {
                    department_name = value;
                    NotifyPropertyChanged("Department_Name");
                }
            }
        }

        private int end_year;
        /// <summary>
        /// 结束年
        /// </summary>
        public int End_Year
        {
            get { return end_year; }
            set
            {
                if (value != end_year)
                {
                    end_year = value;
                    NotifyPropertyChanged("End_Year");
                }
            }
        }

        private int id;
        /// <summary>
        /// 公司Id
        /// </summary>
        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

    }
}
