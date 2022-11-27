using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Apps
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Sobre IPs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //serviço
            IPWS.IP ws = new IPWS.IP();
            //estrutura de dados
            IPWS.DadosPais dp;
            //estrutura de dados II
            //IPWS.paisdados dp2;            

            textBox2.Text = ws.PaisData(textBox1.Text).capital;
            dp = ws.PaisData(textBox1.Text);  
           
        }

        /// <summary>
        /// Sobre Hoteis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //serviço
            HoteisWS.ServiceSoapClient ws = new HoteisWS.ServiceSoapClient();
            
            //Pode-se aceder às estruturas de dados
            //HoteisWS.HotelClass[] hoteis;           

            dataGridView1.DataSource = ws.GetAllHoteisClass();
            dataGridView1.DataMember="Hotel"
            
        }


        /// <summary>
        /// Sobre Pessoas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Estruturados.Service ws = new Estruturados.Service();

            Estruturados.Pessoa[] p;
            Estruturados.Varios v;

            p = ws.GetPessoas();
            dataGridView1.DataSource = p;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Sobre Hoteis com Colletions       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            //serviço
            HoteisWS.ServiceSoapClient ws = new HoteisWS.ServiceSoapClient();

            Object[] a =ws.GetAllHoteisClass();

            ArrayList al= new ArrayList(a);
            foreach (object o in a) al.Add(o);
        
            dataGridView1.DataSource = al;            
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
