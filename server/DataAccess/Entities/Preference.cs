using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

[Index("UserId", Name = "Preferences_UserId_key", IsUnique = true)]
public partial class Preference
{
    [Key]
    public string UserSettingsId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public bool? IfBalanceIsNegative { get; set; }

    public bool? IfPlayerWon { get; set; }

    [StringLength(20)]
    public string? NotificationType { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Preference")]
    public virtual AspNetUser User { get; set; } = null!;
}
