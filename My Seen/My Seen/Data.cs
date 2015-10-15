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
using System.Threading;
using System.Net;
using System.IO;
using System.Web;

namespace My_Seen
{
    public partial class Data : Form
    {
        private enum DBMode
        {
            Films,
            Serials
        }
        private DBMode CurrentDB = DBMode.Films;

        private bool FastFind = false;

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
        public MySeenEvent NeedRestartAppAfterDeleteUserEvent = null;
        public Data()
        {
            InitializeComponent();
            toolStripComboBox1.Items.Add(Resource.Films);
            toolStripComboBox1.Items.Add(Resource.Serials);
            NeedRestartAppAfterDeleteUserEvent = new MySeenEvent();
        }

        private Thread thread_sync;
        private void Data_Load(object sender, EventArgs e)
        {
            CurrentDB = DBMode.Films;
            toolStripComboBox1.Text = toolStripComboBox1.Items[0].ToString();
            thread_sync = new Thread(new ThreadStart(CheckCanSync));
            thread_sync.Start();
        }
        private void CheckCanSync()
        {
            if (!string.IsNullOrEmpty(User.Email))
            {
                if (WebApi.CheckUser(User.Email) == Resource.UserOK)
                {
                    toolStripMenuItem1.Visible = true;
                }
            }
        }
        private void RestartProgram()
        {
            NeedRestartAppAfterDeleteUserEvent.Exec();
            Close();
        }
        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config form = new Config();
            form.User = User;
            form.DBDataChanged.Event += new MySeenEventHandler(LoadItemsToListView);
            form.DBUserDeleted.Event += new MySeenEventHandler(RestartProgram);
            form.DBUpdateUser.Event += new MySeenEventHandler(UpdateUser);
            form.ShowDialog();
            if (thread_sync.ThreadState != ThreadState.Running)
            {
                thread_sync = new Thread(new ThreadStart(CheckCanSync));
                thread_sync.Start();
            }
        }
        private void UpdateUser()
        {
            ModelContainer mc = new ModelContainer();
            user = mc.UsersSet.First(u => u.Id == user.Id);
        }
        private void UpdateListViewColumns()
        {
            listView1.Columns.Clear();

            ColumnHeader cl_id = new ColumnHeader();
            cl_id.Text = "id";
            cl_id.Width = 0;
            listView1.Columns.Add(cl_id);

            if (CurrentDB == DBMode.Films)
            {
                ColumnHeader cl_name = new ColumnHeader();
                cl_name.Text = Resource.Name;
                cl_name.Width = 460;
                listView1.Columns.Add(cl_name);

                ColumnHeader cl_genre = new ColumnHeader();
                cl_genre.Text = Resource.Genre;
                cl_genre.Width = 50;
                listView1.Columns.Add(cl_genre);

                ColumnHeader cl_date = new ColumnHeader();
                cl_date.Text = Resource.Date;
                cl_date.Width = 120;
                listView1.Columns.Add(cl_date);
            }
            else
            {
                ColumnHeader cl_name = new ColumnHeader();
                cl_name.Text = Resource.Name;
                cl_name.Width = 290;
                listView1.Columns.Add(cl_name);

                ColumnHeader cl_last_ep = new ColumnHeader();
                cl_last_ep.Text = Resource.LastEpisode;
                cl_last_ep.Width = 50;
                listView1.Columns.Add(cl_last_ep);

                ColumnHeader cl_genre = new ColumnHeader();
                cl_genre.Text = Resource.Genre;
                cl_genre.Width = 50;
                listView1.Columns.Add(cl_genre);

                ColumnHeader cl_date_last = new ColumnHeader();
                cl_date_last.Text = Resource.DateLast;
                cl_date_last.Width = 120;
                listView1.Columns.Add(cl_date_last);

                ColumnHeader cl_date_begin = new ColumnHeader();
                cl_date_begin.Text = Resource.DateBegin;
                cl_date_begin.Width = 120;
                listView1.Columns.Add(cl_date_begin);
            }
            ColumnHeader cl_rate = new ColumnHeader();
            cl_rate.Text = Resource.Rate;
            cl_rate.Width = 35;
            listView1.Columns.Add(cl_rate);
        }
        private void LoadSerials()
        {
            LoadSerials(string.Empty);
        }
        private void LoadSerials(string filter)
        {
            ModelContainer mc = new ModelContainer();
            foreach (Serials film in mc.SerialsSet.Where(f => f.UsersId == User.Id && f.isDeleted != true && (string.IsNullOrEmpty(filter) ? 1 == 1 : f.Name.Contains(filter))).OrderByDescending(t => t.DateLast))
            {
                LoadItemsToListView(film);
            }
        }
        private void LoadFilms()
        {
            LoadFilms(string.Empty);
        }
        private void LoadFilms(string filter)
        {
            ModelContainer mc = new ModelContainer();
            foreach (Films film in mc.FilmsSet.Where(f => f.UsersId == User.Id && f.isDeleted != true && (string.IsNullOrEmpty(filter) ? 1 == 1 : f.Name.Contains(filter))).OrderByDescending(t => t.DateSee))
            {
                LoadItemsToListView(film);
            }
        }
        private void LoadItemsToListView()
        {
            listView1.Items.Clear();
            if (CurrentDB == DBMode.Films)
            {
                LoadFilms();
            }
            else
            {
                LoadSerials();
            }
        }
        private void LoadItemsToListView(Serials film)
        {
            LoadItemsToListView(film, false);
        }
        private void LoadItemsToListView(Films film)
        {
            LoadItemsToListView(film, false);
        }
        private void LoadItemsToListView(Serials film, bool oneToTop)
        {
            if (oneToTop) listView1.Items.Insert(0, new ListViewItem(new string[] { film.Id.ToString(), film.Name, film.LastSeason.ToString() + "-" + film.LastSeries.ToString(),LibTools.Genres.GetById(film.Genre), film.DateLast.ToString(), film.DateBegin.ToString(), LibTools.Ratings.GetById(film.Rate) }));
            else listView1.Items.Add(new ListViewItem(new string[] { film.Id.ToString(), film.Name, film.LastSeason.ToString() + "-" + film.LastSeries.ToString(), LibTools.Genres.GetById(film.Genre), film.DateLast.ToString(), film.DateBegin.ToString(), LibTools.Ratings.GetById(film.Rate) }));

            toolStripStatusLabel2.Text = listView1.Items.Count.ToString();
        }
        private void LoadItemsToListView(Films film, bool oneToTop)
        {
            if (oneToTop) listView1.Items.Insert(0, new ListViewItem(new string[] { film.Id.ToString(), film.Name, LibTools.Genres.GetById(film.Genre), film.DateSee.ToString(), LibTools.Ratings.GetById(film.Rate) }));
            else listView1.Items.Add(new ListViewItem(new string[] { film.Id.ToString(), film.Name, LibTools.Genres.GetById(film.Genre), film.DateSee.ToString(), LibTools.Ratings.GetById(film.Rate) }));
            
            toolStripStatusLabel2.Text = listView1.Items.Count.ToString();
        }

