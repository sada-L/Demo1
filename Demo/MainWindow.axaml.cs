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

            var isChecked = CheckBoxSort.IsChecked ?? false;

            if (isChecked) 
            {
                list = list.Where(x => x.Birthday!.Value.Month == DateTime.Now.Month).ToList();

                if (list.Count == 0)
                {
                    ClientListBox.ItemsSource = null;
                }
            }

            list = SortBox.SelectedIndex switch
            {
                1 => list.OrderBy(x => x.Firstname).ToList(),
                2 => list.OrderBy(x => x.Dateofvisit).ToList(),
                3 => list.OrderByDescending(x => x.Countofvisit).ToList(),
                _ => list
            };

           
            list = FilterBox.SelectedIndex switch
            {
                1 => list.Where(x => x.Gendercode == 'м').ToList(),
                2 => list.Where(x => x.Gendercode == 'ж').ToList(),
                _ => list
            };

            if(!string.IsNullOrEmpty(SearchBox.Text))
            {
                string[] searchTerms = SearchBox.Text.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                list = list.Where(client =>
                {
                    string[] clientFields = 
                    [
                        client.Firstname.ToLower(),
                        client.Lastname.ToLower(),
                        client.Patronymic!.ToLower(),
                        client.Email!.ToLower(),
                        client.Phone!.ToLower(),
                    ];
                    return searchTerms.Any(term => clientFields.Any(field => field.Contains(term)));
                })
                .OrderByDescending(client =>
                {
                    string[] clientFields = 
                    [
                        client.Firstname.ToLower(),
                        client.Lastname.ToLower(),
                        client.Patronymic!.ToLower(),
                        client.Email!.ToLower(),
                        client.Phone!.ToLower(),
                    ];
                    return searchTerms.Any(term => clientFields.Any(field => field.Contains(term)));
                }).ToList();
            }

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

        private void ComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            CurrentPage = 1;

            InitList();
        }

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

        private void Button_Click_Edit(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var id = (int)(sender as Button)?.Tag!;

            var client = Helper.Database.Clients.Find(id);

            ClientForm clientForm = new ClientForm(client!);
            clientForm.Show();
            Close();
        }

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
                    ErrorTextBlock.Text = "Нельзя удалить клиетов с посещениями";
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
            clientForm.Show();
            Close();
        }

        private void Button_Click_History(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(ClientListBox.SelectedItem != null)
            {
                HistoryWindow historyWindow = new HistoryWindow((ClientListBox.SelectedItem as Client)!);
                historyWindow.ShowDialog(this);
            }
        }
    }
}