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
                    MainWindow main = new MainWindow(users[index]);
                    this.Close();
                    main.ShowDialog();

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

        }
    }
}
