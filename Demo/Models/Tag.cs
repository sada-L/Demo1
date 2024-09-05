using Avalonia.Media;
using Avalonia.Media.Immutable;
using System;
using System.Collections.Generic;

namespace Demo.Models;

public partial class Tag
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Color { get; set; } = null!;

    public SolidColorBrush TagColor => SolidColorBrush.Parse($"#{Color}");

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
