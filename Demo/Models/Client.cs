using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Demo.Models;

public partial class Client
{
    public int Id { get; set; }

    [RegularExpression(@"^[а-яА-Яa-zA-Z\s-]{1,50}$", ErrorMessage = "Фамилия может содержать только буковы.")]
    public string Firstname { get; set; } = null!;

    [RegularExpression(@"^[а-яА-Яa-zA-Z\s-]{1,50}$", ErrorMessage = "Имя может содержать только буковы.")]
    public string Lastname { get; set; } = null!;

    [RegularExpression(@"^[а-яА-Яa-zA-Z\s-]{1,50}$", ErrorMessage = "Отчество может содержать только буковы.")]
    public string? Patronymic { get; set; }

    public DateOnly? Birthday { get; set; }

    public DateTime Registrationdate { get; set; }

    [RegularExpression(@"^[\w\.]+@[\w\.]+$", ErrorMessage = "Неверный формат почты")]
    public string? Email { get; set; }

    [Phone] public string Phone { get; set; } = null!;

    public char Gendercode { get; set; }

    public string? Photopath { get; set; }

    public Bitmap? ClientImage => File.Exists(AppDomain.CurrentDomain.BaseDirectory + $"Assets/" + Photopath)
        ? new Bitmap(AppDomain.CurrentDomain.BaseDirectory + $"Assets/" + Photopath)
        : null;

    public DateOnly? Dateofvisit => Visits.Count != 0 ? Visits.Select(x => x.Starttime).Order().First() : null!;

    public int? Countofvisit => Visits.Count;

    public virtual Gender GendercodeNavigation { get; set; } = null!;

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
