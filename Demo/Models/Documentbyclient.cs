using System;
using System.Collections.Generic;

namespace Demo.Models;

public partial class Documentbyclient
{
    public int Id { get; set; }

    public int Clientid { get; set; }

    public int Documentid { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Document Document { get; set; } = null!;
}
