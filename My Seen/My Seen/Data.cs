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

        public Data()
        {
            InitializeComponent();
            toolStripComboBox1.Items.Add(Resource.Films);
            toolStripComboBox1.Items.Add(Resource.Serials);
        }

        private void Data_Load(object sender, EventArgs e)
        {
            CurrentDB = DBMode.Films;
            toolStripComboBox1.Text = toolStripComboBox1.Items[0].ToString();
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config form = new Config();
            form.User = User;
            form.ShowDialog();
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
                cl_name.Width = 510;
                listView1.Columns.Add(cl_name);

                ColumnHeader cl_date = new ColumnHeader();
                cl_date.Text = Resource.Date;
                cl_date.Width = 120;
                listView1.Columns.Add(cl_date);
            }
            else
            {
                ColumnHeader cl_name = new ColumnHeader();
                cl_name.Text = Resource.Name;
                cl_name.Width = 340;
                listView1.Columns.Add(cl_name);

                ColumnHeader cl_last_ep = new ColumnHeader();
                cl_last_ep.Text = Resource.LastEpisode;
                cl_last_ep.Width = 50;
                listView1.Columns.Add(cl_last_ep);

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
            foreach (Serials film in mc.SerialsSet.Where(f => f.UsersId == User.Id && filter == string.Empty ? 1 == 1 : f.Name.Contains(filter)).OrderByDescending(t => t.DateLast))
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
            foreach (Films film in mc.FilmsSet.Where(f => f.UsersId == User.Id && filter == string.Empty ? 1 == 1 : f.Name.Contains(filter)).OrderByDescending(t => t.DateSee))
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
            if (oneToTop) listView1.Items.Insert(0, new ListViewItem(new string[] { film.Id.ToString(), film.Name, film.LastSeason.ToString()+"-"+film.LastSeries.ToString(),film.DateLast.ToString(),film.DateBegin.ToString(), film.Rate.ToString() }));
            else listView1.Items.Add(new ListViewItem(new string[] { film.Id.ToString(), film.Name, film.LastSeason.ToString() + "-" + film.LastSeries.ToString(), film.DateLast.ToString(), film.DateBegin.ToString(), film.Rate.ToString() }));

            toolStripStatusLabel2.Text = listView1.Items.Count.ToString();
        }
        private void LoadItemsToListView(Films film, bool oneToTop)
        {
            if (oneToTop) listView1.Items.Insert(0, new ListViewItem(new string[] { film.Id.ToString(), film.Name, film.DateSee.ToString(), film.Rate.ToString() }));
            else listView1.Items.Add(new ListViewItem(new string[] { film.Id.ToString(), film.Name, film.DateSee.ToString(), film.Rate.ToString() }));
            
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
                form.EditData(lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text, lvi.SubItems[3].Text);
                form.ShowDialog();
                if (form.NewFilm != null)
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
                    lvi.SubItems[2].Text = form.NewFilm.DateSee.ToString();
                    lvi.SubItems[3].Text = form.NewFilm.Rate.ToString();
                    mc.SaveChanges();
                }
                form.Close();
            }
            else
            {
                Add_Serial form = new Add_Serial();
                form.User = user;
                form.EditData(lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[4].Text, lvi.SubItems[5].Text, lvi.SubItems[2].Text.Split('-')[0], lvi.SubItems[2].Text.Split('-')[1]);
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
                    film.DateBegin = form.NewFilm.DateBegin;
                    film.DateLast = form.NewFilm.DateLast;
                    film.Rate = form.NewFilm.Rate;
                    film.DateChange = form.NewFilm.DateChange;
                    lvi.SubItems[1].Text = form.NewFilm.Name;
                    lvi.SubItems[2].Text = form.NewFilm.LastSeason.ToString() + "-" + form.NewFilm.LastSeries.ToString();
                    lvi.SubItems[3].Text = form.NewFilm.DateLast.ToString();
                    lvi.SubItems[4].Text = form.NewFilm.DateBegin.ToString();
                    lvi.SubItems[5].Text = form.NewFilm.Rate.ToString();
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
    }
}
