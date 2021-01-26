using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComputerRetard_control
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
            this.Left = -this.Width - 1;
        }
        private void Start_Shown(object sender, EventArgs e)
        {
            this.Hide();
            if (Program.Start_Args.Length == 1)
            {
                switch (Program.Start_Args[0])
                {
                    case "/keeper":
                        Program_keeper_exec_class.Keeper_Start();
                        break;
                    case "/lbc":
                        Program_exec_class.ME_Start(true);
                        break;
                    default:
                        Program_exec_class.ME_Start(false);
                        break;
                }
            }
            else
            {
                Program_exec_class.ME_Start(false);
            }
        }
    }
}
