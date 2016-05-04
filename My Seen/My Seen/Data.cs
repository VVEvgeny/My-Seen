using System;
using System.Linq;
using System.Windows.Forms;
using MySeenLib;
using System.Threading;

namespace My_Seen
{
    public partial class Data : Form
    {
        private enum DbMode
        {
            Films,
            Serials,
            Books
        }
        private DbMode _currentDb = DbMode.Films;

        private bool _quickSearch;

        public Users User { private get; set; }

        public readonly MySeenEvent NeedRestartAppAfterDeleteUserEvent;
        public Data()
        {
            InitializeComponent();
            toolStripComboBox1.Items.Add(Resource.Films);
            toolStripComboBox1.Items.Add(Resource.Serials);
            toolStripComboBox1.Items.Add(Resource.Books);
            NeedRestartAppAfterDeleteUserEvent = new MySeenEvent();
        }

        private Thread _threadSync;
        private void Data_Load(object sender, EventArgs e)
        {
            _currentDb = DbMode.Films;
            toolStripComboBox1.Text = toolStripComboBox1.Items[0].ToString();
            _threadSync = new Thread(CheckCanSync);
            _threadSync.Start();
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
            form.DBDataChanged.Event += LoadItemsToListView;
            form.DBUserDeleted.Event += RestartProgram;
            form.DBUpdateUser.Event += UpdateUser;
            form.ShowDialog();
            if (_threadSync.ThreadState != ThreadState.Running)
            {
                _threadSync = new Thread(CheckCanSync);
                _threadSync.Start();
            }
        }
        private void UpdateUser()
        {
            var mc = new ModelContainer();
            User = mc.UsersSet.First(u => u.Id == User.Id);
        }
        private void UpdateListViewColumns()
        {
            listView1.Columns.Clear();

            ColumnHeader clId = new ColumnHeader
            {
                Text = "id",
                Width = 0
            };
            listView1.Columns.Add(clId);

            if (_currentDb == DbMode.Films)
            {
                var clName = new ColumnHeader
                {
                    Text = Resource.Name,
                    Width = 460
                };
                listView1.Columns.Add(clName);

                var clGenre = new ColumnHeader
                {
                    Text = Resource.Genre,
                    Width = 50
                };
                listView1.Columns.Add(clGenre);

                var clDate = new ColumnHeader
                {
                    Text = Resource.Date,
                    Width = 120
                };
                listView1.Columns.Add(clDate);
            }
            else if (_currentDb == DbMode.Serials)
            {
                var clName = new ColumnHeader
                {
                    Text = Resource.Name,
                    Width = 290
                };
                listView1.Columns.Add(clName);

                var clLastEp = new ColumnHeader
                {
                    Text = Resource.LastEpisode,
                    Width = 50
                };
                listView1.Columns.Add(clLastEp);

                var clGenre = new ColumnHeader
                {
                    Text = Resource.Genre,
                    Width = 50
                };
                listView1.Columns.Add(clGenre);

                var clDateLast = new ColumnHeader
                {
                    Text = Resource.DateLast,
                    Width = 120
                };
                listView1.Columns.Add(clDateLast);

                var clDateBegin = new ColumnHeader
                {
                    Text = Resource.DateBegin,
                    Width = 120
                };
                listView1.Columns.Add(clDateBegin);
            }
            else //books
            {
                var clName = new ColumnHeader
                {
                    Text = Resource.Name,
                    Width = 350
                };
                listView1.Columns.Add(clName);

                var clAuthor = new ColumnHeader
                {
                    Text = Resource.Author,
                    Width = 50
                };
                listView1.Columns.Add(clAuthor);

                var clGenre = new ColumnHeader
                {
                    Text = Resource.Genre,
                    Width = 50
                };
                listView1.Columns.Add(clGenre);

                var clDate = new ColumnHeader
                {
                    Text = Resource.Date,
                    Width = 120
                };
                listView1.Columns.Add(clDate);
            }
            var clRate = new ColumnHeader
            {
                Text = Resource.Rating,
                Width = 35
            };
            listView1.Columns.Add(clRate);
        }
        private void LoadBooks()
        {
            LoadBooks(string.Empty);
        }
        private void LoadBooks(string filter)
        {
            var mc = new ModelContainer();
            foreach (var film in mc.BooksSet.Where(f => f.UsersId == User.Id && f.isDeleted != true && (string.IsNullOrEmpty(filter) || f.Name.Contains(filter))).OrderByDescending(t => t.DateRead))
            {
                LoadItemsToListView(film);
            }
        }
        private void LoadSerials()
        {
            LoadSerials(string.Empty);
        }
        private void LoadSerials(string filter)
        {
            ModelContainer mc = new ModelContainer();
            foreach (Serials film in mc.SerialsSet.Where(f => f.UsersId == User.Id && f.isDeleted != true && (string.IsNullOrEmpty(filter) || f.Name.Contains(filter))).OrderByDescending(t => t.DateLast))
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
            foreach (Films film in mc.FilmsSet.Where(f => f.UsersId == User.Id && f.isDeleted != true && (string.IsNullOrEmpty(filter) || f.Name.Contains(filter))).OrderByDescending(t => t.DateSee))
            {
                LoadItemsToListView(film);
            }
        }
        private void LoadItemsToListView()
        {
            listView1.Items.Clear();
            if (_currentDb == DbMode.Films)
            {
                LoadFilms();
            }
            else if (_currentDb == DbMode.Serials)
            {
                LoadSerials();
            }
            else
            {
                LoadBooks();
            }
        }

