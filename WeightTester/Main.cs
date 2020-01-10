using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Win32;
using System.Configuration;
using System.IO.Ports;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace WeightTester
{
    public partial class Main : Form
    {
        //天平（串口）
        bool getWeightSwitch;
        bool noWeight;
        string portLastStr = "";
        readonly int waitWeightTime = int.Parse(ConfigurationManager.AppSettings["waitWeightTime"]);
        readonly int waitAnalysisTime = int.Parse(ConfigurationManager.AppSettings["waitAnalysisTime"]);
        double weight3;
        //扫描器（以太网）
        string LastSN { get { return txtLastSN.Text; } }
        readonly string Hostname = ConfigurationManager.AppSettings["Hostname"];
        readonly int Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
        //数据库属性
        readonly string site_cd = ConfigurationManager.AppSettings["site_cd"];
        readonly string factory_cd = ConfigurationManager.AppSettings["factory_cd"];
        readonly string line_cd = ConfigurationManager.AppSettings["line_cd"];
        readonly string process_cd = ConfigurationManager.AppSettings["process_cd"];
        readonly string inspect_cd = ConfigurationManager.AppSettings["inspect_cd"];
        readonly string datatype_id = ConfigurationManager.AppSettings["datatype_id"];

        readonly string postUrl = ConfigurationManager.AppSettings["postUrl"];

        public Main()
        {
            InitializeComponent();
            Text += "_" + Application.ProductVersion.ToString();
            //根据配置文件打开天平-串口
            try
            {
                sptWeight.PortName = ConfigurationManager.AppSettings["PortName"];
                sptWeight.BaudRate = int.Parse(ConfigurationManager.AppSettings["BaudRate"].ToString());
                Parity parity = Parity.None;
                switch (ConfigurationManager.AppSettings["Parity"])
                {
                    case "偶":
                        parity = Parity.Even;
                        break;
                    case "奇":
                        parity = Parity.Odd;
                        break;
                    case "无":
                        parity = Parity.None;
                        break;
                    case "标记":
                        parity = Parity.Mark;
                        break;
                    case "空格":
                        parity = Parity.Space;
                        break;
                }
                sptWeight.Parity = parity;
                sptWeight.DataBits = int.Parse(ConfigurationManager.AppSettings["DataBits"].ToString());
                StopBits stopBits = StopBits.One;
                switch (ConfigurationManager.AppSettings["StopBits"])
                {
                    case "1":
                        stopBits = StopBits.One;
                        break;
                    case "1.5":
                        stopBits = StopBits.OnePointFive;
                        break;
                    case "2":
                        stopBits = StopBits.Two;
                        break;
                }
                sptWeight.StopBits = stopBits;
                sptWeight.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "天平（串口）设置：", MessageBoxButtons.OK,MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //根据配置文件是否打开扫描器-以太网
            bool ScannerSwitch = ConfigurationManager.AppSettings["ScannerSwitch"] == "1" ? true : false;
            if (ScannerSwitch)
                ThreadPool.QueueUserWorkItem(h => ScannerReceive());
        }

        private void SptWeight_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (!getWeightSwitch)
                return;
            byte[] readBuffer = new byte[sptWeight.BytesToRead];
            sptWeight.Read(readBuffer, 0, readBuffer.Length);
            string indata = Encoding.Default.GetString(readBuffer);
            //MessageShow(ChkTest_ShowMessage.Checked, "串口1接收到" + readBuffer.Length + "字节Byte:\r\n" + indata);

            string identifier = "\r\n";
            if (!indata.Contains(identifier))//接收到的字符串中，是否存在有效值（分隔符）
            {
                portLastStr += indata;
                return;
            }
            else
            {
                indata = portLastStr + indata;
                int temp = indata.LastIndexOf('\r');
                portLastStr = indata.Substring(temp + 2);
                indata = indata.Substring(0, temp);

                string[] SN = indata.Split(new string[] { identifier }, StringSplitOptions.RemoveEmptyEntries);
                Array.Reverse(SN);
                getWeightSwitch = false;
                foreach (string str in SN)
                {
                    //H  +  0.5364 g[CR][LF]
                    if (str.Contains("H  +") && str.Substring(str.Length - 1) == "g")
                    {
                        try//防止串口乱码等
                        {
                            weight3 = Convert.ToDouble(str.Replace("H  +", "").Replace("g", ""));
                            noWeight = false;
                        }
                        catch { }
                        return;
                    }
                }
            }
            noWeight = true;
        }

        void ScannerReceive()
        {
            //异常"在创建窗口句柄之前，不能在控件上调用Invoke或Begininvoke。"
            //方法1)在创建窗口之后加上线程
            //方法2）Thread.Sleep(100);延迟一段窗口开启的时间
            try
            {
                TcpClient tcp = new TcpClient(Hostname, Port);
                using (var r = new StreamReader(tcp.GetStream()))
                {
                    while (!r.EndOfStream)
                    {
                        //var message = r.ReadToEnd();
                        var message = r.ReadLine();

                        this.Invoke(new Action(() =>
                        {
                            #region 测试（删除）
                            //if (message == "1009465K08WHB0005$1009465HBD0005-03$613-09907-17$N/A$555555$NSTD$N/A$N/A$N/A$N/A$WIP_AFTER_HOTBAR_NTRS$N/A$1000$OPERATOR:20170901002")
                            //{ message = "HRD9087-001D9GK09-DCCI1600A"; }
                            #endregion
                            if (message == LastSN)
                                return;
                            Action(message);
                        }));
                        //break;//防止读取"SN\r\nSN\r\nSN\r\nSN\r\n{设置lastSN的值}SN\r\nSN\r\nSN\r\n"
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "扫描器（以太网）设置：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private void TxtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                Action(txtSN.Text);
            }
        }

        void Action(string sn)
        {
            #region 界面初始化
            txtSN.Text = sn;
            txtSN.SelectAll();
            txtPostBody.Text = "";
            txtResult.Text = "";
            lblResult.Visible = false;
            if (sn == "")
            {
                txtMessage.Text = "SN不能为空";
                txtMessage.ForeColor = Color.Red;
                return;
            }
            else
            {
                txtMessage.Text = "";
                txtMessage.ForeColor = Color.Black;
            }
            Application.DoEvents();
            #endregion

            #region 等待N秒获取天平的重量（串口不断发送数据）
            Thread.Sleep(waitWeightTime);
            sptWeight.Close();
            getWeightSwitch = true;
            noWeight = true;
            sptWeight.Open();
            Thread.Sleep(waitAnalysisTime);//等待一段时间，让串口有足够时间赋值
            #region 测试(注释)
            //noWeight = false;
            //weight3 = 200;
            #endregion
            if (noWeight)
            {
                //MessageBox.Show("获取不了合适的天平重量（g），请重试", "天平：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMessage.Text = "天平：\r\n获取不了合适的天平重量（g），请重试";
                txtMessage.ForeColor = Color.Red;
                return;
            }
            #endregion

            txtLastSN.Text = sn;
            
            #region 查找pqm数据库该sn的重量
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(
$@"SELECT dd.inspect_text
FROM t_insp_{DB.Helper.DBremark} AS ii
LEFT JOIN m_process AS pp ON pp.proc_uuid = ii.proc_uuid
LEFT JOIN t_data_{DB.Helper.DBremark} AS dd ON dd.insp_seq = ii.insp_seq
WHERE ii.serial_cd = '{sn}'");
            //选择性增加条件
            if (datatype_id != "N/A")
                sql.AppendLine($"AND ii.datatype_id='{datatype_id}'");
            if (site_cd != "N/A")
                sql.AppendLine($"AND pp.site_cd='{site_cd}'");
            if (factory_cd != "N/A")
                sql.AppendLine($"AND pp.factory_cd='{factory_cd}'");
            if (line_cd != "N/A")
                sql.AppendLine($"AND pp.line_cd = '{line_cd}'");
            if (process_cd != "N/A")
                sql.AppendLine($"AND pp.process_cd = '{process_cd}'");
            if (inspect_cd != "N/A")
                sql.AppendLine($"AND dd.inspect_cd ='{inspect_cd}'");

            sql.Append("ORDER BY ii.process_at DESC LIMIT 1");
            DataTable dt = new DataTable();
            new WeightTester.DB.Helper().ExecuteDataTable(sql.ToString(), ref dt);
            
            double weight1;
            if (dt.Rows.Count != 0) { weight1 = Convert.ToDouble(dt.Rows[0]["inspect_text"]); }
            else
            {
                txtMessage.Text = "数据库：\r\n根据条件，该SN获取数据库的重量失败";
                txtMessage.ForeColor = Color.Red;
                return;
            }
            #endregion

            #region 上传重量3、重量2、设备机器号
            int saveDigit = MaxSaveDigit(weight1, weight3);
            double weight2 = Math.Round(weight3 - weight1, saveDigit);//防止计算异常：1-0.9=0.099999999999999978
            string postBody = WeightTester.Json.CSV2PostBody.PostBody(sn,weight3,weight2);
            try
            {
                JObject jo = JObject.Parse(postBody);
                txtPostBody.Text = jo.ToString();
            }
            catch { txtPostBody.Text = postBody; }

            string contentType = "application/json";
            string method = "POST";
            string result = new API().HttpResponse(postUrl, postBody, contentType, method);
            try
            {
                JObject jo = JObject.Parse(result);
                txtResult.Text = jo.ToString();
            }
            catch { txtResult.Text = result; }

            JsonTextReader reader = new JsonTextReader(new StringReader(result));
            JObject J = (JObject)JToken.ReadFrom(reader);
            WeightTester.Json.ResultJson resultJson = JsonConvert.DeserializeObject<WeightTester.Json.ResultJson>(J.ToString());
            if (resultJson.status != "OK")
            {
                txtResult.ForeColor = Color.Red;//txtResult属性不能只读，否则颜色一直是黑色
            }
            else
            {
                txtResult.ForeColor = Color.Black;
            }
            #endregion

            #region 显示
            StringBuilder display = new StringBuilder();
            display.AppendLine($"数据库获取重量1：{weight1}g");
            display.AppendLine($"上传天平的重量3：{weight3}g");
            display.AppendLine($"上传相差的重量 ：{weight2}g");
            txtMessage.Text = display.ToString();

            #region 最终显示是否在上限内判定
            JsonTextReader reader_2 = new JsonTextReader(new StringReader(postBody));
            JObject J_2 = (JObject)JToken.ReadFrom(reader_2);
            WeightTester.Json.PostJson postJson = JsonConvert.DeserializeObject<WeightTester.Json.PostJson>(J_2.ToString());
            lblResult.Visible = true;
            lblResult.Text = postJson.sendResultDetails.test_attributes.total_judge == "0" ? "OK" : "NG";
            if (lblResult.Text != "OK")
            {
                lblResult.BackColor = Color.Red;
            }
            else
            {
                lblResult.BackColor = Color.Green;
            }
            #endregion
            #endregion
        }

        /// <summary>
        /// 返回2个数中的最大小数点位数
        /// </summary>
        /// <param name="double1">称重1</param>
        /// <param name="double2">称重3</param>
        /// <returns></returns>
        int MaxSaveDigit(double double1, double double2)
        {
            //int result = weight1.ToString().Length - weight1.ToString().IndexOf('.') - 1;
            string[] strArr1 = double1.ToString().Split('.');
            string[] strArr2 = double2.ToString().Split('.');
            if (strArr1.Length == 2 || strArr2.Length == 2)
            {
                int digit1 = 0, digit2 = 0;
                try { digit1 = strArr1[1].Length; } catch { }
                try { digit2 = strArr2[1].Length; } catch { }
                return digit1 > digit2 ? digit1 : digit2;
            }
            else { return 0; }
        }
    }
}
