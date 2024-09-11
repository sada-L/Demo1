using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Demo.Context;
using Demo.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Demo;

public partial class DocumentWindow : Window
{

    public DocumentWindow()
    {
        InitializeComponent();
    }
    public DocumentWindow(Visit visit)
    {
        InitializeComponent();

        ListBox.ItemsSource = visit.Documents;
    }
}