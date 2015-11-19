using System;
using System.Linq;
using System.Windows.Forms;
using MySeenLib;

namespace My_Seen
{
    public partial class Config : Form
    {
        public MySeenEvent DbUserDeleted;
        public MySeenEvent DbDataChanged;
        public MySeenEvent DbUpdateUser;
        public Config()
        {
            InitializeComponent();
            DbDataChanged = new MySeenEvent();
            DbUserDeleted = new MySeenEvent();
            DbUpdateUser = new MySeenEvent();
        }

        public Users User { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox2.Text.Length!=0)
            {
                if(Md5Tools.GetMd5Hash(textBox2.Text)!=User.Password)
                {
                    MessageBox.Show(Resource.WrongPassword);
                    return;
                }
                string msg = "";
                if (!Validations.ValidatePassword(ref msg, textBox3.Text, textBox4.Text))
                {
                    MessageBox.Show(msg);
                    return;
                }
                User.Password = Md5Tools.GetMd5Hash(textBox2.Text);
            }
            ModelContainer mc = new ModelContainer();
            Users updateUser = mc.UsersSet.First(u => u.Id == User.Id);
            updateUser.Email = textBox5.Text;
            updateUser.Password = User.Password;
            mc.SaveChanges();
            DbUpdateUser.Exec();
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
            mc.FilmsSet.RemoveRange(mc.FilmsSet.Where(f=>f.UsersId==User.Id));
            mc.SerialsSet.RemoveRange(mc.SerialsSet.Where(f => f.UsersId == User.Id));
            mc.BooksSet.RemoveRange(mc.BooksSet.Where(f => f.UsersId == User.Id));
            mc.SaveChanges();
            DbDataChanged.Exec();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DeleteData();
            MessageBox.Show(Resource.DBDeleted);
        }
        private void DeleteUser()
        {
            ModelContainer mc = new ModelContainer();
            mc.UsersSet.Remove(mc.UsersSet.First(u=>u.Id==User.Id));
            mc.SaveChanges();
            MessageBox.Show(Resource.DBUserDeleted);
            DbUserDeleted.Exec();
            Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            DeleteUser();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(WebApi.CheckUser(textBox5.Text));
        }
        public Films Map_FilmsRequestResponse_To_Films(MySeenWebApi.SyncJsonData model)
        {
            if (model == null) return new Films();

            return new Films
            {
                Id_R = model.Id,
                Name = model.Name,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rating = model.Rating,
                isDeleted = model.IsDeleted,
                UsersId = User.Id
            };
        }
        public Serials Map_FilmsRequestResponse_To_Serials(MySeenWebApi.SyncJsonData model)
        {
            if (model == null) return new Serials();

            return new Serials
            {
                Id_R = model.Id,
                Name = model.Name,
                Genre = model.Genre,
                Rating = model.Rating,
                isDeleted = model.IsDeleted,
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
                MessageBox.Show(Resource.EnterEmail);
                return;
            }
            else
            {
                User.Email = textBox5.Text;
            }
            DeleteData();
            WebApi.Sync(User);
            DbDataChanged.Exec();
        }

    }
}
