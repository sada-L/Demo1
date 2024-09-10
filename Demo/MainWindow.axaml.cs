using Avalonia.Controls;
using Demo.Context;
using Demo.Models;
using HarfBuzzSharp;
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
                1 => list.Where(x => x.Gendercode == '�').ToList(),
                2 => list.Where(x => x.Gendercode == '�').ToList(),
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

            ItemsPerPage = PageComboBox.SelectedIndex switch
            {
                0 => 10,
                1 => 50,
                2 => 200,
                4 => list.Count,
                _ => 10
            };

            foreach (var product in list.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage))
            {
                DisplayedClient.Add(product);
            }

            CurrentCountTextBlock.Text = list.Count.ToString();
            AllCountTextBlock.Text = AllClient.Count.ToString();
            ClientListBox.ItemsSource = null;
            ClientListBox.ItemsSource = DisplayedClient;
        }

        private void ComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e) => InitList();

        private void TextBox_TextChanged(object? sender, TextChangedEventArgs e) => InitList();

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

        private void Button_Click_Delete(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            int id = (int)(sender as Button)?.Tag!;

            var client = Helper.Database.Clients.Find(id);

            if (client != null)
            {
                if (client.Visits.Count <= 0)
                {
                    List<Models.Tag> tags = client.Tags.ToList();

                    for (int i = 0; i < tags.Count; i++)
                    {
                        Helper.Database.Tags.Remove(tags[i]);
                        Helper.Database.SaveChanges();
                    }

                    Helper.Database.Clients.Remove(client);
                    Helper.Database.SaveChanges();
                }
                else
                {
                    ErrorTextBlock.Text = "������ ������� ������� � �����������";
                }
            }
            else
            {
                ErrorTextBlock.Text = "Error";
            }
            AllClient = Helper.Database.Clients.Include(x => x.Tags).Include(x => x.Visits).ToList();
            InitList();
        }

        private void Button_Click_Add(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ClientForm clientForm = new ClientForm();
            clientForm.ShowDialog(this);

            AllClient = Helper.Database.Clients.Include(x => x.Tags).Include(x => x.Visits).ToList();
            InitList();
        }

        private void Button_Click_Edit(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var id = (int)(sender as Button)?.Tag!;

            var client = Helper.Database.Clients.Find(id);

            ClientForm clientForm = new ClientForm(client!);
            clientForm.ShowDialog(this);

            AllClient = Helper.Database.Clients.Include(x => x.Tags).Include(x => x.Visits).ToList();
            InitList();
        }
    }
}