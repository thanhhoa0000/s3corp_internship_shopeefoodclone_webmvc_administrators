﻿namespace ShopeeFoodClone.WebMvc.Administrators.Application.Models.Requests;

public sealed class VendorUpdateStoreRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(20)]
    public string? WardCode { get; set; }
    [MaxLength(100)]
    public string? StreetName { get; set; }
    [Required, MinLength(10), MaxLength(50)]
    public string? Name { get; set; }
    [Required]
    public TimeOnly OpeningHour { get; set; }
    [Required]
    public TimeOnly ClosingHour { get; set; }
    public string? CoverImagePath { get; set; }
    public Guid ConcurrencyStamp { get; set; }
}
