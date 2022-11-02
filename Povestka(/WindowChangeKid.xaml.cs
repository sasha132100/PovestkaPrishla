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
    /// Логика взаимодействия для WindowChangeKid.xaml
    /// </summary>
    public partial class WindowChangeKid : Window
    {
        public WindowChangeKid()
        {
            InitializeComponent();
            LoadContext();
        }

        private void LoadContext()
        {
            using (var db = new YouthLeisureEntities())
            {
                Kid kid = (from k in db.Kid where k.FullName == SystemContext.Kid.FullName select k).FirstOrDefault();
                FullNameBox.Text = kid.FullName;
                AgeBox.Text = kid.Age.ToString();
                NumberOfSchoolBox.Text = kid.SchoolNumber;
                ClassBox.Text = kid.Class;
                BirthCertificateBox.Text = kid.BirthCertificate;
                AddressBox.Text = kid.Address_;
                HomeNumberBox.Text = kid.HomeNumber;
                ParentsInfoBox.Text = kid.ParentsInfo;
                CurrentGroupBox.Text = kid.CurrentGroup;
                StartDateBox.Text = kid.StartDateOfTheVisit.ToString();
            }
        }

        private void SaveChanges_Button(object sender, RoutedEventArgs e)
        {
            Kid kidInfo = SystemContext.Kid;
            if (FullNameBox.Text == "")
            {
                MessageBox.Show("Введите ФИО");
                return;
            }
            if (AgeBox.Text == "")
            {
                MessageBox.Show("Введите возраст");
                return;
            }
            if (BirthCertificateBox.Text == "")
            {
                MessageBox.Show("Введите данные свидетельства о рождении");
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
            if (ParentsInfoBox.Text == "")
            {
                MessageBox.Show("Введите информацию о родителях");
                return;
            }
            if (CurrentGroupBox.Text == "")
            {
                MessageBox.Show("Введите текущую группу"); 
                return;
            }
            if (StartDateBox.Text == "")
            {
                MessageBox.Show("Введите дату начала обучения в текущей группе");
                return;
            }
            if (kidInfo.FullName == FullNameBox.Text && kidInfo.Age.ToString() == AgeBox.Text && kidInfo.SchoolNumber == NumberOfSchoolBox.Text && kidInfo.Class == ClassBox.Text && kidInfo.BirthCertificate == BirthCertificateBox.Text && kidInfo.Address_ == AddressBox.Text && kidInfo.HomeNumber == HomeNumberBox.Text && kidInfo.ParentsInfo == ParentsInfoBox.Text && CurrentGroupBox.Text == kidInfo.CurrentGroup && kidInfo.StartDateOfTheVisit.ToString() == StartDateBox.Text)
            {
                MessageBox.Show("Измненений не было внесено!");
                return;
            }
            try
            {
                using (var db = new YouthLeisureEntities())
                {
                    Kid kid = new Kid
                    {
                        FullName = FullNameBox.Text,
                        Age = Int32.Parse(AgeBox.Text),
                        SchoolNumber = NumberOfSchoolBox.Text,
                        Class = ClassBox.Text,
                        BirthCertificate = BirthCertificateBox.Text,
                        Address_ = AddressBox.Text,
                        HomeNumber = HomeNumberBox.Text,
                        ParentsInfo = ParentsInfoBox.Text,
                        CurrentGroup = CurrentGroupBox.Text,
                        StartDateOfTheVisit = DateTime.Parse(StartDateBox.Text)
                    };
                    db.Entry(kid).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                MessageBox.Show("Данные ребенка успешно изменены");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("Произошла ошибка при добавлении. Измените введенные данные");
            }
        }

        private void BackToKidWindow_Button(object sender, RoutedEventArgs e)
        {
            WindowViewKids windowViewKids = new WindowViewKids();
            this.Close();
            windowViewKids.ShowDialog();
        }
    }
}
