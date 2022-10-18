using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Win32;
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






        public Boolean cDerecho, cIzquierdo, ocultar, registrarBind, toggle = true;
        public Process[] pr;
        private static Keys bind;
        private Keys r1;
        private KeyboardHook kh = new KeyboardHook(true);

        public Form1()
        {
            InitializeComponent();
            kh.KeyDown += Kh_KeyDown;

        }
        private static void Kh_KeyDown(Keys key, bool Shift, bool Ctrl, bool Alt)
        {
            bind = key;

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
            if (checkBox1.Checked && checkBox2.Checked)
            {
                cIzquierdo = true;
                cDerecho = true;
            }
            if (!checkBox1.Checked && !checkBox2.Checked)
            {
                MessageBox.Show("Debes seleccionar almenos un tipo de click");
                return;
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
            button1.Text = "Ejecutado";
            if (ocultar)
            {
                this.Hide();
            }

            if (bind.Equals(r1))
            {
                bind = Keys.F22;
            }


        }
       

        

       

        private void Form1_Load(object sender, EventArgs e)

        {
           


        }
        
        

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (registrarBind)
            {
                r1 = e.KeyCode;
                button2.Text = r1.ToString();
                registrarBind = false;
            }

        }



        private void button2_Click(object sender, EventArgs e)
        {
            registrarBind = true;
            button2.Text = "Presiona una tecla...";
        }

        private void Click_Tick(object sender, EventArgs e)
        {


            if (GetForegroundWindow() == FindWindow(null, pr[0].MainWindowTitle))
            {
                if (toggle)
                {

                    if (MouseButtons == MouseButtons.Left && cIzquierdo)
                    {
                        SendMessage(GetForegroundWindow(), 0x201, 0, 0);
                        Task.Delay(40).Wait();
                        SendMessage(GetForegroundWindow(), 0x202, 0, 0);
                    }

                    if (MouseButtons == MouseButtons.Right && cDerecho)
                    {
                        SendMessage(GetForegroundWindow(), 0x0204, 0, 0);
                        SendMessage(GetForegroundWindow(), 0x0204, 0, 0);
                        SendMessage(GetForegroundWindow(), 0x0204, 0, 0);
                        SendMessage(GetForegroundWindow(), 0x0204, 0, 0);


                    }
                }

                if (bind.Equals(r1) && toggle)
                {
                    bind = Keys.F24;
                    toggle = false;
                }
                if (bind.Equals(r1) && toggle == false)
                {
                    bind = Keys.F24;
                    toggle = true;
                }
            }

            pr = Process.GetProcessesByName("javaw");
            if (pr.Length < 0)
            {
                Application.Exit();
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

