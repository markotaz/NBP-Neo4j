using Neo4j.DomainModel;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neo4j
{
    public partial class FormSeeMovies : Form
    {
        public String VideoClubName { get; set; }
        public FormUser FormUser { get; set; }
        public bool Flag { get; set; }
        public FormSeeMovies()
        {
            InitializeComponent();
        }
        public FormSeeMovies(String name, FormUser fu, bool flag)
        {
            InitializeComponent();
            this.VideoClubName = name;
            this.FormUser = fu;
            this.Flag = flag;
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void FormSeeMovies_Load(object sender, EventArgs e)
        {
            if (Flag == true)
            {
                List<Movie> movies = new List<Movie>();
                using (var session = DataProvider.Driver.Session())
                {
                    session.ReadTransaction(tx => movies = DataProvider.Instance.GetMoviesFromVideoClub(tx, VideoClubName));
                    foreach (Movie m in movies)
                        bindingSource1.Add(m);
                }
            }
            else
            {
                List<Movie> movies = new List<Movie>();
                using (var session = DataProvider.Driver.Session())
                {
                    movies = session.ReadTransaction(tx => DomainModel.Movie.ParseMovies(DataProvider.Instance.GetRecommendation(tx, DataProvider.Instance.User)));
                    
                    foreach (Movie m in movies)
                        bindingSource1.Add(m);
                }
            }
        }

        private void FormSeeMovies_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormUser.Close();
        }
    }
}
