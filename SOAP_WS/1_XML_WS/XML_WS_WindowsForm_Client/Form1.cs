/**
 * WCF Client
 * Uso sincrono e assíncrono de serviços
 **/

using System;
using System.Windows.Forms;

namespace XML_WS_WindowsForm_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WS.CalcServiceClient proxy = new WS.CalcServiceClient();

            MessageBox.Show("Resultado", proxy.Sum(2, 3).ToString());

            WS2.CalcServiceClient proxy2 = new WS2.CalcServiceClient();
            proxy2.SumCompleted += Proxy2_SumCompleted;
            proxy2.SumAsync(2, 4);
            MessageBox.Show("Esperando pela resposta...");

            WS2.SumSub aux = proxy2.SumAndSub(2, 3);
        }

        private void Proxy2_SumCompleted(object sender, WS2.SumCompletedEventArgs e)
        {
            System.Threading.Thread.Sleep(2000);
            MessageBox.Show(e.Result.ToString());
        }
    }
}
