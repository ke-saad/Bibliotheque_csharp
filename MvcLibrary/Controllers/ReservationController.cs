using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using MvcLibrary.Data;
using MvcLibrary.Models;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MailKit.Security;


namespace MvcLibrary.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Adherent> _userManager;
        private readonly IConfiguration _configuration;

        public ReservationController(ApplicationDbContext dbContext, UserManager<Adherent> userManager, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _configuration = configuration;
        }

        // GET: Reservations
        public IActionResult Index()
        {
            var reservations = _dbContext.Reservations.ToList();
            return View(reservations);
        }

        // POST: ReserveBook
        [HttpPost]
        public async Task<IActionResult> ReserveBook(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var book = await _dbContext.Books.FindAsync(bookId);
            if (book == null)
            {
                // No book found with the given ID
                TempData["ErrorMessage"] = "Book not found.";
                return RedirectToAction("Error", "Home");
            }

            if (book.AvailableCopies <= 0)
            {
                // Book is not available for reservation
                TempData["ErrorMessage"] = "Book not available for reservation.";
                return RedirectToAction("Error", "Home");
            }

            var existingReservation = _dbContext.Reservations
                .Any(r => r.UserId == user.Id && r.BookId == bookId);
            if (existingReservation)
            {
                // User has already reserved this book
                TempData["ErrorMessage"] = "You have already reserved this book.";
                return RedirectToAction("Error", "Home");
            }

            // Proceed with reservation
            var reservation = new Reservation
            {
                UserId = user.Id,
                BookId = bookId,
                ReservationDate = DateTime.UtcNow
            };

            book.AvailableCopies--;
            _dbContext.Add(reservation);
            await _dbContext.SaveChangesAsync();

            // Send the email with PDF
            var pdfFilePath = GenerateReservationPDF(reservation);
            await SendEmailWithAttachmentAsync(user.Email, pdfFilePath);

            // Reservation successful
            TempData["SuccessMessage"] = "Your reservation is successful and a confirmation has been sent to your email.";
            return RedirectToAction("Success", "Home");
        }

        private string GenerateReservationPDF(Reservation reservation)
        {
            string pdfFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reservations", $"Reservation_{reservation.ReservationId}.pdf");

            using (var writer = new PdfWriter(pdfFilePath))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    using (var document = new Document(pdf))
                    {
                        document.Add(new Paragraph($"Reservation ID: {reservation.ReservationId}"));
                        document.Add(new Paragraph($"Member ID: {reservation.UserId}"));
                        document.Add(new Paragraph($"Book ID: {reservation.BookId}"));

                        string reservationDateString = reservation.ReservationDate.HasValue
                            ? reservation.ReservationDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                            : "Date not set";
                        document.Add(new Paragraph($"Reservation Date: {reservationDateString}"));
                    } // The Document object is automatically closed when exiting this using block
                }
            }

            return pdfFilePath;
        }
        //indexer check

        private async Task SendEmailWithAttachmentAsync(string userEmail, string attachmentFilePath)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_configuration["EmailSettings:FromName"], _configuration["EmailSettings:FromAddress"]));
            emailMessage.To.Add(MailboxAddress.Parse(userEmail));
            emailMessage.Subject = "Book Reservation Confirmation";

            var builder = new BodyBuilder { TextBody = "Here is your reservation confirmation." };
            builder.Attachments.Add(attachmentFilePath);
            emailMessage.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                // Note the use of StartTls when connecting to port 587
                await client.ConnectAsync(_configuration["EmailSettings:Host"], int.Parse(_configuration["EmailSettings:Port"]), SecureSocketOptions.StartTls);

                // If your SMTP server requires authentication
                await client.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

    }
}
