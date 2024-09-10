using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Demo.Models;

namespace Demo;

public partial class HistoryWindow : Window
{
    private Client Client { get; set; }

    public HistoryWindow()
    {
        InitializeComponent();

        ListBox.DataContext = Client;
    }
    public HistoryWindow(Client client)
    {
        InitializeComponent();

        Client = client;

        Border.DataContext = Client;
    }
}