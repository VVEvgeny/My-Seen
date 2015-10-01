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
    public partial class Main_Form : Form
    {
        public Main_Form()
        {
            InitializeComponent();
            isRestart = false;
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
            ModelContainer mc = new ModelContainer();
            comboBox1.Items.AddRange(mc.UsersSet.OrderByDescending(u => u.CreationDate).Select(u => u.Name).ToArray());
            if (comboBox1.Items.Count != 0) comboBox1.Text = comboBox1.Items[0].ToString();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (CultureInfoTool.GetCulture() == CultureInfoTool.Cultures.English) comboBox2.Text = comboBox2.Items[0].ToString();
            else comboBox2.Text = comboBox2.Items[1].ToString();

            Load_Users();
        }

        private bool no_close = false;
        private void ReloadAfterUserDelete()
        {
            textBox2.Text = "";
            comboBox1.Text = "";
            no_close = true;
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
                ModelContainer mc= new ModelContainer();
                user = mc.UsersSet.First(u => u.Name == comboBox1.Text);
            }
            catch
            {
                if (errorProvider.GetError(comboBox1) == string.Empty) errorProvider.SetError(comboBox1, Resource.UserNotExists);
            }
            if(user!=null && !MD5Tools.VerifyMd5Hash(textBox2.Text,user.Password))
            {
                if (errorProvider.GetError(textBox2) == string.Empty) errorProvider.SetError(textBox2, Resource.WrongPassword);
            }
            if (!ErrorProviderTools.isValid(errorProvider)) return;

            Hide();
            Data form = new Data();
            form.User = user;
            no_close = false;
            form.NeedRestartAppEventAfterDelUser.Event += new MySeenEventHandler(ReloadAfterUserDelete);
            form.ShowDialog();
            form.Close();
            if (!no_close)
            {
                Close();
            }
            else
            {
                Show();
            }
        }

        public bool isRestart;
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "ENG")
            {
                if (CultureInfoTool.SetCulture(CultureInfoTool.Cultures.English))
                {
                    Properties.Settings.Default.LastLanguage = CultureInfoTool.Cultures.English;
                    Properties.Settings.Default.Save();
                    isRestart = true;
                    Hide();
                }
            }
            else
            {
                if (CultureInfoTool.SetCulture(CultureInfoTool.Cultures.Russian))
                {
                    Properties.Settings.Default.LastLanguage = CultureInfoTool.Cultures.Russian;
                    Properties.Settings.Default.Save();
                    isRestart = true;
                    Hide();
                }
            }
        }
    }
}
