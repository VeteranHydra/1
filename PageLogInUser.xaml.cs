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

namespace UchebPr
{
    /// <summary>
    /// Логика взаимодействия для PageLogInUser.xaml
    /// </summary>
    public partial class PageLogInUser : Page
    {
        public PageLogInUser()
        {
            InitializeComponent();
            dataGridLogIn.ItemsSource = Entities.GetContext().Users.ToList();
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = TBSearch.Text.ToLower();
            var collectionView = CollectionViewSource.GetDefaultView(dataGridLogIn.ItemsSource);
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
                        Users itemData = item as Users;
                        if (itemData != null)
                        {
                            bool found = itemData.name.ToLower().Contains(searchText) ||
                                         itemData.surname.ToLower().Contains(searchText) ||
                                         itemData.lastLogInDate.ToString().ToLower().Contains(searchText);
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
