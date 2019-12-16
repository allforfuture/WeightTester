using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using Newtonsoft.Json;

namespace WeightTester.Json
{
    public class Inspect_itemsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string inspect_cd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string inspect_upper { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string inspect_lower { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string inspect_value { get; set; }
        
        public string inspect_judge
        {
            get
            {
                double v = Convert.ToDouble(inspect_value);
                double u = Convert.ToDouble(inspect_upper);
                double l = Convert.ToDouble(inspect_lower);
                string result = v <= u && v >= l ? "0" : "1";
                return result;
            }
        }
    }

    public class Test_attributes
    {
        /// <summary>
        /// 
        /// </summary>
        public string process_start { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string process_stop { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string total_judge { get; set; }
    }

    public class Child_serial_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string serial_cd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string datatype_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lot_cd { get; set; }
    }

    public class Jig_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string BUCK { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string JIG_INNER { get; set; }
    }

    public class Machine_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string M_FLEXURE_W { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string M_FLEXURE_E { get; set; }
    }

    public class Operator_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string OP_HOT_BAR { get; set; }
    }

    public class Mcset_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string MS_DOCKING { get; set; }
    }

    public class SendResultDetails
    {
        private string model_cd_API= ConfigurationManager.AppSettings["model_cd_API"];
        public string model_cd
        {
            get { return model_cd_API; }
            set { model_cd_API = value; }
        }

        private string site_cd_API = ConfigurationManager.AppSettings["site_cd_API"];
        public string site_cd
        {
            get { return site_cd_API; }
            set { site_cd_API = value; }
        }

        private string factory_cd_API = ConfigurationManager.AppSettings["factory_cd_API"];
        public string factory_cd
        {
            get { return factory_cd_API; }
            set { factory_cd_API = value; }
        }

        private string line_cd_API = ConfigurationManager.AppSettings["line_cd_API"];
        public string line_cd
        {
            get { return line_cd_API; }
            set { line_cd_API = value; }
        }

        private string process_cd_API = ConfigurationManager.AppSettings["process_cd_API"];
        public string process_cd
        {
            get { return process_cd_API; }
            set { process_cd_API = value; }
        }

        public string serial_cd { get; set; }
        
        private string datatype_id_API = ConfigurationManager.AppSettings["datatype_id_API"];
        public string datatype_id
        {
            get { return datatype_id_API; }
            set { datatype_id_API = value; }
        }
        
        private string lot_cd_API = ConfigurationManager.AppSettings["lot_cd_API"];
        public string lot_cd
        {
            get { return lot_cd_API; }
            set { lot_cd_API = value; }
        }
        
