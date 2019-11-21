using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Peak.Can.Basic;

class Element
{
    public Element(string str, uint value) 
    {
        Str = str;
        Value = value;
    }
    public string Str { get; }
    public uint Value { get; }
}

namespace HCAN
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            var v = new { Str = "250 Kb/s", Value = 250 };
            
            this.comboBox1.Items.Add(v);
            this.comboBox1.DisplayMember = "Str";
        }
    }
}


    

