﻿using System;
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
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (CultureInfoTool.GetCulture() == CultureInfoTool.Cultures.English) comboBox2.Text = comboBox2.Items[0].ToString();
            else comboBox2.Text = comboBox2.Items[1].ToString();

            Load_Users();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Length == 0)
            {
                MessageBox.Show(Resource.EnterUsename);
                return;
            }
            if(textBox2.Text.Length==0)
            {
                MessageBox.Show(Resource.EnterPassword);
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
                MessageBox.Show(Resource.UserNotExists);
                return;
            }
            if(!MD5Tools.VerifyMd5Hash(textBox2.Text,user.Password))
            {
                MessageBox.Show(Resource.WrongPassword);
                return;
            }
            Hide();
            Data form = new Data();
            form.User = user;
            form.ShowDialog();
            Close();
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
