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
using System.IO;
using Microsoft.Win32;
using Word = Microsoft.Office.Interop.Word;


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
            WindowAddNewTeacher windowAddNewTeacher = new WindowAddNewTeacher();
            this.Close();
            windowAddNewTeacher.ShowDialog();
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
            List<Teacher> allTeachers;
            using (YouthLeisureEntities db = new YouthLeisureEntities())
            {
                allTeachers = db.Teacher.ToList();
            }

            var app = new Word.Application();
            Word.Document document = app.Documents.Add();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table employeeTable = document.Tables.Add(tableRange, allTeachers.Count() + 1, 9);
            employeeTable.Borders.InsideLineStyle = employeeTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            employeeTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;
            cellRange = employeeTable.Cell(1, 1).Range;
            cellRange.Text = "ФИО";
            cellRange = employeeTable.Cell(1, 2).Range;
            cellRange.Text = "Паспортные данные";
            cellRange = employeeTable.Cell(1, 3).Range;
            cellRange.Text = "Дата рождения";
            cellRange = employeeTable.Cell(1, 4).Range;
            cellRange.Text = "Пол";
            cellRange = employeeTable.Cell(1, 5).Range;
            cellRange.Text = "Семейное положение";
            cellRange = employeeTable.Cell(1, 6).Range;
            cellRange.Text = "Образование";
            cellRange = employeeTable.Cell(1, 7).Range;
            cellRange.Text = "Адрес проживания";
            cellRange = employeeTable.Cell(1, 8).Range;
            cellRange.Text = "Домашний телефон";
            cellRange = employeeTable.Cell(1, 9).Range;
            cellRange.Text = "Специализация(и)";
            employeeTable.Rows[1].Range.Bold = 1;
            employeeTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            int k = 1;
            foreach (var teacher in allTeachers)
            {
                cellRange = employeeTable.Cell(k + 1, 1).Range;
                cellRange.Text = teacher.FullName;
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                cellRange = employeeTable.Cell(k + 1, 2).Range;
                cellRange.Text = teacher.Pasport;
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                cellRange = employeeTable.Cell(k + 1, 3).Range;
                cellRange.Text = teacher.DateOfBirth.ToString();
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                cellRange = employeeTable.Cell(k + 1, 4).Range;
                cellRange.Text = teacher.Gender;
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                cellRange = employeeTable.Cell(k + 1, 5).Range;
                cellRange.Text = teacher.MaritalStatus;
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                cellRange = employeeTable.Cell(k + 1, 6).Range;
                cellRange.Text = teacher.Education;
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                cellRange = employeeTable.Cell(k + 1, 7).Range;
                cellRange.Text = teacher.Address_;
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                cellRange = employeeTable.Cell(k + 1, 8).Range;
                cellRange.Text = teacher.HomeNumber;
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                cellRange = employeeTable.Cell(k + 1, 9).Range;
                cellRange.Text = teacher.Specialization_s;
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                k++;
            }
            app.Visible = true;
            document.SaveAs2(@"D:\Lab 3 Word\outputFile.docx");
            document.SaveAs2(@"D:\Lab 3 Word\outputFile.pdf", Word.WdExportFormat.wdExportFormatPDF);
        }
    }
}
