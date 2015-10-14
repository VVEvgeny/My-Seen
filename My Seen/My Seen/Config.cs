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
using System.Net;
using System.IO;

namespace My_Seen
{
    public partial class Config : Form
    {
        public MySeenEvent DBUserDeleted = null;
        public MySeenEvent DBDataChanged = null;
        public MySeenEvent DBUpdateUser = null;
        public Config()
        {
            InitializeComponent();
            DBDataChanged = new MySeenEvent();
            DBUserDeleted = new MySeenEvent();
            DBUpdateUser = new MySeenEvent();
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
            update_user.Email = textBox5.Text;
            update_user.Password = User.Password;
            mc.SaveChanges();
            DBUpdateUser.Exec();
            Close();
        }

        private void Config_Load(object sender, EventArgs e)
        {
            textBox1.Text = User.Name;
            textBox5.Text = User.Email;
        }
        private void DeleteData()
        {
            ModelContainer mc = new ModelContainer();
            mc.FilmsSet.RemoveRange(mc.FilmsSet.Where(f=>f.UsersId==user.Id));
            mc.SerialsSet.RemoveRange(mc.SerialsSet.Where(f => f.UsersId == user.Id));
            mc.SaveChanges();
            DBDataChanged.Exec();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DeleteData();
            MessageBox.Show(Resource.DBDeleted);
        }
        private void DeleteUser()
        {
            ModelContainer mc = new ModelContainer();
            mc.UsersSet.Remove(mc.UsersSet.First(u=>u.Id==user.Id));
            mc.SaveChanges();
            MessageBox.Show(Resource.DBUserDeleted);
            DBUserDeleted.Exec();
            Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            DeleteUser();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox5.Text.Length == 0)
            {
                MessageBox.Show("Enter Email");
                return;
            }
            WebRequest req = WebRequest.Create(API_Data.ApiHost + API_Data.ApiUsers + MD5Tools.GetMd5Hash(textBox5.Text.ToLower()) + "/" + ((int)API_Data.ModesApiUsers.isUserExists).ToString());
            WebResponse res = req.GetResponse();
            Stream stream = res.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string data = sr.ReadToEnd();
            API_Data.RequestResponseAnswer answer = API_Data.GetResponseAnswer(data);
            if (answer != null)
            {
                if (answer.Value == API_Data.RequestResponseAnswer.Values.UserNotExist)
                {
                    MessageBox.Show("User Not Exist");
                }
                else
                {
                    MessageBox.Show("All OK");
                }
            }
            else
            {
                MessageBox.Show("ERROR WORK WITH API");
            }
        }
        public Films Map_FilmsRequestResponse_To_Films(API_Data.FilmsRequestResponse model)
        {
            if (model == null) return new Films();

            return new Films
            {
                Id_R = model.Id,
                Name = model.Name,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rate = model.Rate,
                isDeleted = model.isDeleted,
                UsersId = User.Id
            };
        }
        public Serials Map_FilmsRequestResponse_To_Serials(API_Data.FilmsRequestResponse model)
        {
            if (model == null) return new Serials();

            return new Serials
            {
                Id_R = model.Id,
                Name = model.Name,
                Genre = model.Genre,
                Rate = model.Rate,
                isDeleted = model.isDeleted,
                UsersId = User.Id,
                DateBegin = model.DateBegin,
                DateLast = model.DateLast,
                LastSeason = model.LastSeason,
                LastSeries = model.LastSeries
            };
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox5.Text.Length == 0)
            {
                MessageBox.Show("Enter Email");
                return;
            }
            WebRequest req = WebRequest.Create(API_Data.ApiHost + API_Data.ApiSync + MD5Tools.GetMd5Hash(textBox5.Text.ToLower()) + "/" + ((int)API_Data.ModesApiFilms.All).ToString());
            DeleteData();

            WebResponse res = req.GetResponse();
            Stream stream = res.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string data = sr.ReadToEnd();

            API_Data.RequestResponseAnswer answer = API_Data.GetResponseAnswer(data);
            if (answer != null)
            {
                if (answer.Value == API_Data.RequestResponseAnswer.Values.UserNotExist)
                {
                    MessageBox.Show("User Not Exist");
                }
                else if (answer.Value == API_Data.RequestResponseAnswer.Values.NoData)
                {
                    MessageBox.Show("No Data");
                }
                else if (answer.Value == API_Data.RequestResponseAnswer.Values.BadRequestMode)
                {
                    MessageBox.Show("Bad Request Mode");
                }
            }
            else
            {
                ModelContainer mc = new ModelContainer();
                foreach(API_Data.FilmsRequestResponse film in API_Data.GetResponse(data))
                {
                    if(film.IsFilm)
                    {
                        mc.FilmsSet.Add(Map_FilmsRequestResponse_To_Films(film));
                    }
                    else
                    {
                        mc.SerialsSet.Add(Map_FilmsRequestResponse_To_Serials(film));
                    }
                }
                mc.SaveChanges();
                DBDataChanged.Exec();
                MessageBox.Show("Sync OK");
            }
        }
    }
}
