using Microsoft.Win32;
using Personal_Ivanov;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            bool isNewRecord = currentSeller.Seller_ID == 0;

            // Заполнение данных currentSeller (Проверки на null уже присутствуют в валидации)
            currentSeller.Position = ComboPosition.SelectedIndex + 1;
            currentSeller.Trade_point = PointCBox.SelectedIndex + 1;
            currentSeller.Work_date = (DateTime)DateDatapicker.SelectedDate;
            currentSeller.Year_birthday = (DateTime)BirthdayDateDatapicker.SelectedDate;
            currentSeller.Surename = SurenameTBox.Text.Trim();
            currentSeller.Name = NameTBox.Text.Trim();
            currentSeller.Patronumic = PatronumicTBox.Text.Trim();
            currentSeller.City = CityTextBox.Text.Trim();
            currentSeller.Address = AddressTextBox.Text.Trim();
            currentSeller.Pasport_data = PasportTBox.Text.Trim();
            currentSeller.Phone_number = PhoneTBox.Text.Trim();
           

            // Валидация
            if (string.IsNullOrWhiteSpace(currentSeller.Surename)) errors.AppendLine("Введите фамилию!");
            if (string.IsNullOrWhiteSpace(currentSeller.Name)) errors.AppendLine("Введите имя!");
            if (string.IsNullOrWhiteSpace(currentSeller.Patronumic)) errors.AppendLine("Введите отчество!");
            if (string.IsNullOrWhiteSpace(currentSeller.City)) errors.AppendLine("Введите город!");
            if (string.IsNullOrWhiteSpace(currentSeller.Address)) errors.AppendLine("Введите адрес!");
            if (string.IsNullOrWhiteSpace(currentSeller.Pasport_data)) errors.AppendLine("Введите паспортные данные!");
            if (string.IsNullOrWhiteSpace(currentSeller.Phone_number)) errors.AppendLine("Введите номер телефона!");
            if (BirthdayDateDatapicker.SelectedDate == null) errors.AppendLine("Введите дату рождения!");
            if (DateDatapicker.SelectedDate == null) errors.AppendLine("Введите дату начала работы!");
            if (ComboGender.SelectedIndex == -1) errors.AppendLine("Введите пол!");
            else currentSeller.Gender = ComboGender.SelectedIndex == 0 ? "М" : "Ж"; // Более компактная запись


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                if (!isNewRecord)
                {
                    // Обновление существующей записи
                    Trade_organizationEntities2.Context().Entry(currentSeller).State = EntityState.Modified;
                    Trade_organizationEntities2.Context().SaveChanges();
                }
                else
                {
                    // Добавление новой записи с инкрементным ID
                    Trade_organizationEntities2.Context().SELLER.Add(currentSeller);
                    Trade_organizationEntities2.Context().SaveChanges();
                }

                MessageBox.Show("Данные сохранены!");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить данные: {ex.Message}");
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