        private void LoadItemsToListView(Books film, bool oneToTop = false)
        {
            var lvi = new ListViewItem(new string[] { film.Id.ToString(), film.Name, film.Authors, Defaults.Genres.GetById(film.Genre), UmtTime.From(film.DateRead).ToString(), Defaults.Ratings.GetById(film.Rating)});
            AddToList(lvi, oneToTop);
        }
        private void LoadItemsToListView(Serials film, bool oneToTop = false)
        {
            var lvi = new ListViewItem(new string[] { film.Id.ToString(), film.Name, film.LastSeason + "-" + film.LastSeries, Defaults.Genres.GetById(film.Genre), UmtTime.From(film.DateLast).ToString(), UmtTime.From(film.DateBegin).ToString(), Defaults.Ratings.GetById(film.Rating) });
            AddToList(lvi, oneToTop);
        }
        private void LoadItemsToListView(Films film, bool oneToTop = false)
        {
            var lvi = new ListViewItem(new string[] { film.Id.ToString(), film.Name, Defaults.Genres.GetById(film.Genre), UmtTime.From(film.DateSee).ToString(), Defaults.Ratings.GetById(film.Rating) });
            AddToList(lvi, oneToTop);
        }
        private void AddToList(ListViewItem lvi, bool oneToTop)
        {
            if (oneToTop) listView1.Items.Insert(0, lvi);
            else listView1.Items.Add(lvi);

            toolStripStatusLabel2.Text = listView1.Items.Count.ToString();
        }

