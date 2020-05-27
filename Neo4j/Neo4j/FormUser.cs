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
    public partial class FormUser : Form
    {
        public FormUser()
        {
            InitializeComponent();
          
        }
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            User user = null;
            using (var session = DataProvider.Driver.Session())
            {
                session.ReadTransaction(tx => user = DataProvider.Instance.GetUser(tx, textBoxEmail.Text));
                if (user != null && user.Password == textBoxPassword.Text)
                {
                    session.WriteTransaction(tx => DataProvider.Instance.UpdateUser(tx, DataProvider.Instance.User, textBoxEmail.Text, textBoxPassword.Text));
                    MessageBox.Show("You have successfully updated your informations.");
                }
                else
                    MessageBox.Show("You entered wrong email or password. Try again.");
            }
        }

        private void FormUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataProvider.Instance.MainForm.Close();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            User user = null; 
            using (var session = DataProvider.Driver.Session())
            {
                session.ReadTransaction(tx => user = DataProvider.Instance.GetUser(tx, textBoxEmail.Text));
                if (user != null && user.Password == textBoxPassword.Text)
                {
                    session.WriteTransaction(tx => DataProvider.Instance.DeleteUser(tx, DataProvider.Instance.User));
                    MessageBox.Show("You have successfully deleted your informations.");
                    this.Close();
                }
                else
                    MessageBox.Show("You entered wrong email or password. Try again.");
            }
        }

        private void buttonAddMovie_Click(object sender, EventArgs e)
        {
            if (radioButtonYes.Checked)
            {
                Movie m = null; 
                using (var session = DataProvider.Driver.Session())
                {
                    m = session.ReadTransaction(tx => DataProvider.Instance.GetMovie(tx, textBoxTitle.Text));
                    if (m != null)
                    {
                        session.WriteTransaction(tx => DataProvider.Instance.AddFavouriteMovie(tx, DataProvider.Instance.User, textBoxTitle.Text, "yes"));
                        MessageBox.Show("You have successfully added new favourite movie.");
                    }else
                        MessageBox.Show("Movie doesn't exist.");

                }
            }
            else
            {
                Movie m = null;
                using (var session = DataProvider.Driver.Session())
                {
                    m = session.ReadTransaction(tx => DataProvider.Instance.GetMovie(tx, textBoxTitle.Text));
                    if (m != null)
                    {
                        session.WriteTransaction(tx => DataProvider.Instance.AddFavouriteMovie(tx, DataProvider.Instance.User, textBoxTitle.Text, "no"));
                        MessageBox.Show("You have successfully added watched movie.");
                    }
                    else
                        MessageBox.Show("Movie doesn't exist.");

                }
            }
        }

        private void buttonJoin_Click(object sender, EventArgs e)
        {
            VideoClub vc = null;
            using (var session = DataProvider.Driver.Session())
            {
                vc = session.ReadTransaction(tx => DataProvider.Instance.GetVideoClub(tx, textBoxClubName.Text));
                if (vc != null)
                {
                    session.WriteTransaction(tx => DataProvider.Instance.JoinVideoClub(tx, DataProvider.Instance.User, textBoxClubName.Text));
                    MessageBox.Show("You have successfully joined video club " + textBoxClubName.Text);
                }else
                    MessageBox.Show("Video club doesn't exist.");
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Actor a = null;
            using (var session = DataProvider.Driver.Session())
            {
                a = session.ReadTransaction(tx => DataProvider.Instance.GetActor(tx, textBoxFirstName.Text, textBoxLastName.Text));
                if (a != null)
                {
                    session.WriteTransaction(tx => DataProvider.Instance.AddFavouriteActor(tx, DataProvider.Instance.User, textBoxFirstName.Text, textBoxLastName.Text));
                    MessageBox.Show("You have successfully added favourite actor.");
                }else
                    MessageBox.Show("Actor doesn't exist.");
            }
        }

        private void buttonAllMovies_Click(object sender, EventArgs e)
        {
            FormSeeMovies fsm = new FormSeeMovies(textBoxVideoClubMovies.Text, this, true);
            this.Hide();
            fsm.Show();
        }

        private void buttonRent_Click(object sender, EventArgs e)
        {
            VideoClub vc = null;
            Movie m = null;
            using (var session = DataProvider.Driver.Session())
            {
                vc = session.ReadTransaction(tx => DataProvider.Instance.GetVideoClub(tx, textBoxVideoClub.Text));
                m = session.ReadTransaction(tx => DataProvider.Instance.GetMovie(tx, textBoxMovie.Text));
                if (vc != null && m!=null)
                {
                    session.WriteTransaction(tx => DataProvider.Instance.RentMovie(tx, textBoxVideoClub.Text, textBoxMovie.Text));
                    session.WriteTransaction(tx => DataProvider.Instance.RentRelation(tx, DataProvider.Instance.User, textBoxMovie.Text));
                    MessageBox.Show("You have successfully rented movie " + textBoxMovie.Text + ".");
                }else
                    MessageBox.Show("Video club or movie doesn't exist.");
            }
        }

        private void buttonRecommendation_Click(object sender, EventArgs e)
        {
            FormSeeMovies fsm = new FormSeeMovies(textBoxVideoClubMovies.Text, this,false);
            this.Hide();
            fsm.Show();
        }
    }
}
