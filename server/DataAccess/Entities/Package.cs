using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

public partial class Package
{
    [Key]
    public Guid PackageId { get; set; }

    public int NumberOfFields { get; set; }

    [Precision(10, 2)]
    public decimal Price { get; set; }

    [InverseProperty("Package")]
    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();
}
