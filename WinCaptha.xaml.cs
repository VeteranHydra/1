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
using System.Windows.Shapes;
using System.Timers;

namespace UchebPr
{
    /// <summary>
    /// Логика взаимодействия для WinCaptha.xaml
    /// </summary>
    public partial class WinCaptha : Window
    {
        private string captchaText;
        private Timer timer;

        public WinCaptha()
        {
            InitializeComponent();
            GenerateAndSetCaptchaText();
        }
        private void GenerateAndSetCaptchaText()
        {
            captchaText = GenerateRandomCaptchaText();
            LabelCaptcha.Content = captchaText;
        }

        private void BtnCheckClick(object sender, RoutedEventArgs e)
        {
            string userInput = captchaInputTextBox.Text;
            if (userInput == captchaText)
            {
                LabelMSG2.Content = "";
                Close();
            }
            else
            {                
                LabelMSG2.Content = "Неверный ввод captcha, " +
                    "\nпопробуйте еще раз через 10 секунд!";
                captchaInputTextBox.Text = string.Empty;
                BtnCheck.IsEnabled = false; 
                timer = new Timer();
                timer.Interval = 10000; 
                timer.Elapsed += (s, args) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        BtnCheck.IsEnabled = true; 
                        LabelMSG2.Content = "";
                    });
                    timer.Stop();
                };
                timer.Start();
                GenerateAndSetCaptchaText();                
            }
        }
        private string GenerateRandomCaptchaText()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
