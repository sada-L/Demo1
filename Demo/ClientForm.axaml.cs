using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Demo.Context;
using Demo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Demo;

public partial class ClientForm : Window
{
    private Client Client { get; set; }
    public ClientForm()
    {
        InitializeComponent();

        Client = new Client();

        MainGrid.DataContext = Client;

        IdBlock.Text = string.Empty;

        DateCalendar.SelectedDate = DateTime.Now;
    }

    public ClientForm(Client client)
    {
        InitializeComponent();

        Client = client;

        MainGrid.DataContext = Client;

        GenderToggle.IsChecked = Client.Gendercode == 'æ';

        DateCalendar.SelectedDate = DateTime.Parse(Client.Birthday.ToString()!);
    }

    private async void Button_Click_AddPhoto(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
       
        var openFileDialog = new OpenFileDialog
        {
            AllowMultiple = false,
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter { Name = "Image files", Extensions = new List<string> {"jpg"} }
            }
        };
        var result = await openFileDialog.ShowAsync(this);

        if (result?.Length > 0)
        {
            var filePath = result[0];
            var appDir = AppContext.BaseDirectory;
            var fileName = Path.GetFileName(filePath);
            var destinationPath = Path.Combine(appDir, "Assets", fileName);
            Directory.CreateDirectory(Path.Combine(appDir, "Assets"));
            if (Client.Photopath != null)
            {
                try
                {
                    File.Delete(Path.Combine(appDir, "Assets", Client.Photopath));
                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                    throw;
                }
            }
            File.Copy(filePath, destinationPath, true);

            Client.Photopath = fileName;
            ImageClient.Source = new Bitmap(destinationPath);
        }
    }

    private void Button_Click_Save(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (Client.Id == 0)
        {
            Client.Birthday = DateOnly.FromDateTime(DateCalendar.SelectedDate!.Value);
            Client.Registrationdate = DateTime.Now;
            Client.Gendercode = GenderToggle.IsChecked == true ? 'æ' : 'ì';
            Helper.Database.Clients.Add(Client!);
            Helper.Database.SaveChanges();
        }
        else
        {
            Helper.Database.Clients.Update(Client!);
            Helper.Database.SaveChanges();
        }
        Close();
    }
}