        private void Edit()
        {
            if (listView1.SelectedItems.Count == 0) return;

            ListViewItem lvi = listView1.SelectedItems[0];

            if (_currentDb == DbMode.Films)
            {
                Add_Film form = new Add_Film();
                form.User = User;
                form.EditData(lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text, lvi.SubItems[3].Text, lvi.SubItems[4].Text);
                form.ShowDialog();

                if (form.NewFilm != null)
                {
                    ModelContainer mc = new ModelContainer();
                    int f_id = Convert.ToInt32(lvi.SubItems[0].Text);
                    Films film = mc.FilmsSet.First(f => f.Id == f_id);
                    film.UsersId = User.Id;
                    film.Name = form.NewFilm.Name;
                    film.DateSee = UmtTime.To(form.NewFilm.DateSee);
                    film.Rating = form.NewFilm.Rating;
                    film.DateChange = UmtTime.To(form.NewFilm.DateChange);
                    if (form.DelRecord)
                    {
                        film.isDeleted = true;
                        listView1.Items.Remove(lvi);
                    }
                    else
                    {
                        lvi.SubItems[1].Text = form.NewFilm.Name;
                        lvi.SubItems[2].Text = Defaults.Genres.GetById(form.NewFilm.Genre);
                        lvi.SubItems[3].Text = form.NewFilm.DateSee.ToString();
                        lvi.SubItems[4].Text = Defaults.Ratings.GetById(film.Rating);
                    }
                    mc.SaveChanges();
                }
                form.Close();
            }
            else if (_currentDb == DbMode.Serials)
            {
                Add_Serial form = new Add_Serial();
                form.User = User;
                form.EditData(lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[5].Text, lvi.SubItems[6].Text, lvi.SubItems[2].Text.Split('-')[0], lvi.SubItems[2].Text.Split('-')[1], lvi.SubItems[3].Text);
                form.ShowDialog();
                if (form.NewFilm != null)
                {
                    ModelContainer mc = new ModelContainer();
                    int f_id = Convert.ToInt32(lvi.SubItems[0].Text);
                    Serials film = mc.SerialsSet.First(f => f.Id == f_id);
                    film.UsersId = User.Id;
                    film.Name = form.NewFilm.Name;
                    film.LastSeason = form.NewFilm.LastSeason;
                    film.LastSeries = form.NewFilm.LastSeries;
                    film.DateBegin = UmtTime.To(form.NewFilm.DateBegin);
                    film.DateLast = UmtTime.To(form.NewFilm.DateLast);
                    film.Rating = form.NewFilm.Rating;
                    film.DateChange = UmtTime.To(form.NewFilm.DateChange);
                    if (form.DelRecord)
                    {
                        film.isDeleted = true;
                        listView1.Items.Remove(lvi);
                    }
                    else
                    {
                        lvi.SubItems[1].Text = form.NewFilm.Name;
                        lvi.SubItems[2].Text = form.NewFilm.LastSeason.ToString() + "-" + form.NewFilm.LastSeries.ToString();
                        lvi.SubItems[3].Text = Defaults.Genres.GetById(form.NewFilm.Genre);
                        lvi.SubItems[4].Text = form.NewFilm.DateLast.ToString();
                        lvi.SubItems[5].Text = form.NewFilm.DateBegin.ToString();
                        lvi.SubItems[6].Text = Defaults.Ratings.GetById(film.Rating);
                    }
                    mc.SaveChanges();
                }
                form.Close();
            }
            else
            {
                AddBook form = new AddBook();
                form.User = User;

                form.EditData(lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[4].Text, lvi.SubItems[5].Text, lvi.SubItems[2].Text, lvi.SubItems[3].Text);
                form.ShowDialog();
                if (form.NewFilm != null)
                {
                    ModelContainer mc = new ModelContainer();
                    int f_id = Convert.ToInt32(lvi.SubItems[0].Text);
                    Books film = mc.BooksSet.First(f => f.Id == f_id);
                    film.UsersId = User.Id;
                    film.Name = form.NewFilm.Name;
                    film.DateRead = UmtTime.To(form.NewFilm.DateRead);
                    film.Authors = form.NewFilm.Authors;
                    film.Rating = form.NewFilm.Rating;
                    film.DateChange = UmtTime.To(form.NewFilm.DateChange);
                    if (form.DelRecord)
                    {
                        film.isDeleted = true;
                        listView1.Items.Remove(lvi);
                    }
                    else
                    {
                        lvi.SubItems[1].Text = form.NewFilm.Name;
                        lvi.SubItems[2].Text = form.NewFilm.Authors;
                        lvi.SubItems[3].Text = Defaults.Genres.GetById(form.NewFilm.Genre);
                        lvi.SubItems[4].Text = form.NewFilm.DateRead.ToString();
                        lvi.SubItems[5].Text = Defaults.Ratings.GetById(film.Rating);
                    }
                    mc.SaveChanges();
                }
                form.Close();
            }
        }
        private void Add()
        {
            if (_currentDb == DbMode.Films)
            {
                Add_Film form = new Add_Film();
                form.User = User;
                form.ShowDialog();
                if (form.NewFilm != null)
                {
                    ModelContainer mc = new ModelContainer();
                    form.NewFilm.DateSee = UmtTime.To(form.NewFilm.DateSee);
                    form.NewFilm.DateChange = UmtTime.To(form.NewFilm.DateChange);
                    mc.FilmsSet.Add(form.NewFilm);
                    mc.SaveChanges();
                    LoadItemsToListView(form.NewFilm, true);
                }
                form.Close();
            }
            else if (_currentDb == DbMode.Serials)
            {
                Add_Serial form = new Add_Serial();
                form.User = User;
                form.ShowDialog();
                if (form.NewFilm != null)
                {
                    ModelContainer mc = new ModelContainer();
                    form.NewFilm.DateChange = UmtTime.To(form.NewFilm.DateChange);
                    form.NewFilm.DateBegin = UmtTime.To(form.NewFilm.DateBegin);
                    form.NewFilm.DateLast = UmtTime.To(form.NewFilm.DateLast);
                    mc.SerialsSet.Add(form.NewFilm);
                    mc.SaveChanges();
                    LoadItemsToListView(form.NewFilm, true);
                }
                form.Close();
            }
            else
            {
                AddBook form = new AddBook();
                form.User = User;
                form.ShowDialog();
                if (form.NewFilm != null)
                {
                    ModelContainer mc = new ModelContainer();
                    form.NewFilm.DateChange = UmtTime.To(form.NewFilm.DateChange);
                    form.NewFilm.DateRead = UmtTime.To(form.NewFilm.DateRead);
                    mc.BooksSet.Add(form.NewFilm);
                    mc.SaveChanges();
                    LoadItemsToListView(form.NewFilm, true);
                }
                form.Close();
            }
        }

        private void ChangeMenus()
        {
            if (_currentDb == DbMode.Films || _currentDb==DbMode.Books)
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
            if (toolStripComboBox1.Text == Resource.Films) _currentDb = DbMode.Films;
            else if(toolStripComboBox1.Text == Resource.Serials) _currentDb = DbMode.Serials;
            else _currentDb = DbMode.Books;

            UpdateListViewColumns();
            LoadItemsToListView();
            ChangeMenus();
        }
        private void quickSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!_quickSearch)
            {
                _quickSearch = true;
                toolStripComboBox2.Visible = true;
                toolStripComboBox2.Text = "";
                toolStripComboBox2.Items.Clear();
                foreach (ListViewItem lvi in listView1.Items) toolStripComboBox2.Items.Add(lvi.SubItems[1].Text);
            }
            else
            {
                _quickSearch = false;
                toolStripComboBox2.Visible = false;
                toolStripComboBox2.Text = "";
            }
        }

        private void toolStripComboBox2_TextChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "0";
            listView1.Items.Clear();
            if (_currentDb == DbMode.Films)
            {
                LoadFilms(toolStripComboBox2.Text);
            }
            else if (_currentDb == DbMode.Serials)
            {
                LoadSerials(toolStripComboBox2.Text);
            }
            else
            {
                LoadBooks(toolStripComboBox2.Text);
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
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (WebApi.Sync(User)) LoadItemsToListView();
        }
    }
}