        private void Edit()
        {
            if (listView1.SelectedItems.Count == 0) return;

            ListViewItem lvi = listView1.SelectedItems[0];

            if (CurrentDB == DBMode.Films)
            {
                Add_Film form = new Add_Film();
                form.User = user;
                form.EditData(lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text, lvi.SubItems[3].Text, lvi.SubItems[4].Text);
                form.ShowDialog();
                if (form.DelRecord)
                {
                    listView1.Items.Remove(lvi);
                }
                else if (form.NewFilm != null)
                {
                    ModelContainer mc = new ModelContainer();
                    int f_id = Convert.ToInt32(lvi.SubItems[0].Text);
                    Films film = mc.FilmsSet.First(f => f.Id == f_id);
                    film.UsersId = User.Id;
                    film.Name = form.NewFilm.Name;
                    film.DateSee = form.NewFilm.DateSee;
                    film.Rate = form.NewFilm.Rate;
                    film.DateChange = form.NewFilm.DateChange;
                    lvi.SubItems[1].Text = form.NewFilm.Name;
                    lvi.SubItems[2].Text = LibTools.Genres.GetById(form.NewFilm.Genre);
                    lvi.SubItems[3].Text = form.NewFilm.DateSee.ToString();
                    lvi.SubItems[4].Text = LibTools.Ratings.GetById(film.Rate);
                    mc.SaveChanges();
                }
                form.Close();
            }
            else
            {
                Add_Serial form = new Add_Serial();
                form.User = user;
                form.EditData(lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[5].Text, lvi.SubItems[6].Text, lvi.SubItems[2].Text.Split('-')[0], lvi.SubItems[2].Text.Split('-')[1], lvi.SubItems[3].Text);
                form.ShowDialog();
                if (form.DelRecord)
                {
                    listView1.Items.Remove(lvi);
                }
                else if (form.NewFilm != null)
                {
                    ModelContainer mc = new ModelContainer();
                    int f_id = Convert.ToInt32(lvi.SubItems[0].Text);
                    Serials film = mc.SerialsSet.First(f => f.Id == f_id);
                    film.UsersId = User.Id;
                    film.Name = form.NewFilm.Name;
                    film.LastSeason = form.NewFilm.LastSeason;
                    film.LastSeries = form.NewFilm.LastSeries;
                    film.DateBegin = form.NewFilm.DateBegin;
                    film.DateLast = form.NewFilm.DateLast;
                    film.Rate = form.NewFilm.Rate;
                    film.DateChange = form.NewFilm.DateChange;
                    lvi.SubItems[1].Text = form.NewFilm.Name;
                    lvi.SubItems[2].Text = form.NewFilm.LastSeason.ToString() + "-" + form.NewFilm.LastSeries.ToString();
                    lvi.SubItems[3].Text = LibTools.Genres.GetById(form.NewFilm.Genre);
                    lvi.SubItems[4].Text = form.NewFilm.DateLast.ToString();
                    lvi.SubItems[5].Text = form.NewFilm.DateBegin.ToString();
                    lvi.SubItems[6].Text = LibTools.Ratings.GetById(film.Rate);
                    mc.SaveChanges();
                }
                form.Close();
            }
        }
        private void Add()
        {
            if (CurrentDB == DBMode.Films)
            {
                Add_Film form = new Add_Film();
                form.User = user;
                form.ShowDialog();
                if (form.NewFilm != null)
                {
                    ModelContainer mc = new ModelContainer();
                    mc.FilmsSet.Add(form.NewFilm);
                    mc.SaveChanges();
                    LoadItemsToListView(form.NewFilm, true);
                }
                form.Close();
            }
            else
            {
                Add_Serial form = new Add_Serial();
                form.User = user;
                form.ShowDialog();
                if (form.NewFilm != null)
                {
                    ModelContainer mc = new ModelContainer();
                    mc.SerialsSet.Add(form.NewFilm);
                    mc.SaveChanges();
                    LoadItemsToListView(form.NewFilm, true);
                }
                form.Close();
            }
        }

