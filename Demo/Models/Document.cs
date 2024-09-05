using System;
using System.Collections.Generic;

namespace Demo.Models;

public partial class Document
{
    public int Id { get; set; }

    public string Documentpath { get; set; } = null!;

    public virtual ICollection<Documentbyclient> Documentbyclients { get; set; } = new List<Documentbyclient>();
}
