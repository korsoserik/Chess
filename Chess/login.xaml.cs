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

                if (item.name == UsernameTXB.Text && item.password == PasswordTXB.Text)
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
                MessageBox.Show("Nem létezik ilyen felhasználo");
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
            if (!String.IsNullOrEmpty(UsernameTXB.Text) && !String.IsNullOrEmpty(PasswordTXB.Text))
            {
                foreach (var user in users)
                {
                    if (user.name == UsernameTXB.Text)
                    {
                        MessageBox.Show("Már létezik ilyen névvel felhasználó!");
                        korrektFelhasznaloAdatok = false;
                    }

                }


                if (korrektFelhasznaloAdatok)
                {
                    StreamWriter sw = new StreamWriter("Logininformation.csv");
                    sw.WriteLine("Userid;Username;Password;Money");
                    foreach (var item in users)
                    {
                        sw.WriteLine($"{item.id};{item.name};{item.password};{item.money}");
                    }
                    sw.WriteLine($"{maxId};{UsernameTXB.Text};{PasswordTXB.Text};10000");
                    sw.Close();
                    MessageBox.Show("sikeres regisztrácio");
                    UsernameTXB.Text = "";
                    PasswordTXB.Text = "";
                    users.Clear();
                    fileRead();
                    int index = 0;
                    foreach (var item in users)
                    {

                        if (item.name == UsernameTXB.Text && item.password == PasswordTXB.Text)
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
                MessageBox.Show("Minden mező kitöltése kötelező!");
            }
        }
    }
}
