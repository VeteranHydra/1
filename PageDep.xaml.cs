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
using System.Xml.Linq;

namespace UchebPr
{
    /// <summary>
    /// Логика взаимодействия для PageDep.xaml
    /// </summary>
    public partial class PageDep : Page
    {
        Entities entities = new Entities();
        public PageDep()
        {
            InitializeComponent();
            TBNameDep.PreviewTextInput += TBNameDep_PreviewTextInput;

            foreach (var x in entities.Department)
            {
                LBDep.Items.Add(x);
            }
        }        
        private void TBNameDep_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^А-Яа-я0-9]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
        private void BtnAddDep_Click(object sender, RoutedEventArgs e)
        {
            string bfrName;
            string ftrName;
            if (string.IsNullOrEmpty(TBNameDep.Text))
            {
                MessageBox.Show("Заполните поле \"Наименование\"", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var existingDepartment = entities.Department.FirstOrDefault(dep 
                    => dep.name == TBNameDep.Text);

                if (existingDepartment != null)
                {
                    MessageBox.Show("Отдел с таким названием уже существует", 
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var selectedDepartment = LBDep.SelectedItem as Department;
                    

                    if (selectedDepartment != null)
                    {
                        bfrName = selectedDepartment.name;                        
                        selectedDepartment.name = TBNameDep.Text;
                        ftrName = TBNameDep.Text;                        
                        LBDep.Items.Refresh();                        
                        MessageBox.Show("Информация об отделе успешно изменена!",
                            "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                        var User = entities.Users.FirstOrDefault(x => 
                        x.Id == GlobalVariables.globalIdUser);
                        Changes change = new Changes
                        {
                            Changer_name = User.name,
                            Changer_surname = User.surname,
                            Change = $"Изменено(Отделы): {bfrName} => {ftrName}",
                            Change_date = DateTime.Now.ToString(),
                        };
                        entities.Changes.Add(change);
                        entities.SaveChanges();
                    }
                    else
                    {
                        string NewName;
                        var newDepartment = new Department
                        {
                            name = TBNameDep.Text,                             
                        };
                        NewName = TBNameDep.Text;
                        entities.Department.Add(newDepartment);
                        entities.SaveChanges();
                        LBDep.Items.Add(newDepartment);
                        MessageBox.Show("Отдел успешно добавлен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                        
                        
                        var User = entities.Users.FirstOrDefault(x => x.Id == GlobalVariables.globalIdUser);
                        Changes change = new Changes
                        {
                            Changer_name = User.name,
                            Changer_surname = User.surname,
                            Change = $"Добавлено(Отделы): {NewName}",
                            Change_date = DateTime.Now.ToString(),
                        };
                        entities.Changes.Add(change);
                        entities.SaveChanges();
                    }
                }
            }
        }

        private void BtnDelDep_Click_1(object sender, RoutedEventArgs e)
        {
            var del_dep = LBDep.SelectedItem as Department;

            try
            {
                if (del_dep == null)
                {
                    MessageBox.Show("Выберите отдел для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var exists = entities.KancTovari.Any(kt => kt.id_dep == del_dep.id_dep);

                if (exists)
                {
                    MessageBox.Show("Запись удалить нельзя! \nСуществуют связи!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    LBDep.SelectedItem = null;
                }
                else
                {
                    var result = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        var User = entities.Users.FirstOrDefault(x => x.Id == GlobalVariables.globalIdUser);
                        string DelName = del_dep.name;
                        Changes change = new Changes
                        {
                            Changer_name = User.name,
                            Changer_surname = User.surname,
                            Change = $"Удалено(Отделы): {DelName}",
                            Change_date = DateTime.Now.ToString(),
                        };
                        entities.Changes.Add(change);                        
                        MessageBox.Show("Отдел успешно удалён!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Question);
                        entities.Department.Remove(del_dep);
                        entities.SaveChanges();
                        LBDep.Items.Remove(del_dep);
                        LBDep.SelectedItem = null;
                        LBDep.Items.Refresh();                       
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Произошла ошибка при удалении записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LBDep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_dep = LBDep.SelectedItem as Department;
            if(selected_dep != null)
            {
                TBNameDep.Text = selected_dep.name;
            }
            
            
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            LBDep.SelectedIndex = -1;
            TBNameDep.Clear();
        }
    }
}
