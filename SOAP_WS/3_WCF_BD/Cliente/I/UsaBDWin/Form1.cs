/*
 * Cliente Windows usa WCF sobre Base de Dados
 * lufer@ipca.pt
 * 
 * */
using System;
using System.Data;  //DataSet
using System.Windows.Forms;

namespace UsaWCF_BD_WIN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
     
   private void Form1_Load(object sender, EventArgs e)
        {
            WSBD1.DBClient ws1 = new WSBD1.DBClient();

            DataSet ds = ws1.GetAllFlights();

            //DataSet ds = ws.GetHoteis();
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "Voos";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            WSBD1.DBClient ws1 = new WSBD1.DBClient();

            if (textBox1.Text.Length > 0)
            {
                DataSet ds = ws1.GetAllHoteisCidade(textBox1.Text);

                //DataSet ds = ws.GetHoteis();
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "Hoteis1";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WSBD1.DBClient ws1 = new WSBD1.DBClient();

            if (textBox1.Text.Length > 0)
            {
                int cap = int.Parse(textBox1.Text);
                DataSet ds = ws1.GetAllHoteisComCapacidade(cap);
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "Hoteis2";
            }
        }

      
    }
}
