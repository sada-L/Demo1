using Avalonia.Controls;
using Demo.Models;
using System;

namespace Demo;

public partial class ClientForm : Window
{
    public ClientForm()
    {
        InitializeComponent();

        Client client = new Client();

        MainGrid.DataContext = client;

        IdBlock.Text = string.Empty;
    }

    public ClientForm(Client client)
    {
        InitializeComponent();

        MainGrid.DataContext = client;

        GenderToggle.IsChecked = client.Gendercode == 'æ';

        DateCalendar.SelectedDate = DateTime.Parse(client.Birthday.ToString()!);
    }
}