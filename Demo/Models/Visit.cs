using System;
using System.Collections.Generic;

namespace Demo.Models;

public partial class Visit
{
    public int Id { get; set; }

    public int Clientid { get; set; }

    public DateOnly Starttime { get; set; }

    public virtual Client Client { get; set; } = null!;
}
