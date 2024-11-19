using Microsoft.Win32;
using Personal_Ivanov;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Personal_Ivanov
{
    /// <summary>
    
    /// </summary>
    public partial class AddEditPage : Page
    {
        public static SELLER currentSeller = new SELLER();
        public bool check = false;

        public AddEditPage(SELLER SelectedSeller)
        {
            InitializeComponent();
            if (SelectedSeller != null)
            {

                currentSeller = SelectedSeller;

                StringBuilder errors = new StringBuilder();
                ComboPosition.SelectedIndex = currentSeller.Position - 1;
                PointCBox.SelectedIndex = currentSeller.Trade_point - 1;
                DateDatapicker.SelectedDate = currentSeller.Work_date;
                BirthdayDateDatapicker.SelectedDate= currentSeller.Year_birthday;
                

            }
            DataContext = currentSeller;
        }
        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                currentSeller.Photo = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }
        private void SaveBtn_Click(Object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            currentSeller.Position = ComboPosition.SelectedIndex + 1;
            currentSeller.Trade_point = PointCBox.SelectedIndex + 1;
            currentSeller.Work_date = (DateTime)DateDatapicker.SelectedDate;
            currentSeller.Year_birthday = (DateTime)BirthdayDateDatapicker.SelectedDate;
            
            
            
            if (string.IsNullOrWhiteSpace(currentSeller.Surename))
                errors.AppendLine("Введите фамилию!");
            if (string.IsNullOrWhiteSpace(currentSeller.Name))
                errors.AppendLine("Введите имя!");
            if (string.IsNullOrWhiteSpace(currentSeller.Patronumic))
                errors.AppendLine("Введите отчество!");
            
            if (string.IsNullOrWhiteSpace(currentSeller.City))
                errors.AppendLine("Введите город!");
            if (string.IsNullOrWhiteSpace(currentSeller.Address))
                errors.AppendLine("Введите адрес!");


            if (string.IsNullOrWhiteSpace(currentSeller.Pasport_data))
                errors.AppendLine("Введите паспортные данные!");
            if (string.IsNullOrWhiteSpace(currentSeller.Phone_number))
                errors.AppendLine("Введите номер телефона!");
            if (string.IsNullOrWhiteSpace(currentSeller.Year_birthday.ToString()))
                errors.AppendLine("Введите дату рождения!");
            if (string.IsNullOrWhiteSpace(currentSeller.Work_date.ToString()))
                errors.AppendLine("Введите дату начала работы!");

            if (ComboGender.SelectedIndex == 0)
            {
                currentSeller.Gender = "М";
            }
            else if (ComboGender.SelectedIndex == 1)
            {
                currentSeller.Gender = "Ж";
            }
            else
            { errors.AppendLine("Введите пол!"); }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            
           


            
            if (currentSeller.Seller_ID == 0)
            {

                Trade_organizationEntities2.Context().SELLER.Add(currentSeller);
            }
            try
            {
                Trade_organizationEntities2.Context().SaveChanges();
                MessageBox.Show("Данные сохранены!");
                Manager.MainFrame.GoBack();

            }
            catch
            {

                MessageBox.Show("Не удалось сохранить данные!");
                Manager.MainFrame.GoBack();
            }
        }
        
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentSeller = (sender as Button).DataContext as SELLER; 
            // Проверяем стаж работы
            if (CalculateWorkExperience(currentSeller.Work_date) < 3) // Проверяем, меньше ли стаж 3 месяцев
            {
                MessageBox.Show("Невозможно удалить продавца. Его стаж работы меньше 3 месяцев.");
                return; // Выходим из метода, если стаж меньше 3 месяцев
            }
            if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    // Удаляем продавца из контекста
                    Trade_organizationEntities2.Context().SELLER.Remove(currentSeller);

                    // Сохраняем изменения в базе данных
                    Trade_organizationEntities2.Context().SaveChanges();

                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        // Функция для расчета стажа в месяцах
        private int CalculateWorkExperience(DateTime workDate)
        {
            DateTime today = DateTime.Now;
            int months = (today.Year - workDate.Year) * 12 + (today.Month - workDate.Month);
            return months;
        }
    }
}

