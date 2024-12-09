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

    public int Year { get; set; }

    public int WeekNumber { get; set; }

    public DateTime ValidFromDate { get; set; }

    public DateTime ValidUntilDate { get; set; }

    public DateTime RegisterCloseDate { get; set; }

    [StringLength(32)]
    public string? Status { get; set; }

    public List<int>? WinningSequence { get; set; }

    public DateTime? FinishedAt { get; set; }

    public int OnlinePlayers { get; set; }

    public int OfflinePlayers { get; set; }

    public int OnlineWinningPlayers { get; set; }

    public int OfflineWinningPlayers { get; set; }

    public int OnlineBoards { get; set; }

    public int OfflineBoards { get; set; }

    public int OnlineWinningBoards { get; set; }

    public int OfflineWinningBoards { get; set; }

    [Precision(10, 2)]
    public decimal OnlineIncome { get; set; }

    [Precision(10, 2)]
    public decimal OfflineIncome { get; set; }

    [Precision(10, 2)]
    public decimal OnlinePayout { get; set; }

    [Precision(10, 2)]
    public decimal OfflinePayout { get; set; }
}
