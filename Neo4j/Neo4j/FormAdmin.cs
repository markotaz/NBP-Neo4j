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
    public partial class FormAdmin : Form
    {
        public FormAdmin()
        {
            InitializeComponent();
            dateTimePickerActor.Format = DateTimePickerFormat.Custom;
            dateTimePickerActor.CustomFormat = "dd-MM-yyyy";
        }

        private void FormAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataProvider.Instance.MainForm.Close();
        }

        private void buttonAddMovie_Click(object sender, EventArgs e)
        {
            Movie m = null;
            using (var session = DataProvider.Driver.Session())
            {
                session.ReadTransaction(tx => m = DataProvider.Instance.GetMovie(tx, textBoxTitleDelete.Text));
                if (m == null)
                {
                    m = new Movie(textBoxTitle.Text, textBoxReleaseYear.Text, textBoxRate.Text);
                    session.WriteTransaction(tx => DataProvider.Instance.CreateMovie(tx, m));
                    MessageBox.Show("You have successfully added new movie.");
                }else
                    MessageBox.Show("Movie already exists.");

            }
        }

        private void buttonDeleteMovie_Click(object sender, EventArgs e)
        {
            Movie m = null;
            using (var session = DataProvider.Driver.Session())
            {
                session.ReadTransaction(tx => m = DataProvider.Instance.GetMovie(tx, textBoxTitleDelete.Text));
                if(m != null)
                {
                    session.WriteTransaction(tx => DataProvider.Instance.DeleteMovie(tx,m));
                    MessageBox.Show("You have successfully deleted a movie.");
                }else
                    MessageBox.Show("Movie doesn't exist.");
            }
        }

        private void buttonUpdateMovie_Click(object sender, EventArgs e)
        {
            Movie m = null;
            using (var session = DataProvider.Driver.Session())
            {
                session.ReadTransaction(tx => m = DataProvider.Instance.GetMovie(tx, textBoxTitleUpdate.Text));
                if (m != null)
                {
                    session.WriteTransaction(tx => DataProvider.Instance.UpdateMovie(tx, m, textBoxUpdateRate.Text));
                    MessageBox.Show("You have successfully updated a movie imdb rate.");
                }else
                    MessageBox.Show("Movie doesn't exist.");
            }
        }

        private void buttonAddActor_Click(object sender, EventArgs e)
        {
            Actor a = null;
            using (var session = DataProvider.Driver.Session())
            {
                a = session.ReadTransaction(tx => DataProvider.Instance.GetActor(tx, textBoxActorFirstName.Text, textBoxActorLastName.Text));
                if (a == null)
                {
                    a = new Actor(textBoxActorFirstName.Text, textBoxActorLastName.Text, dateTimePickerActor.Value.ToString());
                    session.WriteTransaction(tx => DataProvider.Instance.CreateActor(tx, a));
                    MessageBox.Show("You have successfully added new actor.");
                }else
                    MessageBox.Show("Actor already exists.");
            }
        }

        private void buttonRemoveActor_Click(object sender, EventArgs e)
        {
            Actor a = null;
            using (var session = DataProvider.Driver.Session())
            {
                a = session.ReadTransaction(tx => DataProvider.Instance.GetActor(tx, textBoxFName.Text, textBoxLName.Text));
                if (a != null)
                {
                    session.WriteTransaction(tx => DataProvider.Instance.DeleteActor(tx, textBoxFName.Text, textBoxLName.Text));
                    MessageBox.Show("You have successfully removed an actor.");
                }
                else
                    MessageBox.Show("Actor doesn't exist.");
            }
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            VideoClub vc = null;
            using (var session = DataProvider.Driver.Session())
            {
                vc = session.ReadTransaction(tx => DataProvider.Instance.GetVideoClub(tx, textBoxName.Text));
                if (vc == null)
                {
                    vc = new VideoClub(textBoxName.Text, textBoxCity.Text, textBoxAdress.Text, textBoxEmail.Text, textBoxPhone.Text);
                    session.WriteTransaction(tx => DataProvider.Instance.CreateVideoClub(tx, vc));
                    MessageBox.Show("You have successfully added a new video club.");
                }else
                    MessageBox.Show("Video club already exists.");
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            VideoClub vc = null;
            using (var session = DataProvider.Driver.Session())
            {
                vc = session.ReadTransaction(tx => DataProvider.Instance.GetVideoClub(tx, textBoxFindName.Text));
                if (vc != null)
                {
                    session.WriteTransaction(tx => DataProvider.Instance.UpdateVideoClub(tx, textBoxFindName.Text,
                    textBoxUpdateAdress.Text, textBoxUpdateEmail.Text, textBoxUpdatePhone.Text));
                    MessageBox.Show("You have successfully updated video club.");
                }else
                    MessageBox.Show("Video club doesn't exist.");
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            VideoClub vc = null;
            using (var session = DataProvider.Driver.Session())
            {
                vc = session.ReadTransaction(tx => DataProvider.Instance.GetVideoClub(tx, textBoxRemoveName.Text));
                if (vc != null)
                {
                    session.WriteTransaction(tx => DataProvider.Instance.DeleteVideoClub(tx, textBoxRemoveName.Text));
                    MessageBox.Show("You have successfully deleted video club.");
                }else
                    MessageBox.Show("Video club doesn't exist.");
            }
        }

        private void buttonAddGenre_Click(object sender, EventArgs e)
        {
            Genre g = null;
            using (var session = DataProvider.Driver.Session())
            {
                g = session.ReadTransaction(tx => DataProvider.Instance.GetGenre(tx, textBoxGenreName.Text));

                if (g == null)
                {
                    session.WriteTransaction(tx => DataProvider.Instance.CreateGenre(tx, textBoxGenreName.Text));
                    MessageBox.Show("You have successfully created new genre.");
                }
                else
                    MessageBox.Show("Genre already exists.");
            }
        }

        private void buttonAddMovieToVc_Click(object sender, EventArgs e)
        {
            VideoClub vc = null;
            Movie m = null; 
            using (var session = DataProvider.Driver.Session())
            {
                vc = session.ReadTransaction(tx => DataProvider.Instance.GetVideoClub(tx, textBoxVcName.Text));
                session.ReadTransaction(tx => m = DataProvider.Instance.GetMovie(tx, textBoxMovieTitle.Text));
                if (m != null && vc != null)
                {
                    session.WriteTransaction(tx => DataProvider.Instance.AddMovieToVideoClub(tx, textBoxVcName.Text, textBoxMovieTitle.Text));
                    MessageBox.Show("You have successfully added movie to the video club.");
                }else
                    MessageBox.Show("Video club or movie doesn't exist.");
            }
        }

        private void buttonAddMovieGenre_Click(object sender, EventArgs e)
        {
            Genre g = null;
            Movie m = null;
            using (var session = DataProvider.Driver.Session())
            {
                g = session.ReadTransaction(tx => DataProvider.Instance.GetGenre(tx, textBoxMovieGenreName.Text));
                m = session.ReadTransaction(tx => DataProvider.Instance.GetMovie(tx, textBoxMovieTitleGenre.Text));
                if (g != null && m != null)
                {
                    session.WriteTransaction(tx => DataProvider.Instance.AddMovieGenre(tx, textBoxMovieGenreName.Text, textBoxMovieTitleGenre.Text));
                    MessageBox.Show("You have successfully added genre to the movie " + textBoxMovieTitleGenre.Text + ".");
                }else
                    MessageBox.Show("Movie or genre doesn't exist.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Actor a = null;
            Movie m = null;
            using (var session = DataProvider.Driver.Session())
            {
                a = session.ReadTransaction(tx => DataProvider.Instance.GetActor(tx, textBoxFirstNameActor.Text, textBoxLastNameActor.Text));
                m = session.ReadTransaction(tx => DataProvider.Instance.GetMovie(tx, textBoxMovieTitleActor.Text));
                if (a != null && m!=null)
                {
                    session.WriteTransaction(tx => DataProvider.Instance.AddActorToMovie(tx, textBoxFirstNameActor.Text, textBoxLastNameActor.Text, textBoxMovieTitleActor.Text));
                    MessageBox.Show("You have successfully added that actor " + textBoxFirstNameActor.Text + " " + textBoxLastNameActor.Text +
                        " played in " + textBoxMovieTitleActor.Text + ".");
                }
                else
                    MessageBox.Show("Actor or movie doesn't exist.");
            }
        }
    }
}
