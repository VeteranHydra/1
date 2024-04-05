using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для PageZayav.xaml
    /// </summary>
    public partial class PageZayav : Page
    {
        Entities entities = new Entities();
        public PageZayav()
        {
            InitializeComponent();
            
            TBCountKTnew.PreviewTextInput += TBCountKTnew_PreviewTextInput;

            TBNameKT.IsEnabled = false;
            TBCountKT.IsEnabled = false;
            

            foreach (var x in entities.KancTovari)
            {
                LBZayav.Items.Add(x);
            }
        }
        private void TBCountKTnew_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        private void LBZayav_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_KT = LBZayav.SelectedItem as KancTovari;
            if(selected_KT != null)
            {
                TBNameKTnew.IsEnabled = false;
                TBNameKT.Text = selected_KT.name;
                TBNameKTnew.Text = selected_KT.name;
                TBCountKT.Text = (selected_KT.count).ToString();
            }
            else
            {
                TBNameKTnew.IsEnabled = true;                
            }
        }

        private void BtnZayavAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TBNameKTnew.Text) && string.IsNullOrEmpty(TBCountKTnew.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var selectedKT = LBZayav.SelectedItem as KancTovari;

                if (selectedKT != null)
                {
                    try
                    {
                        
                        int cnt1 = Convert.ToInt32(selectedKT.count);
                        int cnt2 = Convert.ToInt32(TBCountKTnew.Text);
                        selectedKT.count = cnt1 + cnt2;
                        Application application = new Application
                        {
                            id_kt = selectedKT.id_kt,
                            id_dep = selectedKT.id_dep,
                            count_kt = selectedKT.count,
                        };
                        entities.Application.Add(application);
                        entities.SaveChanges();
                        LBZayav.Items.Refresh();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    
                    MessageBox.Show($"Вы успешно заказали товар - {TBNameKTnew.Text} в количестве {TBCountKTnew.Text}", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    
                    var newKT = new KancTovari
                    {
                        name = TBNameKTnew.Text,
                        count = Convert.ToInt32(TBCountKTnew.Text),
                        //price = 0,
                        //id_dep = 0,
                    };
                    entities.KancTovari.Add(newKT);
                    entities.SaveChanges();
                    LBZayav.Items.Add(newKT);
                    MessageBox.Show($"Вы успешно заказали товар - {TBNameKTnew.Text} в количестве {TBCountKTnew.Text}", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TBCountKT.Text = string.Empty;
            TBNameKT.Text = string.Empty;
            TBCountKTnew.Text = string.Empty;
            TBNameKTnew.Text = string.Empty;
            LBZayav.SelectedIndex = -1;
        }
    }
}
