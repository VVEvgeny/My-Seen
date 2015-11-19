using System;
using System.Linq;
using System.Windows.Forms;
using MySeenLib;
using My_Seen.Properties;

namespace My_Seen
{
    public partial class AddSerial : Form
    {
        public Users User { get; set; }

        public bool DelRecord;
        public AddSerial()
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

        public Serials NewFilm { get; set; }

        private void Add_Serial_Load(object sender, EventArgs e)
        {
            if (Text != Resource.Edit)
            {
                dateTimePicker1.Value = DateTime.Now;
            }
            dateTimePicker1.MaxDate = DateTime.Now.AddDays(1);
            dateTimePicker1.MinDate = new DateTime(1988, 10, 2);
        }
        private int _editId;
        public void EditData(string id, string name, string seeDate, string rate, string season, string series, string genre)
        {
            Text = Resource.Edit;
            _editId = Convert.ToInt32(id);
            textBox1.Text = name;
            dateTimePicker1.Value = DateTime.Parse(seeDate);
            dateTimePicker1.Enabled = false;
            textBox2.Text = season;
            textBox3.Text = series;
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
                errorProvider.SetError(textBox1, Resource.EnterSerialName);
            }
            if (string.IsNullOrEmpty(textBox2.Text) && _editId == 0) textBox2.Text = Resources.Add_Serial_button1_Click__1;
            try
            {
                // ReSharper disable once UnusedVariable
                var int32 = Convert.ToInt32(textBox2.Text);
            }
            catch
            {
                errorProvider.SetError(textBox2, Resource.EnterSeasonNumber);
            }
            if (string.IsNullOrEmpty(textBox3.Text) && _editId == 0) textBox3.Text = Resources.Add_Serial_button1_Click__1;
            try
            {
                // ReSharper disable once UnusedVariable
                var int32 = Convert.ToInt32(textBox3.Text);
            }
            catch
            {
                errorProvider.SetError(textBox3, Resource.EnterSerionNumber);
            }
            if (string.IsNullOrEmpty(errorProvider.GetError(textBox1)))
            {
                ModelContainer mc = new ModelContainer();
                if (mc.SerialsSet.Count(f => f.Name.ToLower() == textBox1.Text.ToLower() && f.UsersId == User.Id && (_editId == 0 || f.Id != _editId)) != 0)//айди проверяем только для редактируемых, чтобы не налететь по названию на чужой
                {
                    errorProvider.SetError(textBox1, Resource.SerialNameAlreadyExists);
                }
            }
            if (!ErrorProviderTools.IsValid(errorProvider)) return;

            NewFilm = _editId != 0 ? 
                new Serials() { Id = _editId, UsersId = User.Id, Name = textBox1.Text, DateBegin = dateTimePicker1.Value, DateLast = DateTime.Now, DateChange = DateTime.Now, Rating = Defaults.Ratings.GetId(comboBox1.Text), LastSeason = Convert.ToInt32(textBox2.Text), LastSeries = Convert.ToInt32(textBox3.Text), Genre = Defaults.Genres.GetId(comboBox2.Text) } : 
                new Serials() { UsersId = User.Id, Name = textBox1.Text, DateBegin = dateTimePicker1.Value, DateLast = DateTime.Now, DateChange = DateTime.Now, Rating = Defaults.Ratings.GetId(comboBox1.Text), LastSeason = Convert.ToInt32(textBox2.Text), LastSeries = Convert.ToInt32(textBox3.Text), Genre = Defaults.Genres.GetId(comboBox2.Text) };
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewFilm = new Serials() { Id = _editId, UsersId = User.Id, Name = textBox1.Text, DateBegin = dateTimePicker1.Value, DateLast = DateTime.Now, DateChange = DateTime.Now, Rating = Defaults.Ratings.GetId(comboBox1.Text), LastSeason = Convert.ToInt32(textBox2.Text), LastSeries = Convert.ToInt32(textBox3.Text), Genre = Defaults.Genres.GetId(comboBox2.Text) };
            DelRecord = true;
            Hide();
        }
    }
}
