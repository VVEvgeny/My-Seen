using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace My_Seen
{
    public partial class Add_Serial : Form
    {
        public Add_Serial()
        {
            InitializeComponent();
        }
        private Serials newFilm;
        public Serials NewFilm
        {
            get
            {
                return newFilm;
            }
            set
            {
                newFilm = value;
            }
        }
        private void Add_Serial_Load(object sender, EventArgs e)
        {
            if (Text != Resource.Edit)
            {
                comboBox1.Text = comboBox1.Items[comboBox1.Items.Count - 1].ToString();
                dateTimePicker1.Value = DateTime.Now;
            }
            dateTimePicker1.MaxDate = DateTime.Now.AddDays(1);
            dateTimePicker1.MinDate = new DateTime(1988, 10, 2);
        }
        private int EditId;
        public void EditData(string id, string _name, string _seeDate, string _rate,string _season,string _series)
        {
            Text = Resource.Edit;
            EditId = Convert.ToInt32(id);
            textBox1.Text = _name;
            dateTimePicker1.Value = DateTime.Parse(_seeDate);
            dateTimePicker1.Enabled = false;
            textBox2.Text = _season;
            textBox3.Text = _series;
            button1.Text = Resource.Edit;

            comboBox1.Text = _rate.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show(Resource.EnterSerialName);
                return;
            }
            newFilm = new Serials() { Id=EditId,Name= textBox1.Text,DateBegin=dateTimePicker1.Value
                ,Rate=Convert.ToInt32(comboBox1.Text), LastSeason=Convert.ToInt32(textBox2.Text),LastSeries=Convert.ToInt32(textBox3.Text) };
            Hide();
        }
    }
}
