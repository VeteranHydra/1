using Microsoft.Win32;
using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UchebPr;

namespace UchebPr
{
    /// <summary>
    /// Логика взаимодействия для PageAddKT.xaml
    /// </summary>
    public partial class PageAddKT : Page
    {
        Entities entities = new Entities();
        private KancTovari KT = new KancTovari();
        private KancTovari KTvrem = new KancTovari();
        BitmapImage bitmap;
        string image;
        public PageAddKT(KancTovari selectKT)
        {
            InitializeComponent();
            if (selectKT != null)
            {
                KT = selectKT;
            }
            DataContext = KT;
            TBPrice.PreviewTextInput += tbPrice_Text;
            TBCount.PreviewTextInput += tbPrice_Text;
            KTvrem.name = KT.name;
            KTvrem.price = KT.price;
            KTvrem.count = KT.count;
            KTvrem.ImageTov = KT.ImageTov;

            CBDep.ItemsSource = Entities.GetContext().Department.ToList();
            
        }
        private void tbPrice_Text(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
        private void BtnAddPicture_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения|*.jpg;*.jpeg;*.png|Все файлы|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImage = openFileDialog.FileName;
                bitmap = new BitmapImage(new Uri(selectedImage));
                ImageKT.Source = bitmap;
                image = openFileDialog.FileName;
                KT.ImageTov = image;
            }

        }
        private void BtnAddKT_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(KT.name))
                errors.AppendLine("Не введено наименование");
            if (string.IsNullOrWhiteSpace(KT.count.ToString()))
                errors.AppendLine("Не введена количество");
            if (string.IsNullOrWhiteSpace(KT.price.ToString()))
                errors.AppendLine("Не введена цена");
            if(CBDep.SelectedItem == null)
                errors.AppendLine("Не выбран отдел");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var User = entities.Users.FirstOrDefault(x 
                => x.Id == GlobalVariables.globalIdUser);

            string name, surname;
            name = User.surname;
            surname = User.name;
            if (User.surname == null && User.name == null)
                name = User.Id.ToString();
            if (KT.id_kt == 0)
            {
                Entities.GetContext().KancTovari.Add(KT);
                Changes new_change = new Changes
                {
                    Changer_name = name,
                    Changer_surname = surname,
                    Change = $"Добавлено: Наименование: {KT.name}," +
                    $" Цена: {KT.price}, Количество: {KT.count}",
                    Change_date = DateTime.Now.ToString(),
                };
                entities.Changes.Add(new_change);
                entities.SaveChanges();
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены!");
                Navigate.MainFrame.Navigate(new KancTovPage());
            }
            else
            {
                try
                {
                    var existKT = entities.KancTovari.FirstOrDefault(x => x.id_kt == KT.id_kt);
                    if (existKT != null)
                    {
                        existKT.name = KT.name;
                        existKT.price = KT.price;
                        existKT.count = KT.count;
                        existKT.ImageTov = KT.ImageTov;
                        existKT.id_dep = KT.id_dep;
                        Changes change = new Changes
                        {
                            Changer_name = name,
                            Changer_surname = surname,
                            Change = $"Изменено: Наименование: {KTvrem.name} " +
                            $"=> {KT.name}, Цена: {KTvrem.price} => {KT.price}, " +
                            $"Количество: {KTvrem.count} => {KT.count}",
                            Change_date = DateTime.Now.ToString(),
                        };
                        entities.Changes.Add(change);
                        entities.SaveChanges();
                        Entities.GetContext().SaveChanges();
                        MessageBox.Show("Изменения успешно сохранены!");
                        Navigate.MainFrame.Navigate(new KancTovPage());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}