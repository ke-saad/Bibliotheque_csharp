using MvcLibrary.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Reservation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReservationId { get; set; }

    // This should match the type of the Id column in the AspNetUsers table, which is int in your case
    public int UserId { get; set; }

    [Column("BookID")]
    public int? BookId { get; set; }

    public DateTime? ReservationDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("Reservations")]
    public virtual Book? Book { get; set; }

    // Renamed from Member to User to align with Identity conventions
    [ForeignKey("UserId")]
    [InverseProperty("Reservations")]
    public virtual Adherent? User { get; set; } // Rename the navigation property to User
}
