using Avalonia.Controls;
using Avalonia.Media;
using Demo.Context;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

            var list = Helper.Database.Clients.Include(x => x.Tags).ToList();

            list = FilterBox.SelectedIndex switch
            {
                1 => list.Where(x => x.Gendercode == 'æ').ToList(),
                2 => list.Where(x => x.Gendercode == 'ì').ToList(),
                _ => list
            };

            list = SortBox.SelectedIndex switch
            {
                1 => list.OrderBy(x => x.Lastname).ToList(),
                2 => list.OrderBy(x => x.Dataofvisit).ToList(),
                3 => list.OrderBy(x => x.Countofvisit).ToList(),
                _ => list
            };

            ClientListBox.ItemsSource = list;
        }

        private void ComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e) => InitList();
    }
}