using System;
using System.Linq;
using System.Windows.Forms;
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
            if (!Validations.ValidateUserName(ref msg, textBox1.Text))
            {
                errorProvider.SetError(textBox1, msg);
            }
            if (!Validations.ValidatePassword(ref msg, textBox2.Text, textBox3.Text))
            {
                errorProvider.SetError(textBox2, msg);
                errorProvider.SetError(textBox3, msg);
            }

            ModelContainer mc = new ModelContainer();
            if(mc.UsersSet.Count(u=>u.Name==textBox1.Text)!=0)
            {
                MessageBox.Show(Resource.UserAlreadyExists);
            }
            if (!ErrorProviderTools.IsValid(errorProvider)) return;

            Users us = new Users
            {
                Name = textBox1.Text,
                Password = Md5Tools.GetMd5Hash(textBox2.Text),
                CreationDate = DateTime.Now
            };

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
