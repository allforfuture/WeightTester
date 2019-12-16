﻿using System;
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
        private string machine_name_API = ConfigurationManager.AppSettings["machine_name_API"];
        public string machine_name
        { get { return machine_name_API; } }
        //public string M_FLEXURE_W { get; set; }
        //public string M_FLEXURE_E { get; set; }
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
                    machine_info = new Machine_info()
                }
            };
            string postBody = JsonConvert.SerializeObject(postJson);
            return postBody;
        }
    }
}
