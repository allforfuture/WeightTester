namespace WeightTester
{
    partial class Port
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CmbStopBits1 = new System.Windows.Forms.ComboBox();
            this.CmbDataBits1 = new System.Windows.Forms.ComboBox();
            this.CmbParity1 = new System.Windows.Forms.ComboBox();
            this.CmbBaudRate1 = new System.Windows.Forms.ComboBox();
            this.CmbPort = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.TxtIdentifier = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CmbStopBits1
            // 
            this.CmbStopBits1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbStopBits1.FormattingEnabled = true;
            this.CmbStopBits1.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.CmbStopBits1.Location = new System.Drawing.Point(153, 145);
            this.CmbStopBits1.Name = "CmbStopBits1";
            this.CmbStopBits1.Size = new System.Drawing.Size(121, 20);
            this.CmbStopBits1.TabIndex = 42;
            // 
            // CmbDataBits1
            // 
            this.CmbDataBits1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbDataBits1.FormattingEnabled = true;
            this.CmbDataBits1.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.CmbDataBits1.Location = new System.Drawing.Point(153, 110);
            this.CmbDataBits1.Name = "CmbDataBits1";
            this.CmbDataBits1.Size = new System.Drawing.Size(121, 20);
            this.CmbDataBits1.TabIndex = 43;
            // 
            // CmbParity1
            // 
            this.CmbParity1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbParity1.FormattingEnabled = true;
            this.CmbParity1.Items.AddRange(new object[] {
            "偶",
            "奇",
            "无",
            "标记",
            "空格"});
            this.CmbParity1.Location = new System.Drawing.Point(153, 84);
            this.CmbParity1.Name = "CmbParity1";
            this.CmbParity1.Size = new System.Drawing.Size(121, 20);
            this.CmbParity1.TabIndex = 44;
            // 
            // CmbBaudRate1
            // 
            this.CmbBaudRate1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBaudRate1.FormattingEnabled = true;
            this.CmbBaudRate1.Items.AddRange(new object[] {
            "9600",
            "14400",
            "19200",
            "38400",
            "57600",
            "115200",
            "128000"});
            this.CmbBaudRate1.Location = new System.Drawing.Point(153, 58);
            this.CmbBaudRate1.Name = "CmbBaudRate1";
            this.CmbBaudRate1.Size = new System.Drawing.Size(121, 20);
            this.CmbBaudRate1.TabIndex = 41;
            // 
            // CmbPortName1
            // 
            this.CmbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbPort.FormattingEnabled = true;
            this.CmbPort.Location = new System.Drawing.Point(153, 32);
            this.CmbPort.Name = "CmbPortName1";
            this.CmbPort.Size = new System.Drawing.Size(121, 20);
            this.CmbPort.TabIndex = 40;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 148);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 39;
            this.label5.Text = "StopBits停止位：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 38;
            this.label4.Text = "DataBits数据位：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 12);
            this.label3.TabIndex = 37;
            this.label3.Text = "Parity奇偶校验位：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 36;
            this.label2.Text = "BaudRate波特率：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 34;
            this.label1.Text = "PortName端口名：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(328, 34);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 46;
            this.label11.Text = "分隔符：";
            // 
            // TxtIdentifier
            // 
            this.TxtIdentifier.Location = new System.Drawing.Point(319, 49);
            this.TxtIdentifier.Multiline = true;
            this.TxtIdentifier.Name = "TxtIdentifier";
            this.TxtIdentifier.Size = new System.Drawing.Size(100, 42);
            this.TxtIdentifier.TabIndex = 45;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(330, 102);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 70);
            this.btnSave.TabIndex = 35;
            this.btnSave.Text = "保存设置";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // Port
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 191);
            this.Controls.Add(this.CmbStopBits1);
            this.Controls.Add(this.CmbDataBits1);
            this.Controls.Add(this.CmbParity1);
            this.Controls.Add(this.CmbBaudRate1);
            this.Controls.Add(this.CmbPort);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.TxtIdentifier);
            this.Controls.Add(this.btnSave);
            this.Name = "Port";
            this.Text = "Port";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CmbStopBits1;
        private System.Windows.Forms.ComboBox CmbDataBits1;
        private System.Windows.Forms.ComboBox CmbParity1;
        private System.Windows.Forms.ComboBox CmbBaudRate1;
        private System.Windows.Forms.ComboBox CmbPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TxtIdentifier;
        private System.Windows.Forms.Button btnSave;
    }
}