using System;
using System.Linq;
using System.Windows.Forms;
using MySeenLib;

namespace My_Seen
{
    public partial class AddBook : Form
    {
        public Users User { private get; set; }

        public bool DelRecord;
        public AddBook()
        {
            InitializeComponent();
            _editId = 0;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Defaults.Ratings.GetAll().ToArray());
            if (comboBox1.Items.Count != 0) comboBox1.Text = comboBox1.Items[0].ToString();

            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(Defaults.Genres.GetAll().ToArray());
            if (comboBox2.Items.Count != 0) comboBox2.Text = comboBox2.Items[0].ToString();

            DelRecord = false;
        }

        public Books NewFilm { get; set; }

        private void Add_Book_Load(object sender, EventArgs e)
        {
            if (Text != Resource.Edit)
            {
                dateTimePicker1.Value = DateTime.Now;
            }
            dateTimePicker1.MaxDate = DateTime.Now.AddDays(1);
            dateTimePicker1.MinDate = new DateTime(1988, 10, 2);
        }
        private int _editId;
        public void EditData(string id, string name, string seeDate, string rate, string authors, string genre)
        {
            Text = Resource.Edit;
            _editId = Convert.ToInt32(id);
            textBox1.Text = name;
            dateTimePicker1.Value = DateTime.Parse(seeDate);
            dateTimePicker1.Enabled = false;
            textBox2.Text = authors;
            button1.Text = Resource.Edit;

            comboBox1.Text = rate;
            comboBox2.Text = genre;
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
                var mc = new ModelContainer();
                if (mc.BooksSet.Count(f => f.Name.ToLower() == textBox1.Text.ToLower() && f.UsersId == User.Id && (_editId == 0 || f.Id != _editId)) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
                {
                    errorProvider.SetError(textBox1, Resource.BookNameAlreadyExists);
                }
            }
            if (!ErrorProviderTools.IsValid(errorProvider)) return;

            if (_editId != 0) NewFilm = new Books() { Id = _editId, UsersId = User.Id, Name = textBox1.Text, DateRead = dateTimePicker1.Value, DateChange = DateTime.Now, Rating = Defaults.Ratings.GetId(comboBox1.Text), Genre = Defaults.Genres.GetId(comboBox2.Text), Authors=textBox2.Text };
            else NewFilm = new Books() { UsersId = User.Id, Name = textBox1.Text, DateRead = dateTimePicker1.Value, DateChange = DateTime.Now, Rating = Defaults.Ratings.GetId(comboBox1.Text), Genre = Defaults.Genres.GetId(comboBox2.Text), Authors = textBox2.Text };
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewFilm = new Books() { Id = _editId, UsersId = User.Id, Name = textBox1.Text, DateRead = dateTimePicker1.Value, DateChange = DateTime.Now, Rating = Defaults.Ratings.GetId(comboBox1.Text), Genre = Defaults.Genres.GetId(comboBox2.Text), Authors = textBox2.Text };
            DelRecord = true;
            Hide();
        }
    }
}
