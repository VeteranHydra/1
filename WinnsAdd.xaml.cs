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
using System.Windows.Shapes;

namespace UchebPr
{
    /// <summary>
    /// Логика взаимодействия для WinnsAdd.xaml
    /// </summary>
    public partial class WinnsAdd : Window
    {
        public WinnsAdd()
        {
            InitializeComponent();
            TBName.PreviewTextInput += RegTBLogin_PreviewTextInput;
            TBSname.PreviewTextInput += RegTBLogin_PreviewTextInput;
        }
        private void RegTBLogin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^А-Яа-я]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        private void RegBtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TBName.Text) || string.IsNullOrEmpty(TBSname.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (GlobalVariables.flag)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }
    }
}
