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
        string lastSN { get { return txtLastSN.Text; } }
        readonly string Hostname = ConfigurationManager.AppSettings["Hostname"];
        readonly int Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
        //数据库属性
        string UUID;
        readonly string site_cd = ConfigurationManager.AppSettings["site_cd"];
        readonly string factory_cd = ConfigurationManager.AppSettings["factory_cd"];
        readonly string line_cd = ConfigurationManager.AppSettings["line_cd"];
        readonly string process_cd = ConfigurationManager.AppSettings["process_cd"];
        readonly string inspect_cd = ConfigurationManager.AppSettings["inspect_cd"];

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
            #region 获取UUID
            string sql = 
$@"SELECT proc_uuid
FROM m_process 
WHERE site_cd='{site_cd}'
AND factory_cd='{factory_cd}'
AND line_cd='{line_cd}'
AND process_cd ='{process_cd}'";
            DataTable dt = new DataTable();
            new WeightTester.DB.Helper().ExecuteDataTable(sql,ref dt);
            if (dt.Rows.Count != 1)
            {
                MessageBox.Show("根据配置文件属性（site_cd，factory_cd，line_cd，process_cd），获取数据库的UUID失败", "数据库：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show("Failed to get the UUID of the database according to the configuration file attributes (site_cd, factory_cd, line_cd, process_cd)", "Database:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            else
            {
                UUID=dt.Rows[0]["proc_uuid"].ToString();
            }
            #endregion
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //根据配置文件打开扫描器-以太网
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
                            if (message == lastSN)
                                return;
                            Action(message);
                        }));
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
            #region 等待N秒获取天平的重量（串口不断发送数据）
            txtSN.Text = sn;
            txtSN.SelectAll();
            txtMessage.Text = "";
            txtMessage.ForeColor = Color.Black;
            txtPostBody.Text = "";
            txtResult.Text = "";
            lblResult.Visible = false;
            Application.DoEvents();

            Thread.Sleep(waitWeightTime);
            sptWeight.Close();
            getWeightSwitch = true;
            noWeight = true;
            sptWeight.Open();
            Thread.Sleep(waitAnalysisTime);//等待一段时间，让串口有足够时间赋值
            #region 测试(注释)
            //noWeight = false;
            //weight3 = 22.3431;
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
            string sql =
$@"SELECT inspect_text
FROM t_data_vc
WHERE judge_text='0'
AND inspect_cd = '{inspect_cd}'
AND insp_seq=(
	SELECT MAX(insp_seq)
	FROM t_insp_vc
	WHERE proc_uuid='{UUID}'
    AND serial_cd='{sn}')";
            DataTable dt = new DataTable();
            new WeightTester.DB.Helper().ExecuteDataTable(sql, ref dt);
            
            double weight1 = 0;
            if (dt.Rows.Count != 0) { weight1 = Convert.ToDouble(dt.Rows[0]["inspect_text"]); }
            else
            {
                //MessageBox.Show("根据SN，获取数据库的重量失败", "数据库：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMessage.Text = "数据库：\r\n根据SN，获取数据库的重量失败";
                txtMessage.ForeColor = Color.Red;
                return;
            }
            #endregion

            #region 上传重量3、重量2、设备机器号
            double weight2 = weight3 - weight1;
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
            lblResult.Visible = true;
            lblResult.Text = resultJson.status;
            if (resultJson.status != "OK")
            {
                txtResult.ForeColor = Color.Red;//txtResult属性不能只读，否则颜色一直是黑色
                lblResult.BackColor = Color.Red;
            }
            else
            {
                txtResult.ForeColor = Color.Black;
                lblResult.BackColor = Color.Green;
            }
            #endregion

            #region 显示
            StringBuilder display = new StringBuilder();
            display.AppendLine($"数据库获取重量1：{weight1}g");
            display.AppendLine($"上传天平的重量3：{weight3}g");
            display.AppendLine($"上传相差的重量 ：{weight2}g");
            txtMessage.Text = display.ToString();
            #endregion
        }
    }
}
