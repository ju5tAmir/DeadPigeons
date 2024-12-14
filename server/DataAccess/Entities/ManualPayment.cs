using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

[Table("ManualPayment")]
public partial class ManualPayment
{
    [Key]
    public Guid TransactionId { get; set; }

    [StringLength(200)]
    public string ImagePath { get; set; } = null!;

    [StringLength(500)]
    public string? Note { get; set; }

    [ForeignKey("TransactionId")]
    [InverseProperty("ManualPayment")]
    public virtual Transaction Transaction { get; set; } = null!;
}
