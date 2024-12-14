using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

public partial class Transaction
{
    [Key]
    public Guid TransactionId { get; set; }

    public string UserId { get; set; } = null!;

    [StringLength(50)]
    public string PaymentMethod { get; set; } = null!;

    [Precision(10, 2)]
    public decimal Amount { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    public DateTime TransactionDate { get; set; }

    [InverseProperty("Transaction")]
    public virtual ManualPayment? ManualPayment { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Transactions")]
    public virtual AspNetUser User { get; set; } = null!;
}
