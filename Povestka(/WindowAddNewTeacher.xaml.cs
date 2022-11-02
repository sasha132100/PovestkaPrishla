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
    /// Логика взаимодействия для WindowAddNewTeacher.xaml
    /// </summary>
    public partial class WindowAddNewTeacher : Window
    {
        public WindowAddNewTeacher()
        {
            InitializeComponent();
        }

        private void BackToAdminWindow_Button(object sender, RoutedEventArgs e)
        {
            WindowAdmin windowAdmin = new WindowAdmin();
            this.Close();
            windowAdmin.ShowDialog();
        }

        private void SaveNewTeacher_Button(object sender, RoutedEventArgs e)
        {
            if (FullNameBox.Text == "")
            {
                MessageBox.Show("Введите ФИО");
                return;
            }
            if (PasportBox.Text == "")
            {
                MessageBox.Show("Введите паспортные данные");
                return;
            }
            if (DateOfBirthBox.Text == "")
            {
                MessageBox.Show("Введите дату рождения");
                return;
            }
            if (GenderBox.Text == "")
            {
                MessageBox.Show("Введите пол");
                return;
            }
            if (MaritalStatusBox.Text == "")
            {
                MessageBox.Show("Введите семейное положение");
                return;
            }
            if (EducationBox.Text == "")
            {
                MessageBox.Show("Введите образование");
                return;
            }
            if (AddressBox.Text == "")
            {
                MessageBox.Show("Введите адрес");
                return;
            }
            if (HomeNumberBox.Text == "")
            {
                MessageBox.Show("Введите домашний телефон");
                return;
            }
            if (SpecializationsBox.Text == "")
            {
                MessageBox.Show("Введите специализацию");
                return;
            }
            try
            {
                using (var db = new YouthLeisureEntities())
                {
                    Teacher teacher = new Teacher
                    {
                        FullName = FullNameBox.Text,
                        Pasport = PasportBox.Text,
                        DateOfBirth = DateTime.Parse(DateOfBirthBox.Text),
                        Gender = GenderBox.Text,
                        MaritalStatus = MaritalStatusBox.Text,
                        Education = EducationBox.Text,
                        Address_ = AddressBox.Text,
                        HomeNumber = HomeNumberBox.Text,
                        Specialization_s = SpecializationsBox.Text
                    };
                    db.Teacher.Add(teacher);
                    db.SaveChanges();
                }
                MessageBox.Show("Пользователь успешно добавлен");
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при добавлении. Измените введенные данные");
            }
        }
    }
}
