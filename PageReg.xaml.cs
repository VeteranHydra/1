using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
    /// Логика взаимодействия для PageReg.xaml
    /// </summary>
    public partial class PageReg : Page
    {
        Entities entities = new Entities();
        public PageReg()
        {
            InitializeComponent();
            RegTBLogin.PreviewTextInput += RegTBLogin_PreviewTextInput;
        }
        private void RegTBLogin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9@._-]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        private void RegBtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            string Email = RegTBLogin.Text;

            if (string.IsNullOrEmpty(RegTBLogin.Text) 
                || string.IsNullOrEmpty(RegTBPas.Text) 
                || string.IsNullOrEmpty(RegTBPas2.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", 
                    "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (RegTBPas.Text != RegTBPas2.Text)
            {
                MessageBox.Show("Пароли не совпадают. Пожалуйста, " +
                    "проверьте правильность данных и повторите попытку.", 
                    "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var existingUser = entities.Users.FirstOrDefault(u => u.email == Email);
            if (existingUser != null)
            {
                MessageBox.Show("Пользователь с таким email уже существует.", 
                    "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var newUser = new Users
                {
                    email = Email,
                    password = HashFunc.EncryptPassword(RegTBPas.Text),                    
                    role = "User"
                };

                entities.Users.Add(newUser);
                try
                {
                    entities.SaveChanges();                    
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Console.WriteLine($"Property: {validationError.PropertyName} " +
                                $"Error: {validationError.ErrorMessage}");
                        }
                    }
                }

                var mainWindow = new MainWindow();
                mainWindow.Show();
                Window currentWindow = Window.GetWindow(this);
                currentWindow.Close();
            }

            string eemail = RegTBLogin.Text;

            var RegUser = entities.Users.FirstOrDefault(u => u.email == eemail);
            GlobalVariables.globalIdUser = RegUser.Id;
            GlobalVariables.timing = DateTime.Now;

            RegUser.lastLogInDate = (DateTime.Now).ToString();
            entities.SaveChanges();
        }
    }
}
