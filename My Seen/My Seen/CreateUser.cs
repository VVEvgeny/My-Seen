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
            string msg = "";
            if (!LibTools.ValidateUserName(ref msg, textBox1.Text))
            {
                MessageBox.Show(msg);
                return;
            }
            if (!LibTools.ValidatePassword(ref msg,textBox2.Text,textBox3.Text))
            {
                MessageBox.Show(msg);
                return;
            }
            if (!LibTools.ValidateEmail(ref msg, textBox4.Text))
            {
                MessageBox.Show(msg);
                return;
            }



            ModelContainer mc = new ModelContainer();
            if(mc.Users.Count(u=>u.Name==textBox1.Text)!=0)
            {
                MessageBox.Show("User already exists");
                return;
            }
            UsersSet us = new UsersSet();
            us.Name = textBox1.Text;
            us.Password = DesktopTools.GetMd5Hash(textBox2.Text);
            us.CreationDate = DateTime.Now;
            us.Email = textBox4.Text;

            mc.Users.Add(us);
            mc.SaveChanges();

            MessageBox.Show("User created");
            Close();
        }
    }
}
