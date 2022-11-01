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

namespace Povestka_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtLogin.Text = "Admin";
            txtPassword.Password = "qqqqwwww";
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text == "Admin" && txtPassword.Password == "qqqqwwww")
            {
                WindowAdmin windowAdmin = new WindowAdmin();
                this.Close();
                windowAdmin.ShowDialog();
            }
            else if (txtLogin.Text == "Viewer" && txtPassword.Password == "qqqqwwww")
            {
                WindowViewKids windowViewKids = new WindowViewKids();
                this.Close();
                windowViewKids.ShowDialog();
            }
            else
            {
                MessageBox.Show("Неверно введены данные", "Результат", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
