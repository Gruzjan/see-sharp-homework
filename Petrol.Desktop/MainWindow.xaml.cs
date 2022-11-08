using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using Petrol.Shared;

namespace Petrol.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Data data = new();
        public MainWindow()
        {
            InitializeComponent();
        }


        private void CostBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!decimal.TryParse(lpgLiterCostBox.Text, out decimal lpgParsed))
                lpgParsed = 2.29m;
            if (!decimal.TryParse(pbLiterCostBox.Text, out decimal pbParsed))
                pbParsed = 4.99m;
            if (!decimal.TryParse(lpgTankCostBox.Text, out decimal lpgTankParsed))
                lpgTankParsed = 1600m;

            mixedCostBox.Content = data.getExpenses(lpgParsed, pbParsed, lpgTankParsed).Item1;
            pbOnlyCostBox.Content = data.getExpenses(lpgParsed, pbParsed, lpgTankParsed).Item2;
        }

        private void TabItem_Initialized(object sender, EventArgs e)
        {
            lpgLiterCostBox.Text = "2.29";
            pbLiterCostBox.Text = "4.99";
            lpgTankCostBox.Text = "1600";
        }

        private void first_Initialized(object sender, EventArgs e)
        {
            pbRefilCountLabel1.Content = data.pbRefillCount;
            lpgRefilCountLabel.Content = data.lpgRefillCount;
        }

        private void second_Initialized(object sender, EventArgs e)
        {
            lpgOnlyDaysCountLabel.Content = data.lpgOnlyDaysCount;
        }

        private void third_Initialized(object sender, EventArgs e)
        {
            lowLpgDayLabel.Content = data.firstLowOnGasDay.ToShortDateString();
        }

        private void fourth_Initialized(object sender, EventArgs e)
        {
            listBox.Items.Add("Date\t\tBefore\tAfter");
            foreach (var day in data.daysData)
                listBox.Items.Add($"{day.date}\t{day.before}\t{day.after}");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.IO.File.WriteAllText("./data.json", JsonSerializer.Serialize(data.daysData));
            savedLabel.Visibility = Visibility.Visible;
        }
    }
}
