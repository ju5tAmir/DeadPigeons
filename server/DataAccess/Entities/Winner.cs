using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

public partial class Winner
{
    [Key]
    public Guid WinningRecordId { get; set; }

    public Guid GameId { get; set; }

    public Guid BoardId { get; set; }

    public string PlayerId { get; set; } = null!;

    [Precision(10, 2)]
    public decimal? Prize { get; set; }

    [ForeignKey("BoardId")]
    [InverseProperty("Winners")]
    public virtual Board Board { get; set; } = null!;

    [ForeignKey("GameId")]
    [InverseProperty("Winners")]
    public virtual Game Game { get; set; } = null!;

    [ForeignKey("PlayerId")]
    [InverseProperty("Winners")]
    public virtual AspNetUser Player { get; set; } = null!;
}