        private void ChangeMenus()
        {
            if (CurrentDB == DBMode.Films)
            {
                toolStripSeparator1.Visible = false;
                AddSeasonToolStripMenuItem.Visible = false;
                AddSeriesToolStripMenuItem.Visible = false;
                toolStripSeparator2.Visible = false;
                addSeasonToolStripMenuItem1.Visible = false;
                addSeriesToolStripMenuItem1.Visible = false;
            }
            else
            {
                toolStripSeparator1.Visible = true;
                AddSeasonToolStripMenuItem.Visible = true;
                AddSeriesToolStripMenuItem.Visible = true;
                toolStripSeparator2.Visible = true;
                addSeasonToolStripMenuItem1.Visible = true;
                addSeriesToolStripMenuItem1.Visible = true;
            }
        }
        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox1.Text == Resource.Films) CurrentDB = DBMode.Films;
            else CurrentDB = DBMode.Serials;

            UpdateListViewColumns();
            LoadItemsToListView();
            ChangeMenus();
        }
        private void fastFindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!FastFind)
            {
                FastFind = true;
                toolStripComboBox2.Visible = true;
                toolStripComboBox2.Text = "";
                toolStripComboBox2.Items.Clear();
                foreach (ListViewItem lvi in listView1.Items) toolStripComboBox2.Items.Add(lvi.SubItems[1].Text);
            }
            else
            {
                FastFind = false;
                toolStripComboBox2.Visible = false;
                toolStripComboBox2.Text = "";
            }
        }

        private void toolStripComboBox2_TextChanged(object sender, EventArgs e)
        {
            if (CurrentDB == DBMode.Films)
            {
                listView1.Items.Clear();
                LoadFilms(toolStripComboBox2.Text);
            }
            else
            {
                listView1.Items.Clear();
                LoadSerials(toolStripComboBox2.Text);
            }
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Add();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Edit();
        }
        private void addToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Add();
        }
        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private enum eFastUpdateSerial
        {
            Season,
            Series
        }
        private void FastUpdateSerial(eFastUpdateSerial m)
        {
            if (listView1.SelectedItems.Count == 0) return;
            ModelContainer mc = new ModelContainer();
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                int id = Convert.ToInt32(lvi.SubItems[0].Text);
                Serials film = mc.SerialsSet.First(f => f.Id == id);
                switch (m)
                {
                    case eFastUpdateSerial.Season: film.LastSeason += 1; break;
                    case eFastUpdateSerial.Series: film.LastSeries += 1; break;
                }
                film.DateChange = DateTime.Now;
                film.DateLast = DateTime.Now;
                lvi.SubItems[2].Text = film.LastSeason.ToString() + "-" + film.LastSeries.ToString();
                lvi.SubItems[3].Text = film.DateLast.ToString();
            }
            mc.SaveChanges();
        }
        private void addSeasonToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FastUpdateSerial(eFastUpdateSerial.Season);
        }
        private void addSeriesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FastUpdateSerial(eFastUpdateSerial.Series);
        }
        private void AddSeasonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FastUpdateSerial(eFastUpdateSerial.Season);
        }
        private void AddSeriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FastUpdateSerial(eFastUpdateSerial.Series);
        }
        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.KeyCode == Keys.Add) FastUpdateSerial(eFastUpdateSerial.Season);
            else  if (e.KeyCode == Keys.Add) FastUpdateSerial(eFastUpdateSerial.Series);
        }
        public static API_Data.FilmsRequestResponse Map(Films model)
        {
            if (model == null) return new API_Data.FilmsRequestResponse();

            return new API_Data.FilmsRequestResponse
            {
                IsFilm = true,
                Id = model.Id_R,
                Name = model.Name,
                DateChange = model.DateChange,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rate = model.Rate,
                isDeleted = model.isDeleted
            };
        }
        public static API_Data.FilmsRequestResponse Map(Serials model)
        {
            if (model == null) return new API_Data.FilmsRequestResponse();

            return new API_Data.FilmsRequestResponse
            {
                IsFilm = false,
                Id = model.Id_R,
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rate = model.Rate,
                DateBegin = model.DateBegin,
                DateLast = model.DateLast,
                LastSeason = model.LastSeason,
                LastSeries = model.LastSeries,
                isDeleted = model.isDeleted
            };
        }
        public static Films MapToFilm(API_Data.FilmsRequestResponse model, int user_id)
        {
            if (model == null) return new Films();

            return new Films
            {
                Id_R = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                DateSee = model.DateSee,
                Genre = model.Genre,
                Rate = model.Rate,
                isDeleted = model.isDeleted,
                UsersId = user_id
            };
        }
        public static Serials MapToSerial(API_Data.FilmsRequestResponse model, int user_id)
        {
            if (model == null) return new Serials();

            return new Serials
            {
                Id_R = model.Id,
                Name = model.Name,
                DateChange = model.DateChange,
                Genre = model.Genre,
                Rate = model.Rate,
                DateBegin = model.DateBegin,
                DateLast = model.DateLast,
                LastSeason = model.LastSeason,
                LastSeries = model.LastSeries,
                isDeleted = model.isDeleted,
                UsersId = user_id
            };
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            List<API_Data.FilmsRequestResponse> films = new List<API_Data.FilmsRequestResponse>();
            ModelContainer mc = new ModelContainer();

            //PUT NEW + UPDATED + DELETED
            films.AddRange(mc.FilmsSet.Where(f => f.UsersId == User.Id && f.DateChange != null).Select(Map));
            films.AddRange(mc.SerialsSet.Where(f => f.UsersId == User.Id && f.DateChange != null).Select(Map));
            WebRequest req;
            API_Data.RequestResponseAnswer answer;
            if (films.Count() != 0)
            {
                req = WebRequest.Create(API_Data.ApiHost + API_Data.ApiSync + MD5Tools.GetMd5Hash(User.Email.ToLower()) + "/" + ((int)API_Data.ModesApiFilms.PostNewUpdatedDeleted).ToString());
                req.Method = "POST";
                req.Credentials = CredentialCache.DefaultCredentials;
                ((HttpWebRequest)req).UserAgent = "MySeen";
                req.ContentType = "application/json";
                string postData = API_Data.SetResponse(films);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                req.ContentLength = byteArray.Length;
                Stream dataStream = req.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                answer = API_Data.GetResponseAnswer((new StreamReader(req.GetResponse().GetResponseStream())).ReadToEnd());
                req.GetResponse().Close();
                if (answer == null)
                {
                    MessageBox.Show(Resource.ApiError);
                    return;
                }
                //MessageBox.Show(answer.ToString());

                //DEL NEW
                mc.FilmsSet.RemoveRange(mc.FilmsSet.Where(f => f.UsersId == User.Id && f.Id_R == null));
                mc.SerialsSet.RemoveRange(mc.SerialsSet.Where(f => f.UsersId == User.Id && f.Id_R == null));
            }

            //GET NEW + UPDATED + DELETED

            req = WebRequest.Create(API_Data.ApiHost + API_Data.ApiSync + MD5Tools.GetMd5Hash(User.Email.ToLower()) + "/" + ((int)API_Data.ModesApiFilms.GetNewUpdatedDeleted).ToString());

            string data = (new StreamReader(req.GetResponse().GetResponseStream())).ReadToEnd();
            req.GetResponse().Close();
            //MessageBox.Show("data2=" + data);
            answer = API_Data.GetResponseAnswer(data);

            if (answer != null)
            {
                if (answer.Value == API_Data.RequestResponseAnswer.Values.UserNotExist)
                {
                    MessageBox.Show(Resource.UserNotExist);
                }
                else if (answer.Value == API_Data.RequestResponseAnswer.Values.BadRequestMode)
                {
                    MessageBox.Show(Resource.BadRequestMode);
                }
            }
            else
            {
                foreach (API_Data.FilmsRequestResponse film in API_Data.GetResponse(data))
                {
                    if (film.IsFilm)
                    {
                        if (mc.FilmsSet.Where(f => f.Id_R == film.Id && f.UsersId == User.Id).Count() != 0)//с таким ID есть в БД, обновим
                        {
                            var filmBD = mc.FilmsSet.Where(f => f.Id_R == film.Id && f.UsersId == User.Id).First();
                            if (filmBD.DateChange == null || filmBD.DateChange < film.DateChange)//есть не изменненый или изменен ранее чем обновляем
                            {
                                filmBD.DateChange = null;//на клиенте он актуальный, не будем отправлять ему
                                filmBD.DateSee = film.DateSee;
                                filmBD.Genre = film.Genre;
                                filmBD.isDeleted = film.isDeleted;
                                filmBD.Rate = film.Rate;
                                filmBD.Name = film.Name;
                            }
                        }
                        else //новый
                        {
                            mc.FilmsSet.Add(MapToFilm(film, User.Id));
                        }
                    }
                    else
                    {
                        if (mc.SerialsSet.Where(f => f.Id_R == film.Id && f.UsersId == User.Id).Count() != 0)//с таким ID есть в БД, обновим
                        {
                            var filmBD = mc.SerialsSet.Where(f => f.Id_R == film.Id && f.UsersId == User.Id).First();
                            if (filmBD.DateChange == null || filmBD.DateChange < film.DateChange)//есть не изменненый или изменен ранее чем обновляем
                            {
                                filmBD.DateChange = null;//на клиенте он актуальный, не будем отправлять ему
                                filmBD.Genre = film.Genre;
                                filmBD.isDeleted = film.isDeleted;
                                filmBD.Rate = film.Rate;
                                filmBD.DateBegin = film.DateBegin;
                                filmBD.DateLast = film.DateLast;
                                filmBD.LastSeason = film.LastSeason;
                                filmBD.LastSeries = film.LastSeries;
                                filmBD.Name = film.Name;
                            }
                        }
                        else
                        {
                            mc.SerialsSet.Add(MapToSerial(film, User.Id));
                        }
                    }
                }
            }
            mc.SaveChanges();
            //DEL DELETED
            mc.FilmsSet.RemoveRange(mc.FilmsSet.Where(f => f.UsersId == User.Id && f.isDeleted == true));
            mc.SerialsSet.RemoveRange(mc.SerialsSet.Where(f => f.UsersId == User.Id && f.isDeleted == true));
            //SET DC=NULL
            foreach (Films f in mc.FilmsSet.Where(f => f.UsersId == User.Id && f.DateChange != null))
            {
                f.DateChange = null;
            }
            foreach (Serials f in mc.SerialsSet.Where(f => f.UsersId == User.Id && f.DateChange != null))
            {
                f.DateChange = null;
            }
            mc.SaveChanges();
            LoadItemsToListView();
            MessageBox.Show(Resource.SyncOK);
        }
    }
}
