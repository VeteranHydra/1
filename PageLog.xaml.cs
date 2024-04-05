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
using static System.Net.Mime.MediaTypeNames;

namespace UchebPr
{
    /// <summary>
    /// Логика взаимодействия для PageLog.xaml
    /// </summary>
    
    public partial class PageLog : Page
    {
        Entities entities = new Entities();
        public PageLog()
        {
            InitializeComponent();
        }
        private void LogBtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LogTBLogin.Text) || string.IsNullOrEmpty(LogTBPassword.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Авторизация", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool pass = false;

            foreach (var user in entities.Users)
            {
                if (LogTBLogin.Text == user.email || LogTBLogin.Text == user.Id.ToString())
                {
                    if (HashFunc.EncryptPassword(LogTBPassword.Text) == user.password)
                    {                        
                        if (user.role == "Admin")
                        {
                            GlobalVariables.admin = true;
                        }                        
                        var mainwin = new MainWindow();
                        mainwin.Show();
                        Window currentWindow = Window.GetWindow(this);
                        currentWindow.Close();
                        pass = true;
                        break;
                    }
                }
            }

            if (!pass)
            {
                MessageBox.Show("Неверный Логин или Пароль. Пожалуйста проверьте правильность введённых данных и повторите попытку.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                var wincptch = new WinCaptha();
                wincptch.ShowDialog();
            }

            string datauser = LogTBLogin.Text;

            if (int.TryParse(datauser, out int id))
            {
                try
                {
                    GlobalVariables.globalIdUser = id;
                    var User = entities.Users.FirstOrDefault(x => x.Id == GlobalVariables.globalIdUser);
                    User.lastLogInDate = (DateTime.Now).ToString();
                    entities.SaveChanges();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                
            }
            else
            {
                var User = entities.Users.FirstOrDefault(u => u.email == datauser);
                GlobalVariables.globalIdUser = User.Id;
                User.lastLogInDate = (DateTime.Now).ToString();
                GlobalVariables.timing = DateTime.Now;
                entities.SaveChanges();
            }
            
            GlobalVariables.admin = false;

            
        }
        private void BtnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пожалуйста, сообщите о ситуации администратору. Он обязательно Вам поможет!", "Ничего страшного!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
