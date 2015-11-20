using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySeenLib;

namespace My_Seen
{
    public partial class Add_Book : Form
    {
        private Users user;
        public Users User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }
        public bool DelRecord;
        public Add_Book()
        {
            InitializeComponent();
            EditId = 0;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Defaults.Ratings.GetAll().ToArray());
            if (comboBox1.Items.Count != 0) comboBox1.Text = comboBox1.Items[0].ToString();

            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(Defaults.Genres.GetAll().ToArray());
            if (comboBox2.Items.Count != 0) comboBox2.Text = comboBox2.Items[0].ToString();

            DelRecord = false;
        }
        private Books newFilm;
        public Books NewFilm
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
        private void Add_Book_Load(object sender, EventArgs e)
        {
            if (Text != Resource.Edit)
            {
                dateTimePicker1.Value = DateTime.Now;
            }
            dateTimePicker1.MaxDate = DateTime.Now.AddDays(1);
            dateTimePicker1.MinDate = new DateTime(1988, 10, 2);
        }
        private int EditId;
        public void EditData(string id, string _name, string _seeDate, string _rate, string _authors, string _genre)
        {
            Text = Resource.Edit;
            EditId = Convert.ToInt32(id);
            textBox1.Text = _name;
            dateTimePicker1.Value = DateTime.Parse(_seeDate);
            dateTimePicker1.Enabled = false;
            textBox2.Text = _authors;
            button1.Text = Resource.Edit;

            comboBox1.Text = _rate;
            comboBox2.Text = _genre;
            button2.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (textBox1.Text.Length == 0)
            {
                errorProvider.SetError(textBox1, Resource.EnterBookName);
            }
            if (textBox2.Text.Length == 0)
            {
                errorProvider.SetError(textBox2, Resource.EnterBookAuthors);
            }
            if (string.IsNullOrEmpty(errorProvider.GetError(textBox1)))
            {
                ModelContainer mc = new ModelContainer();
                if (mc.BooksSet.Count(f => f.Name.ToLower() == textBox1.Text.ToLower() && f.UsersId == User.Id && (EditId != 0 ? f.Id != EditId : 1 == 1)) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
                {
                    errorProvider.SetError(textBox1, Resource.BookNameAlreadyExists);
                }
            }
            if (!ErrorProviderTools.isValid(errorProvider)) return;

            if (EditId != 0) newFilm = new Books() { Id = EditId, UsersId = user.Id, Name = textBox1.Text, DateRead = dateTimePicker1.Value, DateChange = DateTime.Now, Rating = Defaults.Ratings.GetId(comboBox1.Text), Genre = Defaults.Genres.GetId(comboBox2.Text), Authors=textBox2.Text };
            else newFilm = new Books() { UsersId = user.Id, Name = textBox1.Text, DateRead = dateTimePicker1.Value, DateChange = DateTime.Now, Rating = Defaults.Ratings.GetId(comboBox1.Text), Genre = Defaults.Genres.GetId(comboBox2.Text), Authors = textBox2.Text };
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            newFilm = new Books() { Id = EditId, UsersId = user.Id, Name = textBox1.Text, DateRead = dateTimePicker1.Value, DateChange = DateTime.Now, Rating = Defaults.Ratings.GetId(comboBox1.Text), Genre = Defaults.Genres.GetId(comboBox2.Text), Authors = textBox2.Text };
            DelRecord = true;
            Hide();
        }
    }
}
