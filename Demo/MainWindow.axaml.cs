using Avalonia.Controls;
using Demo.Context;
using Demo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo
{
    public partial class MainWindow : Window
    {
        private int ItemsPerPage = 10;

        private List<Client> AllClient = Helper.Database.Clients.Include(x => x.Tags).Include(x => x.Visits).ToList();
        private List<Client> DisplayedClient = new List<Client>();
        private int CurrentPage;
        private int TotalPages => (AllClient.Count + ItemsPerPage - 1) / ItemsPerPage;


        public MainWindow()
        {
            InitializeComponent();

            CurrentPage = 1;
            InitList();
        }

        private void InitList()
        {
            if (FilterBox == null) return;

            var list = AllClient.ToList();

            if (CheckBoxSort.IsChecked ?? false) 
            {
                list = list.Where(x => x.Birthday!.Value.Month == DateTime.Now.Month).ToList();
            }


            list = FilterBox.SelectedIndex switch
            {
                1 => list.Where(x => x.Gendercode == 'æ').ToList(),
                2 => list.Where(x => x.Gendercode == 'ì').ToList(),
                _ => list
            };

            if (!string.IsNullOrEmpty(SearchBox.Text))
            {
                list = list.Where(x =>
                    x.Lastname.ToLower().Contains(SearchBox.Text.ToLower()) ||
                    x.Firstname.ToLower().Contains(SearchBox.Text.ToLower()) ||
                    x.Patronymic!.ToLower().Contains(SearchBox.Text.ToLower()) ||
                    x.Email!.ToLower().Contains(SearchBox.Text.ToLower()) ||
                    x.Phone.ToLower().Contains(SearchBox.Text.ToLower()))
                    .ToList();
            }

            list = SortBox.SelectedIndex switch
            {
                1 => list.OrderBy(x => x.Firstname).ToList(),
                2 => list.OrderBy(x => x.Dateofvisit).ToList(),
                3 => list.OrderByDescending(x => x.Countofvisit).ToList(),
                _ => list
            };

            UpdateDisplayedClient(list);
        }

        private void UpdateDisplayedClient(List<Client> list)
        {
            DisplayedClient.Clear();

            foreach (var product in list.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage))
            {
                DisplayedClient.Add(product);
            }

            ClientListBox.ItemsSource = null;
            ClientListBox.ItemsSource = DisplayedClient;
        }

        private void ComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e) => InitList();

        private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e) => InitList();

        private void Button_Click_Next(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
            }
            InitList();
        }

        private void Button_Click_Back(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
            InitList();
        }

        private void CheckBox_Checked(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => InitList();
    }
}