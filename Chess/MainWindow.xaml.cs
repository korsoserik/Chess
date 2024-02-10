using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace Chess
{

    public partial class MainWindow : Window
    {
        private bool[,] minefield;
        private int ColumnsCount;
        private int RowsCount;
        private Button[,] buttonsGrid;
        private int MinesCount;
        bool intialsetup = true;
        private List<Button> mineButtons;
        private Button[,] buttons;





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
            RowsCount = int.Parse(RowsCountCbx.SelectedItem.ToString());
            ColumnsCount = int.Parse(CollumnsCountCbx.SelectedItem.ToString());


            for (int i = 1; i <= RowsCount * ColumnsCount - 1; i++)
            {
                MinesCountCbx.Items.Add(i);
            }
            intialsetup = false;

        }


        void UpdateRows()
        {
            RowsCount = int.Parse(RowsCountCbx.SelectedItem.ToString());
            ColumnsCount = int.Parse(CollumnsCountCbx.SelectedItem.ToString());
        }

        void minesCbxchange()
        {

            UpdateRows();
            MinesCountCbx.Items.Clear();
            for (int i = 1; i <= RowsCount * ColumnsCount - 1; i++)
            {
                MinesCountCbx.Items.Add(i);
            }
            MinesCountCbx.SelectedItem = 1;

        }


        void ClearField()
        {
            Field.Children.Clear();
            Field.RowDefinitions.Clear();
            Field.ColumnDefinitions.Clear();

        }

        void LoadField()
        {

            MinesCount = int.Parse(MinesCountCbx.SelectedItem.ToString());
            minefield = new bool[RowsCount, ColumnsCount];

            UniformGrid uniformGrid = new UniformGrid();
            uniformGrid.Rows = RowsCount;
            uniformGrid.Columns = ColumnsCount;
            buttonsGrid = new Button[RowsCount, ColumnsCount];
            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {

                    Button button = new Button();
                    button.Click += Cell_Click;
                    button.Tag = new Tuple<int, int>(i, j);
                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
                    button.VerticalAlignment = VerticalAlignment.Stretch;
                    button.HorizontalContentAlignment = HorizontalAlignment.Center;
                    button.VerticalContentAlignment = VerticalAlignment.Center;
                    button.Style = FindResource("MineButtonStyle") as Style;

                    uniformGrid.Children.Add(button);
                    buttonsGrid[i, j] = button;
                }
            }

            Field.Children.Add(uniformGrid);

            InitializeMinefield(MinesCount);
        }

        private void InitializeMinefield(int mineCount)
        {


            Random random = new Random();
            mineButtons = new List<Button>();

            int minesPlaced = 0;

            while (minesPlaced < mineCount)
            {
                int row = random.Next(minefield.GetLength(0));
                int col = random.Next(minefield.GetLength(1));

                if (!minefield[row, col])
                {
                    minefield[row, col] = true;
                    minesPlaced++;

                    Button button = GetButtonAt(row, col);
                    mineButtons.Add(button);


                }
            }
        }


        private Button GetButtonAt(int row, int col)
        {
            UniformGrid uniformGrid = (UniformGrid)Field.Children[0];

            int index = row * uniformGrid.Columns + col;
            if (index < uniformGrid.Children.Count)
            {
                return uniformGrid.Children[index] as Button;
            }

            return null;
        }




        private void GameOver()
        {

            foreach (Button button in buttonsGrid)
            {
                if (button.Tag != null)
                {
                    ShowMine(button);
                }

            }

            MessageBox.Show("Game Over! All mines revealed.");
        }




        private void ShowMine(Button button)
        {

            if (!mineButtons.Contains(button))
            {
                button.Content = new Image
                {
                    Source = new BitmapImage(new Uri($"{Environment.CurrentDirectory}\\diamond.png")),
                    Stretch = Stretch.Fill,
                    Width = button.ActualWidth,
                    Height = button.ActualHeight

                };

                button.Background = Brushes.Green;
                button.Foreground = Brushes.Green;
                button.BorderBrush = Brushes.Green;

            }
            else
            {
                button.Content = new Image
                {
                    Source = new BitmapImage(new Uri($"{Environment.CurrentDirectory}\\bomb.png")),
                    Stretch = Stretch.Fill,
                    Width = button.ActualWidth,
                    Height = button.ActualHeight

                };

                button.Background = Brushes.Gray;
                button.Foreground = Brushes.White;
            }


        }



        void Cell_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            Tuple<int, int> coordinates = (Tuple<int, int>)clickedButton.Tag;
            int row = coordinates.Item1;
            int col = coordinates.Item2;

            if (minefield[row, col])
            {
                GameOver();
            }
            else
            {
                ShowMine(clickedButton);

            }
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearField();
            LoadField();
        }

        private void MinesCountCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RowsCountCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (intialsetup)
            {

            }
            else
            {
                minesCbxchange();
            }

        }

        private void CollumnsCountCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (intialsetup)
            {

            }
            else
            {
                minesCbxchange();
            }
        }
    }
}
