using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace 测试
{
    class Program
    {
        static void Main(string[] args)
        {
            //2018-07-26 16:06:53:187
            DateTime timeStampStartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var d = (long)(new DateTime(2018, 07, 26, 16, 06, 53, 187).ToUniversalTime() - timeStampStartTime).TotalMilliseconds;
            Console.WriteLine(d);
          
            var a = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;//获取秒
            var strs = new List<string> { "aaa", "bbb" };

            Console.ReadKey();
        }

        /// <summary>
        /// 生成长度为13的时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static long GetTimeStamp(DateTime dt)
        {
            DateTime timeStampStartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var d = (long)(new DateTime(2018, 07, 26, 16, 06, 53, 187).ToUniversalTime() - timeStampStartTime).TotalMilliseconds;
            return d;
        }
        //new DateTime(2018, 07, 26, 16, 06, 53, 187)

        /// <summary>
        /// 生成32位签名sign
        /// </summary>
        /// <param name="readyToMD5"></param>
        /// <returns></returns>
        private static string GetMD5Sign(string readyToMD5)
        {

            StringBuilder builder = new StringBuilder();
            using (MD5CryptoServiceProvider md5Crypto = new MD5CryptoServiceProvider())
            {
                byte[] data = md5Crypto.ComputeHash(Encoding.UTF8.GetBytes(readyToMD5));
                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("x2"));
                }
            }
            return builder.ToString();


        }

    }
    public class A : BaseResponse
    {
        [JsonProperty("aa")]
        public string AA { get; set; }
        [JsonProperty("result")]
        public DataNew ResultNew { get; set; }
    }

    public class DataNew { public int MyProperty { get; set; } }
    public class Student
    {
        public int sku { get; set; }
        public string name { get; set; }
        public int num { get; set; }
        public int boxnum { get; set; }
    }
    public class Data { public string Old { get; set; } }
    public class BaseResponse
    {
        [JsonProperty("result")]
        public Data Result { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }
    }

    public class Respone : BaseResponse
    {
        public Object Data { get; set; }
        public ErrorData error { get; set; }
    }


    public class ErrorData
    {
        [JsonProperty("error")]
        public string error { get; set; }
    }
    public class SuccessData
    {
        [JsonProperty("success")]
        public string success { get; set; }
    }

    public class PeopleInfo
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }

        public List<string> Hobby { get; set; }

        //注意方法名称以及方法类型
        public bool ShouldSerializeName()
        {
            if (this.Name == "李二狗")
            { //如果名称是李四，则Name属性不序列化
                return false;
            }
            else
            {
                return true;
            }

        }
    }

    public class LimitPropsContractResolver : DefaultContractResolver
    {
        string[] Propertys = null;
        bool IsSerialize;
        public LimitPropsContractResolver(string[] props, bool retain = true)
        {
            this.Propertys = props;
            this.IsSerialize = retain;
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);
            return list.Where(p =>
            {
                if (IsSerialize)
                {
                    return Propertys.Contains(p.PropertyName);
                }
                else
                {
                    return !Propertys.Contains(p.PropertyName);
                }
            }).ToList();
        }
    }

    public class ConsignmentInfo
    {
        /// <summary>
        /// 客户单号
        /// </summary>
        [JsonProperty("ref_no")]
        public string RefNo { get; set; }

        /// <summary>
        /// 4PX单号
        /// </summary>
        [JsonProperty("4px_tracking_no")]
        public string FourPXTrackingNo { get; set; }

        /// <summary>
        /// 客户委托单号
        /// </summary>
        [JsonProperty("ds_consignment_no")]
        public string DsConsignmentNo { get; set; }

        /// <summary>
        /// 委托单状态
        /// </summary>
        [JsonProperty("consignment_status")]
        public string ConsignmentStatus { get; set; }

        /// <summary>
        /// 物流渠道号码。如果结果返回为空字符，表示暂时没有物流渠道号码，请稍后主动调用查询直发委托单接口查询
        /// </summary>
        [JsonProperty("logistics_channel_no")]
        public string LogisticsChannel_no { get; set; }
    }
    public class ConsignmentInfoData
    {
        [JsonProperty("consignment_info")]
        public ConsignmentInfo ConsignmentInfo { get; set; }
    }
    public class FourPXTurnDataResponse
    {
        [JsonProperty("data")]
        public string JsonData { get; set; }

        public List<ConsignmentInfoData> ConsignmentInfos
        {
            get
            {
                List<ConsignmentInfoData> temp = null;
                if (!string.IsNullOrWhiteSpace(this.JsonData))
                {
                    temp = JsonConvert.DeserializeObject<List<ConsignmentInfoData>>(this.JsonData);
                }

                return temp;
            }
        }
    }
}

