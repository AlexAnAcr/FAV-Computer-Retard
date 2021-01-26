using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ComputerRetard_control
{
    static class Program
    {
        
        public static string[] Start_Args;
        [STAThread]
        static void Main(string[] args)
        {
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal;
            Start_Args = args;
            Application.Run(new Start());
        }
        /// <summary>
        /// Завершает работу программы
        /// </summary>
        public static void Exit()
        {
            Application.Exit();
            //This lines is not executed
        }
    }
}
