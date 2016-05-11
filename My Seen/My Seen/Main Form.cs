using System;
using System.Linq;
using System.Windows.Forms;
using MySeenLib;
using static MySeenLib.CultureInfoTool;

namespace My_Seen
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            IsRestart = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CreateUser form = new CreateUser();
            form.ShowDialog();
            Load_Users();
        }
        private void Load_Users()
        {
            comboBox1.Items.Clear();
            var mc = new ModelContainer();
            comboBox1.Items.AddRange(mc.UsersSet.OrderByDescending(u => u.CreationDate).Select(u => u.Name).ToArray());
            if (comboBox1.Items.Count != 0) comboBox1.Text = comboBox1.Items[0].ToString();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.Text = Culture == Cultures.English ? comboBox2.Items[0].ToString() : comboBox2.Items[1].ToString();

            Load_Users();
        }

        private bool _noClose = false;
        private void ReloadAfterUserDelete()
        {
            textBox2.Text = "";
            comboBox1.Text = "";
            _noClose = true;
            Load_Users();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (comboBox1.Text.Length == 0)
            {
                errorProvider.SetError(comboBox1, Resource.EnterUsename);
            }
            if(textBox2.Text.Length==0)
            {
                errorProvider.SetError(textBox2, Resource.EnterPassword);
            }
            Users user = null;
            try
            {
                var mc= new ModelContainer();
                user = mc.UsersSet.First(u => u.Name == comboBox1.Text);
            }
            catch
            {
                if (string.IsNullOrEmpty(errorProvider.GetError(comboBox1))) errorProvider.SetError(comboBox1, Resource.UserNotExist);
            }
            if(user!=null && !Md5Tools.VerifyMd5Hash(textBox2.Text,user.Password))
            {
                if (string.IsNullOrEmpty(errorProvider.GetError(textBox2))) errorProvider.SetError(textBox2, Resource.WrongPassword);
            }
            if (!ErrorProviderTools.IsValid(errorProvider)) return;

            Hide();
            var form = new Data {User = user};
            _noClose = false;
            form.NeedRestartAppAfterDeleteUserEvent.Event += new MySeenEventHandler(ReloadAfterUserDelete);
            form.ShowDialog();
            form.Close();
            if (!_noClose)
            {
                Close();
            }
            else
            {
                Show();
            }
        }

        public bool IsRestart;
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "ENG")
            {
                if (SetCulture(Cultures.English))
                {
                    Properties.Settings.Default.LastLanguage = Cultures.English;
                    Properties.Settings.Default.Save();
                    IsRestart = true;
                    Hide();
                }
            }
            else
            {
                if (SetCulture(Cultures.Russian))
                {
                    Properties.Settings.Default.LastLanguage = Cultures.Russian;
                    Properties.Settings.Default.Save();
                    IsRestart = true;
                    Hide();
                }
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {
            textBox2.Text = comboBox1.Text;
        }
    }
}
