using Neo4j.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neo4j.Driver;

namespace Neo4j
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            DataProvider.Instance.MainForm = this;
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            User user = null;
            if (textPassword.Text == textBoxRepeatPassword.Text)
            {
                using (var session = DataProvider.Driver.Session())
                {
                    session.ReadTransaction(tx => user = DataProvider.Instance.GetUser(tx, textEmail.Text));
                    if (user == null)
                    {
                        User newUser = new User(textIme.Text, textPrezime.Text, textBrTel.Text, textEmail.Text, textPassword.Text);
                        session.WriteTransaction(tx => DataProvider.Instance.CreateUser(tx, newUser));
                        DataProvider.Instance.User = newUser;
                        FormUser fu = new FormUser();
                        this.Hide();
                        fu.Show();
                    }
                    else
                        MessageBox.Show("User with this email already exists.");
                }
            }else
                MessageBox.Show("Your passwords don't match. Try again");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User user = null;
            using (var session = DataProvider.Driver.Session())
            {
                session.ReadTransaction(tx => user = DataProvider.Instance.GetUser(tx, textBox1.Text));
                if (user != null && user.Password==textBox2.Text)
                {
                    FormUser fu = new FormUser();
                    this.Hide();
                    fu.Show();
                }else
                    MessageBox.Show("User with this email and password doesn't exist."); 
                

            }
            DataProvider.Instance.User = user;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormAdmin fa = new FormAdmin();
            this.Hide();
            fa.Show();
        }
    }
}
