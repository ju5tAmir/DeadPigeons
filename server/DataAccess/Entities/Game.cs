using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

public partial class Game
{
    [Key]
    public Guid GameId { get; set; }

    public int WeekNumber { get; set; }

    public DateTime ValidFromDate { get; set; }

    public DateTime ValidUntilDate { get; set; }

    [StringLength(32)]
    public string? Status { get; set; }

    public List<int>? WinningSequence { get; set; }

    public DateTime? FinishedAt { get; set; }

    [InverseProperty("Game")]
    public virtual ICollection<Board> Boards { get; set; } = new List<Board>();

    [InverseProperty("Game")]
    public virtual ICollection<Winner> Winners { get; set; } = new List<Winner>();
}
