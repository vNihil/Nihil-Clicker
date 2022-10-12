using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoclick
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);



        public Boolean cDerecho,cIzquierdo, ocultar;
        public Process[] pr;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                cDerecho = true;
            }
            if (checkBox2.Checked)
            {
                cIzquierdo = true;
            }
            if (checkBox3.Checked)
            {
                ocultar = true;
            }
            if(checkBox1.Checked && checkBox2.Checked)
            {
                cIzquierdo = true;
                cDerecho = true;
            }
            pr = Process.GetProcessesByName("javaw");
            if (pr.Length > 0)
            {
                Click.Start();
            }
            else
            {
                MessageBox.Show("Debes abrir minecraft para inyectar el autoclicker");
                return;
            }

            if (ocultar)
            {
                this.Hide();
            }

        }

        private void Click_Tick(object sender, EventArgs e)
        {
            if (GetParent(pr[0]).ToString().Equals("System.Diagnostics.Process (MinecraftLauncher)"))
            {
                if (GetForegroundWindow() == FindWindow(null, pr[0].MainWindowTitle))
                {
                    Console.WriteLine(GetForegroundWindow());
                    Console.WriteLine(pr[0].MainWindowTitle);
                    if (MouseButtons == MouseButtons.Left && cIzquierdo)
                    {
                        SendMessage(GetForegroundWindow(), 0x201, 0, 0);
                        Task.Delay(20).Wait();
                        SendMessage(GetForegroundWindow(), 0x202, 0, 0);
                    }
                    if (MouseButtons == MouseButtons.Right && cDerecho)
                    {
                        SendMessage(GetForegroundWindow(), 0x0204, 0, 0);
                        Task.Delay(20).Wait();
                        SendMessage(GetForegroundWindow(), 0x0204, 0, 0);
                    }
                }
               

            }
            else if (ocultar)
            {
                Application.Exit();
            }
            else
            {

               Click.Stop();
                MessageBox.Show("Minecraft no vanilla o minecraft ha sido cerrado");
            }
        }

        public static Process GetParent(Process process)
        {
            try
            {
                using (var query = new ManagementObjectSearcher(
                  "SELECT * " +
                  "FROM Win32_Process " +
                  "WHERE ProcessId=" + process.Id))
                {
                    return query
                      .Get()
                      .OfType<ManagementObject>()
                      .Select(p => Process.GetProcessById((int)(uint)p["ParentProcessId"]))
                      .FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
