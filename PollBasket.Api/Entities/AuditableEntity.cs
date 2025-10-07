﻿namespace PollBasket.Api.Entities;

public class AuditableEntity
{
    public string CreatedByID { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? UpdatedByID { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ApplicationUser CreatedBy { get; set; } = default!;
    public ApplicationUser? UpdatedBy { get; set; }
}
