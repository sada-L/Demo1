using Avalonia.Controls;
using Demo.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitList();
        }

        private void InitList()
        {
            if (FilterBox == null) return;

            var list = Helper.Database.Clients
                .Include(x => x.Tags)
                .Include(x => x.Visits)
                .ToList();

            list = FilterBox.SelectedIndex switch
            {
                1 => list.Where(x => x.Gendercode == 'æ').ToList(),
                2 => list.Where(x => x.Gendercode == 'ì').ToList(),
                _ => list
            };

            list = SortBox.SelectedIndex switch
            {
                1 => list.OrderBy(x => x.Firstname).ToList(),
                2 => list.OrderBy(x => x.Dateofvisit).ToList(),
                3 => list.OrderByDescending(x => x.Countofvisit).ToList(),
                _ => list
            };

            ClientListBox.ItemsSource = list;
        }

        private void ComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e) => InitList();
    }
}