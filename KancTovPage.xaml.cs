using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace UchebPr
{
    /// <summary>
    /// Логика взаимодействия для KancTovPage.xaml
    /// </summary>
    public partial class KancTovPage : Page
    {
        private List<KancTovari> allRecords;
        Entities entities = new Entities();

        public KancTovPage()
        {
            InitializeComponent();
            LoadRecords();
            PriceFilter.SelectionChanged += FilterRecordsByPrice;
        }

        private void LoadRecords()
        {
            using (var newContext = new Entities())
            {               
                allRecords = newContext.KancTovari.ToList();
                LView.ItemsSource = allRecords;
            }
        }
        private void FilterRecordsByPrice(object sender, EventArgs e)
        {
            if (allRecords == null)
                return;

            var selectedPriceRange = ((ComboBoxItem)
                PriceFilter.SelectedItem).Content.ToString();

            if (selectedPriceRange == "Все")
            {
                LView.ItemsSource = allRecords;
            }
            else
            {
                double priceLimit = 0;

                switch (selectedPriceRange)
                {
                    case "до 100 рублей":
                        priceLimit = 100;
                        break;
                    case "до 200 рублей":
                        priceLimit = 200;
                        break;
                    case "до 300 рублей":
                        priceLimit = 300;
                        break;
                    case "до 500 рублей":
                        priceLimit = 500;
                        break;
                    case "до 1000 рублей":
                        priceLimit = 1000;
                        break;
                    default:
                        return;
                }

                var filteredRecords = allRecords.Where(record 
                    => record.price <= priceLimit).ToList();
                LView.ItemsSource = filteredRecords;
                lblCount.Content = $"Найдено: " +
                    $"{filteredRecords.Count} записей.";

            }
        }
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            int IdDel = 0;            
            if (MessageBox.Show($"Вы точно хотите удалить выбранные элементы? {LView.SelectedItems.Count}", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var newContext = new Entities())
                    {
                        foreach (var item in LView.SelectedItems.Cast<KancTovari>().ToList())
                        {
                            var itemToRemove = newContext.KancTovari.Find(item.id_kt);

                            IdDel = Convert.ToInt32(itemToRemove.id_dep);                            

                            if (itemToRemove != null)
                            {                                
                                newContext.KancTovari.Remove(itemToRemove);
                                string depname;

                                var dep = entities.Department.FirstOrDefault(x => x.id_dep == IdDel);
                                var User = entities.Users.FirstOrDefault(x => x.Id == GlobalVariables.globalIdUser);
                                if(dep != null)
                                {
                                    depname = dep.name;
                                }
                                else
                                {
                                    depname = "?";
                                }
                                Changes change = new Changes
                                {
                                    Changer_name = User.name,
                                    Changer_surname = User.surname,
                                    Change = $"Удалено(Канцтовары): Наименование: {itemToRemove.name}, Отдел: {depname}, Цена: {itemToRemove.price}, Количество: {itemToRemove.count}",
                                    Change_date = DateTime.Now.ToString(),
                                };
                                newContext.Changes.Add(change);
                            }
                        }
                        
                        newContext.SaveChanges();

                        MessageBox.Show("Данные удалены!");
                        LoadRecords();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedData = (sender as Button).DataContext as KancTovari; //Использование контекста данных для
                                                                             //редактирования выбранной записи из ListView

            var editPage = new PageAddKT(selectedData);
            NavigationService.Navigate(editPage);
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = TBSearch.Text.ToLower();

            var collectionView = CollectionViewSource.GetDefaultView(LView.ItemsSource);

            if (collectionView != null)
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    collectionView.Filter = null;
                    lblCount.Content = string.Empty;
                }
                else
                {
                    collectionView.Filter = item =>
                    {
                        KancTovari itemData = item as KancTovari;

                        if (itemData != null)
                        {
                            bool isMatch =
                                itemData.name.ToLower().Contains(searchText) ||
                                itemData.ImageTov.ToLower().Contains(searchText) ||
                                itemData.price.ToString().ToLower().Contains(searchText) ||
                                itemData.count.ToString().ToLower().Contains(searchText);

                            return isMatch;
                        }

                        return false;
                    };

                    int count = 0;
                    foreach (KancTovari item in LView.Items)
                    {
                        if (collectionView.Contains(item))
                        {
                            count++;
                        }
                    }

                    lblCount.Content = $"Найдено: {count} записей по запросу '{searchText}'.";
                }
            }
        }

        
    }
}
