using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MvcLibrary.Models;

public partial class Book
{
    [Key]
    [Column("BookID")]
    public int BookId { get; set; }

    [StringLength(100)]
    public string? Title { get; set; }

    [StringLength(100)]
    public string? Author { get; set; }

    [Column("ISBN")]
    [StringLength(20)]
    public string? Isbn { get; set; }

    public int? PublicationYear { get; set; }

    public int? AvailableCopies { get; set; }

    [InverseProperty("Book")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
