using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            for (int i = 0; i < 12+1; i++)
            {
                
            }

            InitializeComponent();
        }

        private bool[,] minefield = new bool[5, 5];

        void looadField()
        {

        }

        private void InitializeMinefield()
        {
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    minefield[i, j] = random.Next(5) == 0; // 1 in 5 chance of having a mine
                }
            }
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
