using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCF_Client_WindowsForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WCF_EXT.CalcServiceClient proxy = new WCF_EXT.CalcServiceClient();

            WCF_EXT.SumSub aux = proxy.SumAndSub(2, 3);
            MessageBox.Show("Resultado", String.Format("Soma: {0} - Subtração: {1}", aux.Sum.ToString(), aux.Sub.ToString()));
        }

        private void button2_Click(object sender, EventArgs e)
        {  
            WCF_EXT_2.CalcServiceClient proxy = new WCF_EXT_2.CalcServiceClient();
            proxy.SumAndSubCompleted += Proxy_SumAndSubCompleted;
            MessageBox.Show("Espera pelo resultado...");
            proxy.SumAndSubAsync(12, -3);
        }

        private void Proxy_SumAndSubCompleted(object sender, WCF_EXT_2.SumAndSubCompletedEventArgs e)
        {
            WCF_EXT_2.SumSub  aux = e.Result;
            MessageBox.Show("Resultado", String.Format("Soma: {0} - Subtração: {1}", aux.Sum.ToString(), aux.Sub.ToString()));
        }
    }
}
