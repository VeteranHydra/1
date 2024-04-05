using System;
using System.Collections.Generic;
using System.IO;
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
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace UchebPr
{
    /// <summary>
    /// Логика взаимодействия для PagePrice123.xaml
    /// </summary>
    public partial class PagePrice123 : Page
    {
        public PagePrice123()
        {            
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            long totalQuantity = 0;
            long totalPrice = 0;
            using (var entities = new Entities())
            {
                foreach (var kt in entities.KancTovari)
                {
                    long quantity = Convert.ToInt64(kt.count);
                    long price = Convert.ToInt64(kt.price);
                    long totalPerItem = quantity * price;
                    totalPrice += totalPerItem;
                    totalQuantity += quantity;
                }
            }
            countKT.Content = totalQuantity.ToString();
            priceKT.Content = totalPrice.ToString();
        }
        private void BtnLoadData_Click(object sender, RoutedEventArgs e)
        {
            string reportFolderPath = "C:\\Report\\";
            Directory.CreateDirectory(reportFolderPath);

            string fileName = $"ProductsReport_{DateTime.Now:yyyyMMddHHmmss}.docx";
            string filePath = System.IO.Path.Combine(reportFolderPath, fileName);

            using (DocX document = DocX.Create(filePath))
            {
                document.InsertParagraph("Отчет о продукции").FontSize(14).Font(new Font("TimesNewRoman")).Bold().Alignment = Alignment.center;

                Xceed.Document.NET.Table table = document.AddTable(1, 4);
                table.Alignment = Alignment.center;

                table.Rows[0].Cells[0].Paragraphs.First().Append("ID товара");
                table.Rows[0].Cells[1].Paragraphs.First().Append("Наименование");
                table.Rows[0].Cells[2].Paragraphs.First().Append("Цена");
                table.Rows[0].Cells[3].Paragraphs.First().Append("Количество");

                using (Entities entities = new Entities())
                {
                    var products = entities.KancTovari.ToList();

                    foreach (var product in products)
                    {
                        Row row = table.InsertRow();

                        row.Cells[0].Paragraphs[0].Append(product.id_kt.ToString());
                        row.Cells[1].Paragraphs[0].Append(product.name);
                        row.Cells[2].Paragraphs[0].Append(product.price.ToString());
                        row.Cells[3].Paragraphs[0].Append(product.count.ToString());
                    }

                    document.InsertTable(table);

                    document.InsertParagraph("").SpacingAfter(20);

                    document.InsertParagraph("Итого").FontSize(14).Font(new Font("TimesNewRoman")).Bold().Alignment = Alignment.center;

                    Xceed.Document.NET.Table secondTable = document.AddTable(1, 2);
                    secondTable.Alignment = Alignment.center;
                    secondTable.Design = TableDesign.TableGrid;

                    secondTable.Rows[0].Cells[0].Paragraphs.First().Append("Общее количество товаров");
                    secondTable.Rows[0].Cells[1].Paragraphs.First().Append("Суммарная цена (руб.)");

                    int totalQuantity = products.Sum(p => p.count ?? 0);
                    decimal totalSum = products.Sum(p => (decimal)(p.price ?? 0) * (p.count ?? 0));

                    Row totalRow = secondTable.InsertRow();
                    totalRow.Cells[0].Paragraphs[0].Append(totalQuantity.ToString());
                    totalRow.Cells[1].Paragraphs[0].Append(totalSum.ToString("0.00"));

                    document.InsertTable(secondTable);
                }

                try
                {
                    document.Save();
                    MessageBox.Show("Файл успешно сохранен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при сохранении файла: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                System.Diagnostics.Process.Start("explorer.exe", "/select," + filePath);
            }
        }
    }
}
