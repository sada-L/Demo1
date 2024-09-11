using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Demo.Context;
using Demo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Demo;

public partial class HistoryWindow : Window
{
    private List<Visit> visits;

    public HistoryWindow()
    {
        InitializeComponent();
    }
    public HistoryWindow(Client client)
    {
        InitializeComponent();

        visits = Helper.Database.Visits.Include(x => x.Documents).Where(x => x.Clientid == client.Id).ToList();

        ListBox.ItemsSource = visits;
    }

    private void Button_Click_Open(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var id = (int)(sender as Button)!.Tag!;
        var visit = visits.Find(x => x.Id == id);

        DocumentWindow documentWindow = new DocumentWindow(visit!);
        documentWindow.ShowDialog(this);
    }
}