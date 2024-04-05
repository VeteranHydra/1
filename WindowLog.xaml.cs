using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
    /// Логика взаимодействия для WindowLog.xaml
    /// </summary>
    public partial class WindowLog : Window
    {
        public WindowLog()
        {
            InitializeComponent();
            FrameAutLog.Navigate(new PageLog());

        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            FrameAutLog.Navigate(new PageReg());
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            FrameAutLog.Navigate(new PageLog());
        }

        
    }
    public class HashFunc
    {
        public static string EncryptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                string base64Hash = Convert.ToBase64String(hash);

                return base64Hash;
            }
        }
    }
}
