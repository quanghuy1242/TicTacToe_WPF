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

namespace Caro3x3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        char[] board = { '*', '*', '*', '*', '*', '*', '*', '*', '*' };
        int bestVal;

        public MainWindow()
        {
            InitializeComponent();
            foreach(var btn in gridMain.Children.OfType<Button>())
            {
                btn.Click += Button_Click;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string text;
            try
            {
                text = button.Content.ToString();
            }
            catch
            {
                text = "";
            }

            if (text != "")
            {
                return;
            }
            button.Content = "o";
            board[Convert.ToInt32(button.Tag) - 1] = 'o';

            if(txtChedo.Text == "KHÓ")
            {
                minimax(1, true);
                board[bestVal] = 'x';

                foreach (var btn in gridMain.Children.OfType<Button>())
                {
                    if (Convert.ToInt32(btn.Tag) == (bestVal + 1))
                    {
                        btn.Content = 'x';
                    }
                }
            }
            if(txtChedo.Text == "DỄ" && !hasMoveLeft())
            {
                int r = ramdomnum();
                board[r - 1] = 'x';
                foreach (var btn in gridMain.Children.OfType<Button>())
                {
                    if (Convert.ToInt32(btn.Tag) == r)
                    {
                        btn.Content = 'x';
                    }
                }
            }
            

            changecolor();

            if (checkEndGame('o'))
            {
                txtStatus.Text = "Thắng";
                txtBan.Text = (Convert.ToInt32(txtBan.Text) + 10).ToString();
                txtMay.Text = (Convert.ToInt32(txtMay.Text) - 5).ToString();
                tatnut();
                return;

            }
            if (checkEndGame('x'))
            {
                txtStatus.Text = "Thua";
                txtMay.Text = (Convert.ToInt32(txtMay.Text) + 10).ToString();
                txtBan.Text = (Convert.ToInt32(txtBan.Text) - 5).ToString();
                tatnut();
                return;

            }
            if (hasMoveLeft())
            {
                txtStatus.Text = "Hoà";
                txtBan.Text = (Convert.ToInt32(txtBan.Text) - 5).ToString();
                txtMay.Text = (Convert.ToInt32(txtMay.Text) - 5).ToString();
                tatnut();
                return;

            }

        }

        int ramdomnum()
        {

            int a = Convert.ToInt32(new Random().Next(9) + 1);
            while (!checkiftrung(a))
            {
                a = Convert.ToInt32(new Random().Next(9) + 1);
            }
            return a;
        }

        bool checkiftrung(int b)
        {
            foreach (var btn in gridMain.Children.OfType<Button>())
            {
                if (btn.Tag.ToString() == b.ToString())
                {
                    if (btn.Content.ToString() != "") return false;
                }
            }
            return true;

        }

        void tatnut()
        {
            foreach (var btn in gridMain.Children.OfType<Button>())
            {
                btn.IsEnabled = false;
            }
        }

        void batnut()
        {
            foreach (var btn in gridMain.Children.OfType<Button>())
            {
                btn.IsEnabled = true;
            }
        }

        void InitialBoard()
        {
            txtStatus.Text = "";

            foreach (var btn in gridMain.Children.OfType<Button>())
            {
                btn.Content = "";
            }
            for (int i = 0; i < 9; i++)
            {
                board[i] = '*';
            }
            batnut();
        }

        void changecolor()
        {
            foreach (var btnn in gridMain.Children.OfType<Button>())
            {
                try
                {
                    if (btnn.Content.ToString() == "o")
                    {
                        btnn.Foreground = Brushes.Black;
                    }

                    if (btnn.Content.ToString() == "x")
                    {
                        btnn.Foreground = Brushes.White;
                    }
                }
                catch
                {

                }
                
            }
        }

        public bool hasMoveLeft()
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[i] != 'x' && board[i] != 'o')
                {
                    return false;
                }
            }
            return true;
        }
        
        bool checkEndGame(char s)
        {
            for (int i = 0; i < 9; i += 3)
            {
                if ((board[i] == board[i + 1]) && (board[i + 1] == board[i + 2]) && (board[i] == s))
                    return true;
            }
            for (int i = 0; i < 3; i++)
            {
                if ((board[i] == board[i + 3]) && (board[i + 3] == board[i + 6]) && (board[i] == s))
                    return true;
            }
            if ((board[0] == board[4]) && (board[4] == board[8]) && (board[0] == s))
            {
                return true;
            }
            if ((board[2] == board[4]) && (board[4] == board[6]) && (board[2] == s))
            {
                return true;
            }
            return false;
        }

        int minimax(int dept, bool flag)
        {
            int max_val = -1000, min_val = 1000;
            int value = 1, i, j;
            if (checkEndGame('x')) return 10 - dept;
            if (checkEndGame('o')) return -10 - dept;
            if (hasMoveLeft()) return 0 - dept;

            int[] score = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            for (i = 0; i < 9; i++)
            {
                if (board[i] == '*')
                {
                    if (flag)
                    {
                        board[i] = 'x';
                        value = minimax(dept + 1, false);
                    }
                    else
                    {
                        board[i] = 'o';
                        value = minimax(dept + 1, true);
                    }
                    board[i] = '*';
                    score[i] = value;
                }
            }

            if (flag)
            {
                max_val = -1000;
                for (j = 0; j < 9; j++)
                {
                    if (score[j] > max_val && score[j] != 1)
                    {
                        max_val = score[j];
                        bestVal = j;
                    }
                }
                return max_val;
            }
            else
            {
                min_val = 1000;
                for (j = 0; j < 9; j++)
                {
                    if (score[j] < min_val && score[j] != 1)
                    {
                        min_val = score[j];
                        bestVal = j;
                    }
                }
                return min_val;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            InitialBoard();
        }

        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(txtChedo.Text == "KHÓ")
            {
                txtChedo.Text = "DỄ";
            }
            else
            {
                txtChedo.Text = "KHÓ";
            }

            InitialBoard();
        }
    }
}
