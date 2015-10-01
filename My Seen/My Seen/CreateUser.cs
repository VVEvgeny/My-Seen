using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using MySeenLib;

namespace My_Seen
{
    public partial class CreateUser : Form
    {
        public CreateUser()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            string msg = "";
            if (!LibTools.Validation.ValidateUserName(ref msg, textBox1.Text))
            {
                errorProvider.SetError(textBox1, msg);
            }
            if (!LibTools.Validation.ValidatePassword(ref msg,textBox2.Text,textBox3.Text))
            {
                errorProvider.SetError(textBox2, msg);
                errorProvider.SetError(textBox3, msg);
            }

            ModelContainer mc = new ModelContainer();
            if(mc.UsersSet.Count(u=>u.Name==textBox1.Text)!=0)
            {
                MessageBox.Show(Resource.UserAlreadyExists);
            }
            if (!ErrorProviderTools.isValid(errorProvider)) return;

            Users us = new Users();
            us.Name = textBox1.Text;
            us.Password = MD5Tools.GetMd5Hash(textBox2.Text);
            us.CreationDate = DateTime.Now;
            us.DateLastSync = DateTime.Now;

            mc.UsersSet.Add(us);
            mc.SaveChanges();

            MessageBox.Show(Resource.UserCreated);
            Close();
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {

        }

        private void textBox2_Validated(object sender, EventArgs e)
        {

        }

        private void textBox3_Validated(object sender, EventArgs e)
        {

        }
    }
}
