using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
using UchebPr;
using System.Diagnostics;


namespace UchebPr
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Entities entities = new Entities();        
        private bool isClosing = false;
        private Users currentUser;
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new KancTovPage());
            Navigate.MainFrame = MainFrame;
            Closing += MainWindow_Closing;


            if (!GlobalVariables.admin)
            {
                BtnChanges.IsEnabled = false;
                BtnChanges.Visibility = Visibility.Hidden;
                BtnLogIn.IsEnabled = false;
                BtnLogIn.Visibility = Visibility.Hidden;
            }
            currentUser = entities.Users.FirstOrDefault(x => x.Id == GlobalVariables.globalIdUser);            
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var User = entities.Users.FirstOrDefault(x => x.Id == GlobalVariables.globalIdUser);

            DateTime logout = DateTime.Now;
            DateTime login = GlobalVariables.timing;

            TimeSpan timeSpent = logout - login;

            int hours = timeSpent.Hours;
            int minutes = timeSpent.Minutes;
            int seconds = timeSpent.Seconds;

            User.Hlbo = $"{hours} hours, {minutes} minutes, {seconds} secondss";
            entities.SaveChanges();
        }

        private void BtnKT_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new KancTovPage());
        }

        private void BtnDep_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageDep());
        }

        private void BtnAddKT_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageAddKT(null));
        }

        private void BtnAppl_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageZayav()); 
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageProfile());
        }

        private void BtnChanges_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageChanges());
        }

        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageLogInUser());
        }

        private void Btn123_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PagePrice123()); 
        }
    }

    public class GlobalVariables
    {
        public static int globalIdUser;

        public static bool admin = false;

        public static string KT;

        public static DateTime timing;
    }
    
}

//public static Entities _context;

//public static Entities GetContext()
//{
//    if (_context == null)
//    {
//        _context = new Entities();
//    }
//    return _context;
//}
