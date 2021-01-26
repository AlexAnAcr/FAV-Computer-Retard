using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ComputerRetard_control
{
   using forms = System.Windows.Forms;
   using io = System.IO;
    public class Basic_Func
    {
        public static string APP_PATH = forms.Application.StartupPath;
        public static string USER_AUTORUN =Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        public delegate void Timer_Elapse_Func(object sender, EventArgs e);
        public static void Timer_Initialise(System.Timers.Timer timer, Timer_Elapse_Func func, uint interval, bool start)
        {
            timer.Elapsed += new System.Timers.ElapsedEventHandler(func);
            timer.Interval = interval;
            timer.Enabled = start;
        }
        public static System.Collections.Generic.List<Config_entry> Analyse_config(string[] config_text)
        {
            System.Collections.Generic.List<Config_entry> config = new System.Collections.Generic.List<Config_entry>();
            for (ushort i = 0; i < config_text.Length; i++)
            {
                if (config_text[i] != "")
                {
                    if (config_text[i].TrimStart(' ').IndexOf(';') != 0)
                    {
                        string[] parts = config_text[i].Split('=');
                        if (parts.Length == 2)
                        {
                            config.Add(new Config_entry(parts[0], parts[1]));
                        }
                    }
                }
            }
            return config;
        }
        public static class ShortCut {
            public static void Create(string PathToFile, string PathToLink, string icon_way) {
                string way; int index;
                if (icon_way.IndexOf(',') > -1) {
                    way = icon_way.Substring(0, icon_way.LastIndexOf(','));
                    if (!int.TryParse(icon_way.Substring(icon_way.LastIndexOf(icon_way) + 1), out index)) {
                        index = 0;
                    }
                } else {
                    way = icon_way;
                    index = 0;
                }
                ShellLink.IShellLinkW shlLink = ShellLink.CreateShellLink();
                Marshal.ThrowExceptionForHR(shlLink.SetPath(PathToFile));
                Marshal.ThrowExceptionForHR(shlLink.SetIconLocation(way, index));
                ((System.Runtime.InteropServices.ComTypes.IPersistFile)shlLink).Save(PathToLink, false);
            }
        }
       public static void Write_pkd(int id1, int id2, int id3)
        {
            if (io.File.Exists(APP_PATH + "\\pkd.dat"))
            {
                string[] data = io.File.ReadAllLines(APP_PATH + "\\pkd.dat");
                if (data.Length != 3)
                {
                    Array.Resize(ref data, 3);
                }
                if (id1 != -1)
                {
                    data[0] = id1.ToString();
                }
                if (id2 != -1)
                {
                    data[1] = id2.ToString();
                }
                if (id3 != -1)
                {
                    data[2] = id3.ToString();
                }
                io.File.WriteAllLines(APP_PATH + "\\pkd.dat", data);
            }
            else
            {
                string[] data = { "none", "none", "none" };
                if (id1 != -1)
                {
                    data[0] = id1.ToString();
                }
                if (id2 != -1)
                {
                    data[1] = id2.ToString();
                }
                if (id3 != -1)
                {
                    data[2] = id3.ToString();
                }
                io.File.WriteAllLines(APP_PATH + "\\pkd.dat", data);
            }
        }
        public class Config_entry
        {
            public Config_entry(string key = "", string value = "")
            {
                this.key = key; this.value = value;
            }
            public string key, value;
        }
       static class ShellLink
        {
            [ComImport, Guid("000214F9-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            internal interface IShellLinkW
            {
                [PreserveSig]
                int GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cch, ref IntPtr pfd, uint fFlags);

                [PreserveSig]
                int GetIDList(out IntPtr ppidl);

                [PreserveSig]
                int SetIDList(IntPtr pidl);

                [PreserveSig]
                int GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)]StringBuilder pszName, int cch);

                [PreserveSig]
                int SetDescription([MarshalAs(UnmanagedType.LPWStr)]string pszName);

                [PreserveSig]
                int GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)]StringBuilder pszDir, int cch);

                [PreserveSig]
                int SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)]string pszDir);

                [PreserveSig]
                int GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)]StringBuilder pszArgs, int cch);

                [PreserveSig]
                int SetArguments([MarshalAs(UnmanagedType.LPWStr)]string pszArgs);
                [PreserveSig]
                int GetHotkey(out ushort pwHotkey);

                [PreserveSig]
                int SetHotkey(ushort wHotkey);

                [PreserveSig]
                int GetShowCmd(out int piShowCmd);

                [PreserveSig]
                int SetShowCmd(int iShowCmd);

                [PreserveSig]
                int GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)]StringBuilder pszIconPath, int cch, out int piIcon);

                [PreserveSig]
                int SetIconLocation([MarshalAs(UnmanagedType.LPWStr)]string pszIconPath, int iIcon);

                [PreserveSig]
                int SetRelativePath([MarshalAs(UnmanagedType.LPWStr)]string pszPathRel, uint dwReserved);

                [PreserveSig]
                int Resolve(IntPtr hwnd, uint fFlags);

                [PreserveSig]
                int SetPath([MarshalAs(UnmanagedType.LPWStr)]string pszFile);
            }

            [ComImport, Guid("00021401-0000-0000-C000-000000000046"), ClassInterface(ClassInterfaceType.None)]
            private class shl_link { }

            internal static IShellLinkW CreateShellLink()
            {
                return (IShellLinkW)(new shl_link());
            }
        }
    }
    public static class Program_exec_class {
        public static System.Diagnostics.Process meKeeper_process = new System.Diagnostics.Process(), CR_process = new System.Diagnostics.Process(), my_process = new System.Diagnostics.Process();
        public static System.Timers.Timer main_timer;
        static string cr_location, me_name, shortcut_name, shortcut_icon; static string[] proc_black_list = new string[0]; static byte crl_mode, crl_add_mode; static ushort crl_end_num; static bool wait_file_exist;
        public static void ME_Start(bool launched = false) { //Main programm
            if (io.File.Exists(Basic_Func.APP_PATH + "\\config.ini")) {
                string []config_text = io.File.ReadAllLines(Basic_Func.APP_PATH + "\\config.ini");
                System.Collections.Generic.List<Basic_Func.Config_entry> config = Basic_Func.Analyse_config(config_text);
                if (config.Count == 0) {
                    Set_default_config_values();
                } else {
                    bool[] find = { false, false, false, false, false, false, false};
                    for (ushort i = 0; i < config.Count; i++) {
                        if (config[i].key == "ComputerRetardLocation" && config[i].value != "" && find[0] == false) {
                            cr_location = config[i].value;
                            find[0] = true;
                        } else if (config[i].key == "MyName" && config[i].value != "" && find[1] == false) {
                            me_name = config[i].value;
                            find[1] = true;
                        } else if (config[i].key == "SCName" && config[i].value != "" && find[2] == false) {
                            shortcut_name = config[i].value;
                            find[2] = true;
                        } else if (config[i].key == "SCIcon" && config[i].value != "" && find[3] == false) {
                            shortcut_icon = config[i].value;
                            find[3] = true;
                        } else if (config[i].key == "CRMode" && config[i].value != "" && find[4] == false) {
                            find[4] = true;
                            switch (config[i].value) {
                                case "wait_startMem": //ожидать перед запуском однажды
                                    crl_mode = 1;
                                    break;
                                case "wait_start": //ожидать перед запуском каждый раз
                                    crl_mode = 2;
                                    break;
                                case "wait_activDeactiv": //активировать и деактивировать
                                    crl_mode = 3;
                                    break;
                                default: //сразу запустить
                                    crl_mode = 0;
                                    break;
                            }
                        } else if (config[i].key == "TimeCounterMode" && config[i].value != "" && find[5] == false) {
                            switch (config[i].value) {
                                case "AddDec": //Добавлять и убавлять
                                    crl_add_mode = 1;
                                    find[5] = true;
                                    break;
                                case "AddOnly": //Только убавлять
                                    crl_add_mode = 2;
                                    find[5] = true;
                                    break;
                            }
                        }  else if (config[i].key == "TimeCounterInterval" && config[i].value != "" && find[6] == false) {
                            if (ushort.TryParse(config[i].value,out crl_end_num)) {
                                find[6] = true;
                            }
                        }
                    }
                    if (find[0] == false || find[1] == false || find[2] == false || find[4] == false || find[5] == false || find[6] == false) {
                        Set_default_config_values();
                    }
                    if (find[3] == false) {
                        shortcut_icon = Basic_Func.APP_PATH + "\\" + me_name + ".exe,0";
                    }
                    config.Clear();
                    PBL_analyse(config_text);
                    config_text = null;
                }
            } else {
                Set_default_config_values();
            }
            my_process = System.Diagnostics.Process.GetCurrentProcess();
            if (launched) {
                bool[] start = {false,false};
                int[] int_prog_id = new int[2];
                if (io.File.Exists(Basic_Func.APP_PATH + "\\pkd.dat")) { //1 line - Computer Retard Control, 2 line - Computer Retard Control Keeper, 3 line - Computer Retard
                    string[] proc_id = io.File.ReadAllLines(Basic_Func.APP_PATH + "\\pkd.dat");
                    if (proc_id.Length == 3) {
                        start[0] = int.TryParse(proc_id[1], out int_prog_id[0]);
                        start[1] = int.TryParse(proc_id[2], out int_prog_id[1]);
                    }
                }
                Basic_Func.Write_pkd(my_process.Id, -1, -1);
                if (start[0]) { //Computer Retard Control Keeper
                    try {
                        meKeeper_process = System.Diagnostics.Process.GetProcessById(int_prog_id[0]);
                    } catch {
                        Start_keeper_CR(1);
                        Basic_Func.Write_pkd(-1, meKeeper_process.Id, -1);
                    }
                } else { Program.Exit(); }
                if (start[1]) { //Computer Retard
                    try {
                        CR_process = System.Diagnostics.Process.GetProcessById(int_prog_id[1]);
                    } catch {
                        Start_keeper_CR(2);
                        Basic_Func.Write_pkd(-1, -1, CR_process.Id);
                    }
                } else { Program.Exit(); }
                if (crl_mode == 1) { wait_file_exist = true; open_CR = true; }
            } else {
                Basic_Func.Write_pkd(my_process.Id, -1, -1);
                Start_keeper_CR(1);
                Basic_Func.Write_pkd(-1, meKeeper_process.Id, -1);
                switch (crl_mode) {
                    case 0:
                        Start_keeper_CR(2);
                        Basic_Func.Write_pkd(-1, -1, CR_process.Id);
                        break;
                    case 1:
                        if (io.File.Exists(Basic_Func.APP_PATH + "\\waited.dat")) {
                            wait_file_exist = true;
                            open_CR = true;
                            Start_keeper_CR(2);
                            Basic_Func.Write_pkd(-1, -1, CR_process.Id);
                        } else {
                            wait_file_exist = false;
                        }
                        break;
                }
            }
            main_timer = new System.Timers.Timer();
            Basic_Func.Timer_Elapse_Func tef = main_timer_elapsed;
            Basic_Func.Timer_Initialise(main_timer, tef, 1000, true);
        }

        static byte timer_cicle = 1; static ushort user_wait = 0; static System.Drawing.Point last_mouse_position = Get_mouse_position(); static bool open_CR = false;
        private static void main_timer_elapsed(object sender, EventArgs e) {
            main_timer.Stop();
            if (!io.File.Exists(Basic_Func.USER_AUTORUN + "\\" + shortcut_name + ".lnk")) {
                Basic_Func.ShortCut.Create(Basic_Func.APP_PATH + "\\" + me_name + ".exe", Basic_Func.USER_AUTORUN + "\\" + shortcut_name + ".lnk", shortcut_icon);
            }
            if (crl_mode == 0)
            {
                if (!open_CR)
                {
                    open_CR = true;
                }
            }
            else if (crl_mode == 1)
            {
                if (!wait_file_exist)
                {
                    System.Drawing.Point temp = Get_mouse_position();
                    if (last_mouse_position.X == temp.X && last_mouse_position.Y == temp.Y && user_wait > 0)
                    {
                        if (crl_add_mode == 1) {user_wait -= 1;}
                    }
                    else
                    {
                        last_mouse_position = temp;
                        if (crl_add_mode == 1) {user_wait += 2;} else {user_wait += 1;}
                        if (user_wait >= crl_end_num)
                        {
                            io.File.WriteAllText(Basic_Func.APP_PATH + "\\waited.dat", "");
                            open_CR = true;
                        }
                    }
                }
            }
            else if (crl_mode == 2)
            {
                if (user_wait >= crl_end_num)
                {
                    if (!open_CR)
                    {
                        open_CR = true;
                    }
                }
                else
                {
                    System.Drawing.Point temp = Get_mouse_position();
                    if (last_mouse_position.X == temp.X && last_mouse_position.Y == temp.Y && user_wait > 0)
                    {
                        if (crl_add_mode == 1) { user_wait -= 1; }
                    }
                    else
                    {
                        last_mouse_position = temp;
                        if (crl_add_mode == 1) { user_wait += 2; } else { user_wait += 1; }
                    }
                }
            }
            else if (crl_mode == 3)
            {
                System.Drawing.Point temp = Get_mouse_position();
                if (last_mouse_position.X == temp.X && last_mouse_position.Y == temp.Y && user_wait > 0)
                {
                    user_wait -= 1;
                    if (user_wait == 0)
                    {
                        if (open_CR)
                        {
                            open_CR = false;
                        }
                        try {
                            System.Diagnostics.Process.GetProcessById(CR_process.Id);
                            CR_process.Kill();
                        } catch { }
                    }
                }
                else
                {
                    last_mouse_position = temp;
                    if (crl_add_mode == 1) { user_wait += 2; } else { user_wait += 4; }
                    if (user_wait >= crl_end_num)
                    {
                        if (!open_CR)
                        {
                            open_CR = true;
                        }
                    }
                }
            }
            System.Diagnostics.Process temp_me_proc1 = System.Diagnostics.Process.GetProcessById(my_process.Id);
            if (temp_me_proc1.PriorityClass != System.Diagnostics.ProcessPriorityClass.AboveNormal)
            {
                temp_me_proc1.PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal;
            }
            try {
                System.Diagnostics.Process temp_proc1 = System.Diagnostics.Process.GetProcessById(meKeeper_process.Id);
                if (temp_proc1.PriorityClass != System.Diagnostics.ProcessPriorityClass.AboveNormal)
                {
                    temp_proc1.PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal;
                }
            } catch {
                Start_keeper_CR(1);
                Basic_Func.Write_pkd(-1, meKeeper_process.Id, -1);
            }
            if (open_CR)
            {
                try {
                    System.Diagnostics.Process temp_proc1 = System.Diagnostics.Process.GetProcessById(CR_process.Id);
                    if (temp_proc1.PriorityClass != System.Diagnostics.ProcessPriorityClass.AboveNormal)
                    {
                        temp_proc1.PriorityClass = System.Diagnostics.ProcessPriorityClass.AboveNormal;
                    }
                } catch {
                    Start_keeper_CR(2);
                    Basic_Func.Write_pkd(-1, -1, CR_process.Id);
                }
            }

            if (timer_cicle % 5 == 0) {
                if (proc_black_list.Length > 0)
                {
                    foreach (string i in proc_black_list)
                    {
                        System.Diagnostics.Process[] temp_proc = System.Diagnostics.Process.GetProcessesByName(i);
                        if (temp_proc.Length > 0) { temp_proc[0].Kill(); }
                    }
                }
            }
            if (timer_cicle % 10 == 0) {
                if (io.File.Exists(Basic_Func.APP_PATH + "\\off.txt"))
                {
                    try {
                        System.Diagnostics.Process.GetProcessById(CR_process.Id);
                        CR_process.Kill();
                    } catch { }
                    try {
                        System.Diagnostics.Process.GetProcessById(meKeeper_process.Id);
                        meKeeper_process.Kill();
                    } catch { }
                    Program.Exit();
                }
            }
            if (timer_cicle == 10) {timer_cicle = 0;} else {timer_cicle += 1;}
            main_timer.Start();
        }
       static System.Drawing.Point Get_mouse_position() {
            return System.Windows.Forms.Control.MousePosition;
        }
        static void Start_keeper_CR(byte obj) {
            switch (obj) {
                case 1:
                    meKeeper_process = new System.Diagnostics.Process();
                    meKeeper_process.StartInfo.FileName = Basic_Func.APP_PATH + "\\" + me_name + ".exe";
                    meKeeper_process.StartInfo.Arguments = "/keeper";
                    meKeeper_process.Start();
                    break;
                case 2:
                    CR_process = new System.Diagnostics.Process();
                    CR_process.StartInfo.FileName = cr_location;
                    CR_process.Start();
                    break;
            }
        }
        static void PBL_analyse(string []config_text) {
            string[] pbl = new string[0]; bool pdb_add_mode = false;
            for (ushort i = 0; i < config_text.Length; i++)
            {
                if (config_text[i] != "")
                {
                    if (config_text[i].TrimStart(' ').IndexOf(';') != 0)
                    {
                        if (pdb_add_mode)
                        {
                            if (config_text[i].IndexOf('=') != -1)
                            {
                                pdb_add_mode = true;
                                break;
                            }
                            if (config_text[i] == "<end process black list>")
                            {
                                pdb_add_mode = false;
                                break;
                            }
                            Array.Resize(ref pbl, pbl.Length + 1);
                            pbl[pbl.Length - 1] = config_text[i];
                        }
                        else
                        {
                            if (config_text[i] == "<process black list>")
                            {
                                pdb_add_mode = true;
                            }
                        }
                    }
                }
            }
            if (!pdb_add_mode && pbl.Length > 0)
            {
                proc_black_list = pbl;
            }
        }
        static void Set_default_config_values() {
            cr_location = Basic_Func.APP_PATH + "\\ComputerRetard.exe";
            me_name = "ComputerRetard control";
            shortcut_name = "ComputerRetard control";
            shortcut_icon = Basic_Func.APP_PATH + "\\" + me_name + ".exe,0";
            crl_mode = 0;
            crl_add_mode = 1;
            crl_end_num = 120;
            string[] data = {";   Computer Retard Control config file  ",
                "; ComputerRetardLocation - местоположение файла ComputerRetard.exe (замедлителя)",
                "; MyName - имя программы Computer Retard Control (просто имя, без .exe)",
                "; SCName - имя ярлыка программы Computer Retard Control (просто имя, без .lnk)",
                "; SCIcon - путь к файлу с иконкой для ярлыка и индекс иконки (пример: C:\Windows\explorer.exe,0)",
                "; CRMode - режим работы Computer Retard Control (start - сразу запустить, wait_startMem - ожидать перед запуском однажды, wait_start - ожидать перед запуском каждый раз, wait_activDeactiv - активировать и деактивировать)",
                "; TimeCounterMode - режим работы счётчика запуска Computer Retard (AddDec - добавлять (+2) и убавлять (-1), AddOnly - только добавлять)",
                "; TimeCounterInterval - интервал счётчика запуска Computer Retard","",
                "ComputerRetardLocation=" + cr_location,
                "MyName=" + me_name,
                "SCName=" + shortcut_name,
                "SCIcon=" + shortcut_icon,
                "CRMode=start",
                "TimeCounterMode=AddDec",
                "TimeCounterInterval=120","",
                "<process black list>",
                "; Здесь следует указать список программ, запуск которых не следует допускать при запущенном Computer Retard Control",
                "<end process black list>" };
            io.File.WriteAllLines(Basic_Func.APP_PATH + "\\config.ini", data);
        }
    }

    public static class Program_keeper_exec_class {
        static System.Diagnostics.Process CRC_process;
        static System.Timers.Timer keeper_timer;
        static string crc_name;
        public static void Keeper_Start() { //Program start 
            bool to_exit = true;
            int int_prog_id;


            if (io.File.Exists(Basic_Func.APP_PATH + "\\config.ini"))
            {
                
                System.Collections.Generic.List<Basic_Func.Config_entry> config = Basic_Func.Analyse_config(io.File.ReadAllLines(Basic_Func.APP_PATH + "\\config.ini"));
                if (config.Count != 0)
                {
                    bool find = false;
                    for (ushort i = 0; i < config.Count; i++)
                    {
                        if (config[i].key == "MyName" && config[i].value != "" && find == false)
                        {
                            crc_name = config[i].value;
                            find = true;
                            break;
                        }

                    }
                    config.Clear();
                    if (find)
                    {
                        if (io.File.Exists(Basic_Func.APP_PATH + "\\pkd.dat"))
                        { //1 line - Computer Retard Control, 2 line - Computer Retard Control Keeper, 3 line - Computer Retard
                            string[] proc_id = io.File.ReadAllLines(Basic_Func.APP_PATH + "\\pkd.dat");
                            if (proc_id.Length == 3)
                            {
                                if (int.TryParse(proc_id[0], out int_prog_id))
                                { //Computer Retard Control Keeper
                                    try
                                    {
                                        CRC_process = System.Diagnostics.Process.GetProcessById(int_prog_id);
                                        to_exit = false;
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
            }
            if (to_exit) {
                Program.Exit();
            } else {
                keeper_timer = new System.Timers.Timer();
                Basic_Func.Timer_Elapse_Func tef = keeper_timer_elapsed;
                Basic_Func.Timer_Initialise(keeper_timer, tef, 1000, true);
            }
        }
        static void keeper_timer_elapsed(object sender, EventArgs e)
        {
            try {
                System.Diagnostics.Process.GetProcessById(CRC_process.Id);
            } catch {
                CRC_process = new System.Diagnostics.Process();
                CRC_process.StartInfo.FileName = Basic_Func.APP_PATH + "\\" + crc_name + ".exe";
                CRC_process.StartInfo.Arguments = "/lbc";
                CRC_process.Start();
            }
        }
    }
}