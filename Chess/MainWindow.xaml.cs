using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
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
        private int clickedButtonsCount;
        private int gemsLeft;
        private decimal multiplier = 0;
        user currentuser;
        ObservableCollection<user> users;


        private enum GameState
        {
            Start,
            Cashout
        }
        private GameState currentGameState;

        public MainWindow(user cuser, ObservableCollection<user> users)
        {
            InitializeComponent();
            this.currentuser = cuser;
            this.users = users;
            Loadcbx();
            minesCbxchange();
            UpdateWallet();
            BetAmountTxb.Text = $"{200.00:C}";
            currentGameState = GameState.Start;

        }

        private void UpdateWallet()
        {
            walletTxb.Text = $"{currentuser.money.ToString():C}";
        }

        private void CalculateCashout()
        {
            decimal tilesCount = ColumnsCount * RowsCount;
            multiplier = multiplier + (MinesCount / (tilesCount - clickedButtonsCount));


            decimal cashoutValue = decimal.Parse(BetAmountTxb.Text, NumberStyles.Currency) * multiplier;


            currentuser.money += cashoutValue;
            MessageBox.Show($"You cashed out with {clickedButtonsCount} gems. Cashout Value: {cashoutValue:C}. New Wallet Balance: {currentuser.money:C}");


            currentGameState = GameState.Start;
            ToggleButtonText();
            GemsLeftLbl.Text = "0";
            profitTxb.Text = $"{0:C}";
        }

        private void ToggleButtonText()
        {
            startBtn.Content = (currentGameState == GameState.Start) ? "Start" : "Cashout";
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {

            if (currentGameState == GameState.Start)
            {

                ClearField();
                LoadField();
                gemsLeft = RowsCount * ColumnsCount - MinesCount - clickedButtonsCount;
                GemsLeftLbl.Text = gemsLeft.ToString();
                DisableSettings();
                currentuser.money -= decimal.Parse(BetAmountTxb.Text.Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol, string.Empty));
                UpdateWallet();

                currentGameState = GameState.Cashout;
            }
            else if (currentGameState == GameState.Cashout)
            {

                if (clickedButtonsCount == 0)
                {
                    MessageBox.Show("Click at least one mine to cashout!");
                }
                else
                {
                    CalculateCashout();
                    EnableSettings();
                    RevalAllMines();
                    GemsLeftLbl.Text = "0";
                    UpdateWallet();
                }
            }

            ToggleButtonText();
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
            multiplier = 0;
            clickedButtonsCount = 0;
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

        private void EnableSettings()
        {
            MinesCountCbx.IsHitTestVisible = true;
            CollumnsCountCbx.IsHitTestVisible = true;
            RowsCountCbx.IsHitTestVisible = true;
            BetAmountTxb.IsHitTestVisible = true;
        }

        private void DisableSettings()
        {
            MinesCountCbx.IsHitTestVisible = false;
            CollumnsCountCbx.IsHitTestVisible = false;
            RowsCountCbx.IsHitTestVisible = false;
            BetAmountTxb.IsHitTestVisible = false;
        }

        private void RevalAllMines()
        {
            foreach (Button button in buttonsGrid)
            {
                if (button.Tag != null)
                {
                    ShowMine(button);
                }

            }
        }

        private void GameOver()
        {


            RevalAllMines();
            GemsLeftLbl.Text = "0";
            currentGameState = GameState.Start;
            EnableSettings();
            ToggleButtonText();
            profitTxb.Text = $"{0:C}";
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

                clickedButtonsCount += 1;
                button.Background = Brushes.Green;
                button.Foreground = Brushes.Green;
                button.BorderBrush = Brushes.Green;
                gemsLeft = RowsCount * ColumnsCount - MinesCount - clickedButtonsCount;
                GemsLeftLbl.Text = gemsLeft.ToString();
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

                GemsLeftLbl.Text = "0";
                button.Background = Brushes.Gray;
                button.Foreground = Brushes.White;
            }

            button.IsHitTestVisible = false;
        }

        private void UpdateProfit()
        {

            decimal tilesCount = ColumnsCount * RowsCount;
            multiplier += 1 + (MinesCount / (tilesCount - clickedButtonsCount));


            decimal cashoutValue = decimal.Parse(BetAmountTxb.Text, NumberStyles.Currency) * multiplier;
            profitTxb.Text = $"{Math.Round(cashoutValue, 2):C}";
        }

        private void Win()
        {
            RevalAllMines();
            GemsLeftLbl.Text = "0";
            currentGameState = GameState.Start;
            EnableSettings();
            ToggleButtonText();
            currentuser.money += decimal.Parse(profitTxb.Text, NumberStyles.Currency);
            walletTxb.Text = $"{currentuser.money:C}";
            profitTxb.Text = $"{0:C}";
            MessageBox.Show($"You have found all {clickedButtonsCount - 1} gem(s). You won: {profitTxb.Text}. New Wallet Balance: {currentuser.money:C}");
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
                UpdateProfit();
                if (gemsLeft == 0)
                {
                    Win();
                }



            }
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

        private void CurrencyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if (!Regex.IsMatch(e.Text, @"^[0-9]*$"))
            {
                e.Handled = true;
            }
        }

        private void CurrencyTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

            UpdateCurrencyTextBoxText();
        }

        private void UpdateCurrencyTextBoxText()
        {

            if (decimal.TryParse(BetAmountTxb.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal value))
            {

                value = Math.Max(200, Math.Min(currentuser.money, value));
                BetAmountTxb.Text = value.ToString("C", CultureInfo.CurrentCulture);
            }
            else
            {

                BetAmountTxb.Text = "200.00";
            }
        }
        private void CurrencyTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Remove the currency symbol when the TextBox gains focus
            if (sender is TextBox textBox)
            {
                textBox.Text = textBox.Text.Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol, string.Empty);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            int index = 0;
            foreach (var item in users)
            {
                if (item.id == currentuser.id)
                {
                    break;
                }
                index++;
            }
            users.RemoveAt(index);
            users.Add(currentuser);
            StreamWriter sw = new StreamWriter("Logininformation.csv");
            sw.WriteLine("Userid;Username;Password;Money");
            foreach (var item in users)
            {
                sw.WriteLine($"{item.id};{item.name};{item.password};{item.money};{item.freereward}");
            }
            sw.Close();
        }

        private void freereward_Click(object sender, RoutedEventArgs e)
        {
            var mins = (DateTime.Now - currentuser.freereward).TotalMinutes;
            if (mins > 15 )
            {
                currentuser.money += 1000;
                currentuser.freereward = DateTime.Now;
                UpdateWallet();
                MessageBox.Show("You have claimed your reward of 1000 coins");
            }
            else
            {
                int time = 15 - (int)mins;
                MessageBox.Show($"You have to wait {time} minutes to claim your reward");
            }
        }
    }
}
