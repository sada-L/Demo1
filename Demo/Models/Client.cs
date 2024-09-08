using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Models;

public partial class Client
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public DateOnly? Birthday { get; set; }

    public DateTime Registrationdate { get; set; }

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;

    public char Gendercode { get; set; }

    public string? Photopath { get; set; }

    public Bitmap? ClientImage => !string.IsNullOrEmpty(Photopath) ? new Bitmap(AppDomain.CurrentDomain.BaseDirectory + $"Assets/" + Photopath) : null;

    public DateOnly? Dateofvisit => Visits.Count != 0 ? Visits.Select(x => x.Starttime).Order().First() : null!;

    public int? Countofvisit => Visits.Count;

    public virtual ICollection<Documentbyclient> Documentbyclients { get; set; } = new List<Documentbyclient>();

    public virtual Gender GendercodeNavigation { get; set; } = null!;

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
