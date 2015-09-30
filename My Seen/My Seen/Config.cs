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
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }
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

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox2.Text.Length!=0)
            {
                if(MD5Tools.GetMd5Hash(textBox2.Text)!=User.Password)
                {
                    MessageBox.Show(Resource.WrongPassword);
                    return;
                }
                string msg = "";
                if (!LibTools.Validation.ValidatePassword(ref msg, textBox3.Text, textBox4.Text))
                {
                    MessageBox.Show(msg);
                    return;
                }
                User.Password = MD5Tools.GetMd5Hash(textBox2.Text);
            }
            ModelContainer mc = new ModelContainer();
            Users update_user = mc.UsersSet.First(u => u.Id == User.Id);
            User.NameRemote = textBox5.Text;
            if (textBox6.Text.Length != 0)
            {
                User.PasswordRemote = MD5Tools.GetMd5Hash(textBox6.Text);
                update_user.PasswordRemote = User.PasswordRemote;
            }
            update_user.Password = User.Password;
            update_user.NameRemote = User.NameRemote;
            mc.SaveChanges();
            Close();
        }

        private void Config_Load(object sender, EventArgs e)
        {
            textBox1.Text = User.Name;
            textBox5.Text = User.NameRemote;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("In progress");
        }
    }
}
