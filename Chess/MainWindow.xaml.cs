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
using static System.Net.Mime.MediaTypeNames;

namespace Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            Loadcbx();
        }
        void Loadcbx()
        {
            for (int i = 3; i <= 7; i++)
            {
                CollumnsCountCbx.Items.Add(i);
                RowsCountCbx.Items.Add(i);
            }
            CollumnsCountCbx.SelectedIndex = 2;
            RowsCountCbx.SelectedIndex = 2;
        }


        void ClearField()
        {
            Field.Children.Clear();
            Field.RowDefinitions.Clear();
            Field.ColumnDefinitions.Clear(); 
        }

        void looadField()
        {
            for (int i = 0; i < int.Parse(RowsCountCbx.SelectedItem.ToString()); i++)
            {
                RowDefinition r1 = new RowDefinition();
                
                Field.RowDefinitions.Add(r1);
                for (int j = 0; j < int.Parse(CollumnsCountCbx.SelectedItem.ToString()); j++)
                {
                    ColumnDefinition c1 = new ColumnDefinition();
                    Field.ColumnDefinitions.Add(c1);
                }
            }
        }

        private void InitializeMinefield()
        {
            Random random = new Random();
            for (int i = 0; i < int.Parse(RowsCountCbx.SelectedItem.ToString()); i++)
            {
                for (int j = 0; j < int.Parse(CollumnsCountCbx.SelectedItem.ToString()); j++)
                {
                   
                }
            }
        }

        void Cell_Click(object sender, RoutedEventArgs e)
        {

        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearField();
            looadField();

        }
    }
}
