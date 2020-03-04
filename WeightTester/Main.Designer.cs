namespace WeightTester
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.sptWeight = new System.IO.Ports.SerialPort(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSN = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.txtPostBody = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLastSN = new System.Windows.Forms.TextBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblDetail = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sptWeight
            // 
            this.sptWeight.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SptWeight_DataReceived);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "SN:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Message:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "PostString:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 399);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "API Result:";
            // 
            // txtSN
            // 
            this.txtSN.Location = new System.Drawing.Point(95, 49);
            this.txtSN.Name = "txtSN";
            this.txtSN.Size = new System.Drawing.Size(250, 21);
            this.txtSN.TabIndex = 10;
            this.txtSN.Text = "GH912345678912346";
            this.txtSN.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtSN_KeyDown);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(95, 87);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(250, 96);
            this.txtMessage.TabIndex = 13;
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(95, 396);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(500, 74);
            this.txtResult.TabIndex = 15;
            // 
            // txtPostBody
            // 
            this.txtPostBody.Location = new System.Drawing.Point(95, 200);
            this.txtPostBody.Multiline = true;
            this.txtPostBody.Name = "txtPostBody";
            this.txtPostBody.ReadOnly = true;
            this.txtPostBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPostBody.Size = new System.Drawing.Size(500, 179);
            this.txtPostBody.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "Last SN:";
            // 
            // txtLastSN
            // 
            this.txtLastSN.Location = new System.Drawing.Point(95, 18);
            this.txtLastSN.Name = "txtLastSN";
            this.txtLastSN.ReadOnly = true;
            this.txtLastSN.Size = new System.Drawing.Size(250, 21);
            this.txtLastSN.TabIndex = 17;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("宋体", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblResult.Location = new System.Drawing.Point(366, 5);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(0, 97);
            this.lblResult.TabIndex = 18;
            // 
            // lblDetail
            // 
            this.lblDetail.AutoSize = true;
            this.lblDetail.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDetail.ForeColor = System.Drawing.Color.Red;
            this.lblDetail.Location = new System.Drawing.Point(355, 114);
            this.lblDetail.Name = "lblDetail";
            this.lblDetail.Size = new System.Drawing.Size(0, 16);
            this.lblDetail.TabIndex = 19;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 487);
            this.Controls.Add(this.lblDetail);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.txtLastSN);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtPostBody);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.txtSN);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Main";
            this.Text = "WeightTester";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort sptWeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSN;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.TextBox txtPostBody;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLastSN;
        private System.Windows.Forms.Label lblResult;
        public System.Windows.Forms.Label lblDetail;
    }
}

