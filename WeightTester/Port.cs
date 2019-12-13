using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeightTester
{
    public partial class Port : Form
    {
        public Port()
        {
            InitializeComponent();
            CmbPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
        }
    }
}
