using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    /// Логика взаимодействия для PageChanges.xaml
    /// </summary>
    public partial class PageChanges : Page
    {
        Entities entities = new Entities();

        public PageChanges()
        {
            InitializeComponent();

            dataGridChanges.ItemsSource = Entities.GetContext().Changes.ToList();
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = TBSearch.Text.ToLower();
            var collectionView = CollectionViewSource.GetDefaultView(dataGridChanges.ItemsSource);
            int count = 0;
            if (collectionView != null)
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    collectionView.Filter = null;
                }
                else
                {
                    collectionView.Filter = item =>
                    {
                        Changes itemData = item as Changes;
                        if (itemData != null)
                        {
                            bool found = itemData.Changer_name.ToLower().Contains(searchText) ||
                                         itemData.Changer_surname.ToLower().Contains(searchText) ||
                                         itemData.Change_date.ToString().ToLower().Contains(searchText) ||
                                         itemData.Change.ToString().ToLower().Contains(searchText);

                            if (found)
                            {
                                count++;
                            }
                            return found;
                        }
                        return false;
                    };
                }
                lblCount.Content = $"Найдено записей: {count}";
            }
        }
    }
}
