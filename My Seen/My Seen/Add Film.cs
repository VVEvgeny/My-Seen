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
    public partial class Add_Film : Form
    {
        public Add_Film()
        {
            InitializeComponent();
        }

        private void Add_Film_Load(object sender, EventArgs e)
        {
            if (Text != Resource.Edit)
            {
                comboBox1.Text = comboBox1.Items[comboBox1.Items.Count - 1].ToString();
                dateTimePicker1.Value = DateTime.Now;
            }
            dateTimePicker1.MaxDate = DateTime.Now.AddDays(1);
            dateTimePicker1.MinDate = new DateTime(1988, 10, 2);
        }
        private FilmsResult newFilm;
        public FilmsResult NewFilm
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

        private int EditId;
        public void EditData(string id, string _name, string _seeDate, string _rate)
        {
            Text = Resource.Edit;
            EditId = Convert.ToInt32(id);
            textBox1.Text = _name;
            dateTimePicker1.Value = DateTime.Parse(_seeDate);
            dateTimePicker1.Enabled = false;
            button1.Text = Resource.Edit;

            comboBox1.Text = _rate.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length==0)
            {
                MessageBox.Show(Resource.EnterFilmName);
                return;
            }
            newFilm = new FilmsResult(Text == Resource.Edit ? EditId : 0, textBox1.Text, dateTimePicker1.Value, comboBox1.Text);
            Hide();
        }
    }
}
