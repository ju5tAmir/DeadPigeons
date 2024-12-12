using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

public partial class Board
{
    [Key]
    public Guid BoardId { get; set; }

    public Guid GameId { get; set; }

    public Guid PackageId { get; set; }

    public string PlayerId { get; set; } = null!;

    public List<int>? PlaySequence { get; set; }

    public DateTime? PlayDate { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("Boards")]
    public virtual Game Game { get; set; } = null!;

    [ForeignKey("PackageId")]
    [InverseProperty("Boards")]
    public virtual Package Package { get; set; } = null!;

    [ForeignKey("PlayerId")]
    [InverseProperty("Boards")]
    public virtual AspNetUser Player { get; set; } = null!;

    [InverseProperty("Board")]
    public virtual ICollection<Winner> Winners { get; set; } = new List<Winner>();
}
