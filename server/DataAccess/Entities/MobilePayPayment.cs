using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

public partial class MobilePayPayment
{
    [Key]
    public Guid PaymentId { get; set; }

    public Guid TransactionId { get; set; }

    [StringLength(50)]
    public string PaymentPhone { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    [ForeignKey("TransactionId")]
    [InverseProperty("MobilePayPayments")]
    public virtual Transaction Transaction { get; set; } = null!;
}
