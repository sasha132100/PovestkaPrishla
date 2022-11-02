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
using System.Windows.Shapes;

namespace Povestka_
{
    /// <summary>
    /// Логика взаимодействия для WindowViewKids.xaml
    /// </summary>
    public partial class WindowViewKids : Window
    {
        public WindowViewKids()
        {
            InitializeComponent();
            LoadContent();
        }

        void LoadContent()
        {
            using (var db = new YouthLeisureEntities())
            {
                List<Kid> kids;
                try
                {
                    kids = (from t in db.Kid select t).ToList<Kid>();
                    int i = 0;
                    foreach (var kid in kids)
                    {
                        AddNewUser(i, kid.FullName, kid.Age.ToString(), kid.CurrentGroup);
                        i++;
                    }
                }
                catch
                {

                }
            }
        }

        void AddNewUser(int i, string FullName, string Age, string CurrentGroup)
        {
            var borderPanel = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(2), Style = (Style)UserView.Resources["contentBorderStyle"] };
            var mainGrid = new Grid() { };
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength() });

            StackPanel sp = new StackPanel() { };
            Grid.SetColumn(sp, 0);
            TextBlock TxtLogin = new TextBlock() { Text = "ФИО: ", Style = (Style)UserView.Resources["Lbl"], Margin = new Thickness(0, 0, 0, 0) };
            TextBlock TxtEmail = new TextBlock() { Text = "Возраст: ", Style = (Style)UserView.Resources["Lbl"], Margin = new Thickness(0, 5, 0, 0) };
            TextBlock TxtRole = new TextBlock() { Text = "Текущая группа: ", Style = (Style)UserView.Resources["Lbl"], Margin = new Thickness(0, 5, 0, 0) };
            TxtLogin.Inlines.Add(new TextBlock() { Text = $" {FullName}", Foreground = (Brush)(new BrushConverter().ConvertFrom("Black")), Margin = new Thickness(0) });
            TxtEmail.Inlines.Add(new TextBlock() { Text = $" {Age}", Foreground = (Brush)(new BrushConverter().ConvertFrom("Black")), Margin = new Thickness(0) });
            TxtRole.Inlines.Add(new TextBlock() { Text = $" {CurrentGroup}", Foreground = (Brush)(new BrushConverter().ConvertFrom("Black")), Margin = new Thickness(0) });
            WrapPanel wp = new WrapPanel() { };
            Button changeBtn = new Button() { Width = 81, Height = 23, Content = "Изменить", Foreground = Brushes.Black, FontWeight = FontWeights.Bold, Cursor = Cursors.Hand };
            changeBtn.Style = (Style)UserView.Resources["RoundedButtonStyle"];
            Grid.SetColumn(changeBtn, 1);
            changeBtn.Name = FullName;
            changeBtn.Click += ChangeButtonOnClick;

            sp.Children.Add(TxtLogin);
            sp.Children.Add(TxtEmail);
            sp.Children.Add(TxtRole);
            mainGrid.Children.Add(sp);
            mainGrid.Children.Add(changeBtn);
            borderPanel.Child = mainGrid;
            UserView.Children.Add(borderPanel);
        }

        private void ChangeButtonOnClick(object sender, EventArgs eventArgsm)
        {
            var button = (Button)sender;
            if (button != null)
            {
                using (var db = new YouthLeisureEntities())
                {
                    SystemContext.Kid = (from k in db.Kid where k.FullName == button.Name select k).FirstOrDefault();
                    WindowChangeKid windowChangeKid = new WindowChangeKid();
                    this.Close();
                    windowChangeKid.ShowDialog();
                }
            }
        }

        private void BackToMainWindow_Button(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.ShowDialog();
        }
    }
}
