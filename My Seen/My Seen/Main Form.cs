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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Load_Users();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Length == 0)
            {
                MessageBox.Show("Enter usename");
                return;
            }
            if(textBox2.Text.Length==0)
            {
                MessageBox.Show("Enter password");
                return;
            }
            Users user = null;
            try
            {
                ModelContainer mc= new ModelContainer();
                user = mc.UsersSet.First(u => u.Name == comboBox1.Text);
            }
            catch
            {
                MessageBox.Show("User not exists");
                return;
            }
            if(!MD5Tools.VerifyMd5Hash(textBox2.Text,user.Password))
            {
                MessageBox.Show("Wrong password");
                return;
            }
            Hide();
            Data form = new Data();
            form.User = user;
            form.ShowDialog();
            Close();
        }
    }
}