        private string mo_cd_API = ConfigurationManager.AppSettings["mo_cd_API"];
        public string mo_cd
        {
            get { return mo_cd_API; }
            set { mo_cd_API = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public List<Inspect_itemsItem> inspect_items { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Test_attributes test_attributes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Child_serial_info child_serial_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Jig_info jig_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Machine_info machine_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Operator_info operator_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Mcset_info mcset_info { get; set; }
    }

    public class PostJson
    {
        /// <summary>
        /// 
        /// </summary>
        public SendResultDetails sendResultDetails { get; set; }
    }

    static class CSV2PostBody
    {
        public static string PostBody(string[] csvArr)
        {
            #region 需要post的参数
            //POST中的inspect_items
            int itemsCount = Convert.ToUInt16(csvArr[15]);
            List<Inspect_itemsItem> itemsList = new List<Inspect_itemsItem>();
            for (int i = 0; i < itemsCount; i++)
            {
                Inspect_itemsItem item = new Inspect_itemsItem()
                {
                    inspect_cd = csvArr[16 + (5 * i)],
                    inspect_upper = csvArr[17 + (5 * i)],
                    inspect_lower = csvArr[17 + (5 * i)],
                    inspect_value = csvArr[17 + (5 * i)],
                    //inspect_judge = csvArr[20 + (5 * i)]
                };
                itemsList.Add(item);
            }

            //POST中的test_attributes
            string timeStr = string.Format("20{0}-{1}-{2} {3}:{4}:{5}",
                csvArr[9], csvArr[10], csvArr[11], csvArr[12], csvArr[13], csvArr[14]);
            int total_judge_Location = 15 + (5 * itemsCount) + 1;

            //POST中的附加项
            Jig_info jig_Info = new Jig_info();
            Machine_info machine_Info = new Machine_info();
            Operator_info operator_Info = new Operator_info();
            Mcset_info mcset_Info = new Mcset_info();
            string nowType = "";
            for (int i = total_judge_Location + 1; i < csvArr.Length; i++)
            {
                switch (csvArr[i])
                {
                    case "JIG":
                        nowType = "JIG";
                        continue;
                    case "MACHINE":
                        nowType = "MACHINE";
                        continue;
                    case "OPERATOR":
                        nowType = "OPERATOR";
                        continue;
                    case "MCSET":
                        nowType = "MCSET";
                        continue;
                }
                switch (nowType)
                {
                    case "JIG":
                        jig_Info.JIG_INNER = csvArr[i];
                        continue;
                    case "MACHINE":
                        if (machine_Info.M_FLEXURE_W == null)
                        { machine_Info.M_FLEXURE_W = csvArr[i]; }
                        else
                        { machine_Info.M_FLEXURE_E = csvArr[i]; }
                        continue;
                    case "OPERATOR":
                        operator_Info.OP_HOT_BAR = csvArr[i];
                        continue;
                    case "MCSET":
                        mcset_Info.MS_DOCKING = csvArr[i];
                        continue;
                }
            }
            #endregion

            PostJson postJson = new PostJson()
            {
                sendResultDetails = new SendResultDetails()
                {
                    model_cd = csvArr[0],
                    site_cd = csvArr[1],
                    factory_cd = csvArr[2],
                    line_cd = csvArr[3],
                    process_cd = csvArr[4],
                    serial_cd = csvArr[5],
                    //datatype_id = "BASE",
                    lot_cd="N/A",
                    //mo_cd="N/A",
                    inspect_items = itemsList,
                    test_attributes = new Test_attributes()
                    {
                        process_start = timeStr,
                        process_stop = timeStr,
                        total_judge = csvArr[total_judge_Location]
                    },
                    child_serial_info = null,
                    jig_info = jig_Info,
                    machine_info = machine_Info,
                    operator_info = operator_Info,
                    mcset_info = mcset_Info
                }
            };
            string postBody = JsonConvert.SerializeObject(postJson);
            return postBody;
        }

        public static string PostBody(string sn, double inspect_Weigrt3, double inspect_WaterWeigrt)
        {
            #region 检查项
            List<Inspect_itemsItem> itemsList = new List<Inspect_itemsItem>();
            Inspect_itemsItem weight3 = new Inspect_itemsItem()
            {
                inspect_cd = ConfigurationManager.AppSettings["inspect_cd_3"],
                inspect_upper = ConfigurationManager.AppSettings["inspect_upper_3"],
                inspect_lower = ConfigurationManager.AppSettings["inspect_lower_3"],
                inspect_value = inspect_Weigrt3.ToString(),
            };
            itemsList.Add(weight3);
            Inspect_itemsItem weightWater = new Inspect_itemsItem()
            {
                inspect_cd = ConfigurationManager.AppSettings["inspect_cd_Water"],
                inspect_upper = ConfigurationManager.AppSettings["inspect_upper_Water"],
                inspect_lower = ConfigurationManager.AppSettings["inspect_lower_Water"],
                inspect_value = inspect_WaterWeigrt.ToString(),
            };
            itemsList.Add(weightWater);
            #endregion
            #region 所有检查项的总判断
            string tempJudge = "0"; ;
            foreach (var var in itemsList)
            {
                if (var.inspect_judge != "0")
                {
                    tempJudge = "1";
                    break;
                }
            }
            #endregion

            string timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            PostJson postJson = new PostJson()
            {
                sendResultDetails = new SendResultDetails()
                {
                    serial_cd = sn,
                    inspect_items = itemsList,
                    test_attributes = new Test_attributes()
                    {
                        process_start = timeStr,
                        process_stop = timeStr,
                        total_judge = tempJudge
                    },
                    //machine_info = machine_Info,


                    //child_serial_info = null,
                    //jig_info = jig_Info,
                    //operator_info = operator_Info,
                    //mcset_info = mcset_Info
                }
            };
            string postBody = JsonConvert.SerializeObject(postJson);
            return postBody;
        }
    }
}
