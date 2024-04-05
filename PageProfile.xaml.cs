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
    /// Логика взаимодействия для PageProfile.xaml
    /// </summary>
    public partial class PageProfile : Page
    {
        Entities entities = new Entities();
        public PageProfile()
        {
            InitializeComponent();
            TBName.PreviewTextInput += TextBox_PreviewTextInput;
            TBSurname.PreviewTextInput += TextBox_PreviewTextInput;
            var Userr = entities.Users.FirstOrDefault(x => x.Id == GlobalVariables.globalIdUser);            
            if (Userr.name != null)
            {
                TBName.Text = Userr.name;
            }
            if (Userr.surname != null)
            {
                TBSurname.Text = Userr.surname;
            }
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^а-яА-Я]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var User = entities.Users.FirstOrDefault(x => x.Id == GlobalVariables.globalIdUser);

            if (User != null)
            {
                if (!string.IsNullOrEmpty(TBSurname.Text) && !string.IsNullOrEmpty(TBName.Text))
                {
                    User.surname = TBSurname.Text;
                    User.name = TBName.Text;
                    entities.SaveChanges();
                    MessageBox.Show("Внесённые данные сохранены!", "Сохранение", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                }

                
            }

            
        }
    }
}
