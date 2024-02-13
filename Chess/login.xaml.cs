using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chess
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        ObservableCollection<user> users = new ObservableCollection<user>();
        public login()
        {
            InitializeComponent();
            fileRead();
        }

        void fileRead()
        {
            StreamReader sr = new StreamReader("Logininformation.csv");
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                users.Add(new user(sr.ReadLine()));
            }
            sr.Close();

        }

        private void UserLoginBTN_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            bool létezike = false;
            foreach (var item in users)
            {

                if (item.name == UsernameTXB.Text && item.password == PasswordTXB.Password)
                {
                    létezike = true;
                    MainWindow main = new MainWindow(users[index], users);
                    this.Close();
                    main.ShowDialog();
                    break;
                }
                index++;
            }
            if (!létezike)
            {
                MessageBox.Show("This user does not exist!");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            int maxId = 0;
            foreach (var item in users)
            {
                if (item.id >= maxId)
                {
                    maxId = item.id + 1;
                }
            }
            bool korrektFelhasznaloAdatok = true;
            if (!String.IsNullOrEmpty(UsernameTXB.Text) && !String.IsNullOrEmpty(PasswordTXB.Password))
            {
                foreach (var user in users)
                {
                    if (user.name == UsernameTXB.Text)
                    {
                        MessageBox.Show("This username is already in use!");
                        korrektFelhasznaloAdatok = false;
                    }

                }


                if (korrektFelhasznaloAdatok)
                {
                    StreamWriter sw = new StreamWriter("Logininformation.csv");
                    sw.WriteLine("Userid;Username;Password;Money");
                    foreach (var item in users)
                    {
                        sw.WriteLine($"{item.id};{item.name};{item.password};{item.money};{item.freereward}");
                    }
                    sw.WriteLine($"{maxId};{UsernameTXB.Text};{PasswordTXB.Password};10000;{DateTime.Now}");
                    sw.Close();
                    MessageBox.Show("Successful registration!");
                    users.Clear();
                    fileRead();
                    int index = 0;
                    foreach (var item in users)
                    {

                        if (item.name == UsernameTXB.Text && item.password == PasswordTXB.Password)
                        {
                            MainWindow main = new MainWindow(users[index], users);
                            this.Close();
                            main.ShowDialog();
                            break;
                        }
                        index++;
                    }
                }
            }
            else
            {
                MessageBox.Show("All fields must be filled!");
            }
        }
    }
}
