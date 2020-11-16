using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.IO;

namespace PARCIAL2PASSARELLIBRUNO
{
    public partial class AuthService : ServiceBase
    {
        private Security security = new Security();
        private string path = "C:/Users/bruno/Desktop/rsa";

        public AuthService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1.Start();
        }

        protected override void OnStop()
        {
            timer1.Stop();
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Write(security.GetPublicKey(), "/public.xml");
                Read("/file.xml");
            }
            catch(Exception error)
            {
                Write(error.Message, "/error.txt");
            }
        }

        private void Write(string Message, string file)
        {
            try
            {
                if (!Directory.Exists(this.path))
                {
                    Directory.CreateDirectory(this.path);
                }

                string filepath = this.path + file;
                if (!File.Exists(filepath))
                {
                    using (StreamWriter streamWriter = File.CreateText(filepath))
                    {
                        streamWriter.WriteLine(Message);
                    }
                }
                else
                {
                    using (StreamWriter streamWriter = File.CreateText(filepath))
                    {
                        streamWriter.WriteLine(Message);
                    }
                }
            }
            catch(Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        private void Read(string file)
        {
            string filepath = this.path + file;
            if (!File.Exists(filepath))
            {
                // todavia no se envio ningun archivo para desencriptar
            }
            else
            {   

                try
                {
                    Write(security.Encryptation(filepath), "/crypto.txt");
                    File.Delete(filepath);
                }
                catch (Exception error)
                {
                    throw new Exception(error.Message);
                }
            }
        }
    }
}
