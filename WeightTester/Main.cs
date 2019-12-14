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

namespace WeightTester
{
    public partial class Main : Form
    {
        readonly string Hostname = ConfigurationManager.AppSettings["Hostname"];
        readonly int Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
        string UUID_1, UUID_2_3;
        readonly string machine_cd = ConfigurationManager.AppSettings["machine_cd"];
        readonly string datatype_id = ConfigurationManager.AppSettings["datatype_id"];
        readonly string site_cd = ConfigurationManager.AppSettings["site_cd"];
        readonly string factory_cd = ConfigurationManager.AppSettings["factory_cd"];
        readonly string line_cd = ConfigurationManager.AppSettings["line_cd"];
        readonly string process_cd_1 = ConfigurationManager.AppSettings["process_cd_1"];
        readonly string process_cd_2_3 = ConfigurationManager.AppSettings["process_cd_2_3"];

        readonly string inspect_cd1 = ConfigurationManager.AppSettings["inspect_cd1"];
        readonly string inspect_cd2 = ConfigurationManager.AppSettings["inspect_cd2"];
        readonly double upper_text2 = double.Parse(ConfigurationManager.AppSettings["upper_text2"]);
        readonly double lower_text2 = double.Parse(ConfigurationManager.AppSettings["lower_text2"]);
        readonly string inspect_cd3 = ConfigurationManager.AppSettings["inspect_cd3"];
        readonly double upper_text3 = double.Parse(ConfigurationManager.AppSettings["upper_text3"]);
        readonly double lower_text3 = double.Parse(ConfigurationManager.AppSettings["lower_text3"]);

        public Main()
        {
            InitializeComponent();
            //根据配置文件打开扫描器-以太网
            //ThreadPool.QueueUserWorkItem(h => ScannerReceive());
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
$@"SELECT process_cd,proc_uuid
FROM m_process 
WHERE site_cd='{site_cd}'
AND factory_cd='{factory_cd}'
AND line_cd='{line_cd}'
AND process_cd in('{process_cd_1}','{process_cd_2_3}')";
            DataTable dt = new DataTable();
            new WeightTester.DB.Helper().ExecuteDataTable(sql,ref dt);
            if (dt.Rows.Count != 2)
            {
                MessageBox.Show("根据配置文件属性（site_cd，factory_cd，line_cd，process_cd），获取数据库的UUID失败", "数据库：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["process_cd"].ToString() == process_cd_1)
                    {
                        UUID_1 = dr["proc_uuid"].ToString();
                    }
                    else if (dr["process_cd"].ToString() == process_cd_2_3)
                    {
                        UUID_2_3 = dr["proc_uuid"].ToString();
                    }
                }
            }
            #endregion
        }

        private void TxtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                txtSN.SelectAll();
                Action(txtSN.Text);
            }
        }

        void Action(string sn)
        {
            #region 等待4秒获取天平的重量（串口不断发送数据）
            double weight3 = 18.111;
            #endregion

            #region 查找pqm数据库该sn的重量
            string sql =
$@"SELECT inspect_text
FROM t_data_vc
WHERE judge_text='0'
AND inspect_cd = '{inspect_cd1}'
AND insp_seq=(
	SELECT MAX(insp_seq)
	FROM t_insp_vc
	WHERE proc_uuid='{UUID_1}'
    AND serial_cd='{sn}')";
            DataTable dt = new DataTable();
            new WeightTester.DB.Helper().ExecuteDataTable(sql, ref dt);


            double weight1 = 0;
            if (dt.Rows.Count != 0) { weight1 = Convert.ToDouble(dt.Rows[0]["inspect_text"]); }
            else
            {
                MessageBox.Show("根据SN，获取数据库的重量失败", "数据库：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //数据库找不到重量的操作，显示红色什么的
                return;
            }
            #endregion

            #region 上传重量3、重量2、设备机器号
            double weight2 = weight3 - weight1;
            insertDB(inspect_cd3, upper_text3, lower_text3, weight3);
            insertDB(inspect_cd2, upper_text2, lower_text2, weight2);
            #endregion

            #region 显示
            StringBuilder display = new StringBuilder();
            display.AppendLine($"数据库获取重量1：{weight1}g");
            display.AppendLine($"上传天平的重量3：{weight3}g");
            display.AppendLine($"上传相差的重量 ：{weight2}g");
            txtResult.Text = display.ToString();
            #endregion
        }






        string weightLastStr = "";
        private void SptWeight_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            byte[] readBuffer = new byte[sptWeight.BytesToRead];
            sptWeight.Read(readBuffer, 0, readBuffer.Length);
            string indata = Encoding.Default.GetString(readBuffer);
            //MessageShow(ChkTest_ShowMessage.Checked, "串口1接收到" + readBuffer.Length + "字节Byte:\r\n" + indata);

            string identifier = "\r";
            if (indata.Substring(indata.Length - 1, 1) != identifier)
            {
                weightLastStr += indata;
                return;
            }
            indata = weightLastStr + indata;
            weightLastStr = "";
            string[] SN = indata.Split(new string[] { identifier }, StringSplitOptions.RemoveEmptyEntries);
            //foreach (string str in SN)
            //{
            //    string sn = str.Replace("\r", "").Replace("\n", "");
            //    if (sn == "") continue;
            //    if (sn != "ERROR" && !SN.Contains("+") && sn.Length != 17)
            //    {
            //        Log.WriteError(sn);
            //    }
            //    this.Invoke(new Action(() =>
            //    {
            //        //就是textbox输入
            //        Action(sn);
            //    }));
            //}
        }

        void ScannerReceive()
        {
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
                            //Action(sn);
                            txtSN.Text = message;
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

        
        
        private void button1_Click(object sender, EventArgs e)
        {
            double weight3 = 19.812;
            insertDB(inspect_cd3, upper_text3, lower_text3, weight3);
        }

        void insertDB(string inspect_cd, double upper_text, double lower_text,double weight)
        {
            string judge_text = weight <= upper_text && weight >= lower_text ? "0" : "1";
            List<string> sql = new List<string>();
            sql.Add(
$@"INSERT INTO t_insp_vc
(insp_seq,updated_at,process_at,proc_uuid,work_cd,machine_cd,serial_cd,lot_cd,mo_cd,tag_id,datatype_id,judge_text,status_text,remarks_text)
VALUES
(NEXTVAL('t_insp_vc_insp_seq_seq'),NOW(),NOW(),
'{UUID_2_3}','1', '{machine_cd}',
'{txtSN.Text}','','','',
'{datatype_id}','0','','')
RETURNING insp_seq");

            sql.Add(
$@"INSERT INTO t_data_vc
(insp_seq,process_at,inspect_cd,inspect_teXt,judge_text,upper_text,lower_text,status_text)
VALUES(@@,NOW(),'{inspect_cd}',{weight},'{judge_text}','{upper_text}','{lower_text}','')");
            new WeightTester.DB.Helper().TestTran(sql);
        }
    }
}
