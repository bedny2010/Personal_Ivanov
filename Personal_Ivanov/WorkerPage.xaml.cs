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

namespace Personal_Ivanov
{
    /// <summary>
    /// Логика взаимодействия для WorkerPage.xaml
    /// </summary>
    public partial class WorkerPage : Page
    {
        public WorkerPage()
        {

            InitializeComponent();
            List<SELLER> currentWorkers = Trade_organizationEntities2.Context().SELLER.ToList();

            FiltrBox.SelectedIndex = 0;
            SortBox.SelectedIndex = 0;

            WorkersListView.ItemsSource = currentWorkers;



            Update();
        }
        public void Update()
        {
            var currentWorkers = Trade_organizationEntities2.Context().SELLER.ToList();
            if (FiltrBox.SelectedIndex == 0)
            {
                currentWorkers = currentWorkers.Where(s => s.SellerSalary >= 0 && s.SellerSalary < 100000).ToList();
            }

            if (FiltrBox.SelectedIndex == 1)
            {
                currentWorkers = currentWorkers.Where(s => s.SellerSalary >= 0 && s.SellerSalary < 40000).ToList();
            }

            if (FiltrBox.SelectedIndex == 2)
            {
                currentWorkers = currentWorkers.Where(s => s.SellerSalary >= 40000 && s.SellerSalary < 60000).ToList();
            }

            if (FiltrBox.SelectedIndex == 3)
            {
                currentWorkers = currentWorkers.Where(s => s.SellerSalary >= 60000 && s.SellerSalary < 100000).ToList();
            }

            if (SortBox.SelectedIndex == 0) { }
            if (SortBox.SelectedIndex == 1)
            {
                currentWorkers = currentWorkers.OrderBy(s => s.SellerSalary).ToList();
            }
            if (SortBox.SelectedIndex == 2)
            {
                currentWorkers = currentWorkers.OrderByDescending(s => s.SellerSalary).ToList();
            }
            string searchText = TBoxSearch.Text.ToLower();
            currentWorkers = currentWorkers.Where(s =>
                s.Name.ToLower().Contains(searchText) ||
                s.Surename.ToLower().Contains(searchText) ||
                s.Patronumic.ToLower().Contains(searchText) ||
                s.Trade_point_name1.ToLower().Contains(searchText)||
                s.SellerPosition.ToLower().Contains(searchText)
            ).ToList();
            WorkersListView.ItemsSource = currentWorkers;

           

            AllQuantity.Text = Trade_organizationEntities2.Context().SELLER.ToList().Count().ToString();
            CurrentQuantity.Text = currentWorkers.Count.ToString();
            

        }
        
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void FiltrBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void edit_button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as SELLER));
        }

        private void WorkersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void Add_button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }
    }
}

