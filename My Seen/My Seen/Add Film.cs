using System;
using System.Linq;
using System.Windows.Forms;
using MySeenLib;

namespace My_Seen
{
    public partial class AddFilm : Form
    {
        public Users User { get; set; }

        public AddFilm()
        {
            InitializeComponent();
            _editId = 0;

            comboBox1.Items.Clear();
            // ReSharper disable once CoVariantArrayConversion
            comboBox1.Items.AddRange(items: Defaults.Ratings.GetAll().ToArray());
            if (comboBox1.Items.Count != 0) comboBox1.Text = comboBox1.Items[0].ToString();

            comboBox2.Items.Clear();
            // ReSharper disable once CoVariantArrayConversion
            comboBox2.Items.AddRange(items: Defaults.Genres.GetAll().ToArray());
            if (comboBox2.Items.Count != 0) comboBox2.Text = comboBox2.Items[0].ToString();

            DelRecord = false;
        }

        private void Add_Film_Load(object sender, EventArgs e)
        {
            if (Text != Resource.Edit)
            {
                dateTimePicker1.Value = DateTime.Now;
            }
            dateTimePicker1.MaxDate = DateTime.Now.AddDays(1);
            dateTimePicker1.MinDate = new DateTime(1988, 10, 2);
        }

        public Films NewFilm { get; set; }

        private int _editId;
        public bool DelRecord;
        public void EditData(string id, string name, string genre, string seeDate, string rate)
        {
            Text = Resource.Edit;
            _editId = Convert.ToInt32(id);
            textBox1.Text = name;
            dateTimePicker1.Value = DateTime.Parse(seeDate);
            dateTimePicker1.Enabled = false;
            button1.Text = Resource.Edit;

            comboBox1.Text = rate;
            comboBox2.Text = genre;
            button2.Visible = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if(textBox1.Text.Length==0)
            {
                errorProvider.SetError(textBox1, Resource.EnterFilmName);
            }
            if (string.IsNullOrEmpty(errorProvider.GetError(textBox1)))
            {
                ModelContainer mc = new ModelContainer();
                if (mc.FilmsSet.Count(f => f.Name.ToLower() == textBox1.Text.ToLower() && f.UsersId==User.Id && (_editId == 0 || f.Id != _editId)) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
                {
                    errorProvider.SetError(textBox1, Resource.FilmNameAlreadyExists);
                }
            }
            if (!ErrorProviderTools.IsValid(errorProvider)) return;

            NewFilm = _editId != 0 ? 
                new Films() { Id = _editId, UsersId = User.Id, Name = textBox1.Text, DateSee = dateTimePicker1.Value, DateChange = DateTime.Now, Rating = Defaults.Ratings.GetId(comboBox1.Text), Genre = Defaults.Genres.GetId(comboBox2.Text) } : 
                new Films() { UsersId = User.Id, Name = textBox1.Text, DateSee = dateTimePicker1.Value, DateChange = DateTime.Now, Rating = Defaults.Ratings.GetId(comboBox1.Text), Genre = Defaults.Genres.GetId(comboBox2.Text) };
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewFilm = new Films() { Id = _editId, UsersId = User.Id, Name = textBox1.Text, DateSee = dateTimePicker1.Value, DateChange = DateTime.Now, Rating = Defaults.Ratings.GetId(comboBox1.Text), Genre = Defaults.Genres.GetId(comboBox2.Text) };
            DelRecord = true;
            Hide();
        }
    }
}
