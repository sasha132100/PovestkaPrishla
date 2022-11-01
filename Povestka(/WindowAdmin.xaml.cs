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
    /// Логика взаимодействия для WindowAdmin.xaml
    /// </summary>
    public partial class WindowAdmin : Window
    {
        public WindowAdmin()
        {
            InitializeComponent();
            LoadContent();
        }

        void LoadContent()
        {
            using (var db = new YouthLeisureEntities())
            {
                List<Teacher> teachers;
                try
                {
                    teachers = (from t in db.Teacher select t).ToList<Teacher>();
                    int i = 0;
                    foreach (var teacher in teachers)
                    {
                        AddNewUser(i, teacher.FullName, teacher.Education, teacher.Specialization_s);
                        i++;
                    }
                }
                catch
                {

                }
            }
        }

        void AddNewUser(int i, string FullName, string Education, string Specialization_s)
        {
            var borderPanel = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(2), Style = (Style)UserView.Resources["contentBorderStyle"] };
            var mainGrid = new Grid() { };
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength() });

            StackPanel sp = new StackPanel() { };
            Grid.SetColumn(sp, 0);
            TextBlock TxtLogin = new TextBlock() { Text = "ФИО: ", Style = (Style)UserView.Resources["Lbl"], Margin = new Thickness(0, 0, 0, 0) };
            TextBlock TxtEmail = new TextBlock() { Text = "Образование: ", Style = (Style)UserView.Resources["Lbl"], Margin = new Thickness(0, 5, 0, 0) };
            TextBlock TxtRole = new TextBlock() { Text = "Специализация(и): ", Style = (Style)UserView.Resources["Lbl"], Margin = new Thickness(0, 5, 0, 0) };
            TxtLogin.Inlines.Add(new TextBlock() { Text = $" {FullName}", Foreground = (Brush)(new BrushConverter().ConvertFrom("Black")), Margin = new Thickness(0) });
            TxtEmail.Inlines.Add(new TextBlock() { Text = $" {Education}", Foreground = (Brush)(new BrushConverter().ConvertFrom("Black")), Margin = new Thickness(0) });
            TxtRole.Inlines.Add(new TextBlock() { Text = $" {Specialization_s}", Foreground = (Brush)(new BrushConverter().ConvertFrom("Black")), Margin = new Thickness(0) });
            WrapPanel wp = new WrapPanel() { };
            Button deleteBtn = new Button() { Width = 81, Height = 23, Content = "Удалить", Foreground = Brushes.Black, FontWeight = FontWeights.Bold, Cursor = Cursors.Hand };
            deleteBtn.Style = (Style)UserView.Resources["RoundedButtonStyle"];
            Grid.SetColumn(deleteBtn, 1);
            deleteBtn.Name = FullName;
            deleteBtn.Click += DeleteButtonOnClick;

            sp.Children.Add(TxtLogin);
            sp.Children.Add(TxtEmail);
            sp.Children.Add(TxtRole);
            mainGrid.Children.Add(sp);
            mainGrid.Children.Add(deleteBtn);
            borderPanel.Child = mainGrid;
            UserView.Children.Add(borderPanel);
        }

        private void AddNewTeacher_Button(object sender, MouseButtonEventArgs e)
        {

        }

        private void BackToMainWindow_Button(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.ShowDialog();
        }

        private void DeleteButtonOnClick(object sender, EventArgs eventArgs)
        {
            var button = (Button)sender;
            if (button != null)
            {
                var result = MessageBox.Show("Вы действительно хотите удалить пользователя?", "Выход из программы", MessageBoxButton.YesNo);
                if (result.ToString() == "Yes")
                {
                    using (var db = new YouthLeisureEntities())
                    {
                        var teacher = db.Teacher.FirstOrDefault(x => x.FullName == button.Name);
                        if (teacher == null)
                        {
                            return;
                        }
                        db.Teacher.Remove(teacher);
                        db.SaveChanges();
                        UserView.Children.Clear();
                        LoadContent();
                    }
                }
            }
        }

        private void PDFOut_Button(object sender, EventArgs eventArgs)
        {

        }
    }
}
