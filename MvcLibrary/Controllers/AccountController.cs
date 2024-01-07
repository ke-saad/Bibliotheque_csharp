using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcLibrary.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Net;
using System.Text.Encodings.Web;

namespace MvcLibrary.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Adherent> _userManager;
        private readonly SignInManager<Adherent> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<Adherent> userManager,
            SignInManager<Adherent> signInManager,
            IConfiguration configuration,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Unable to load user with ID '{UserId}'.", userId);
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
            }

            return RedirectToAction("ConfirmEmailSuccess", "Account"); // A view that shows a successful confirmation message
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Adherent
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    Semester = model.Semester,
                    Department = model.Department
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", 
                        new { userId = user.Id, token = token }, Request.Scheme);

                    await SendEmailAsync(user.Email, confirmationLink, "Confirm your email");

                    // Do not sign in the user until they have confirmed their email
                    // Redirect to a view that informs the user to confirm their email
                    return RedirectToAction("RegisterConfirmation", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        private async Task SendEmailAsync(string email, string link, string subject)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["EmailSettings:FromName"], _configuration["EmailSettings:FromAddress"]));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(link)}'>clicking here</a>."
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration["EmailSettings:Host"], int.Parse(_configuration["EmailSettings:Port"]), SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        public IActionResult RegisterConfirmation()
        {
            return View(); 
        }
        public IActionResult ConfirmEmailSuccess()
        {
            return View(); 
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var encodedToken = WebUtility.UrlEncode(token);
                    var resetLink = Url.Action("ResetPassword", "Account", new { token = encodedToken, email = user.Email }, Request.Scheme);

                    try
                    {
                        await SendPasswordResetEmailAsync(user.Email, resetLink);
                        _logger.LogInformation("Password reset email sent to {Email}.", user.Email);
                        return RedirectToAction("ForgotPasswordConfirmation", "Account");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error sending password reset email for user {Email}.", user.Email);
                        ModelState.AddModelError("", "There was an error sending the password reset email. Please try again later.");
                    }
                }
                else
                {
                    _logger.LogWarning("Password reset requested for non-existent or unconfirmed email: {Email}", model.Email);
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
            }

            return View(model);
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Manage()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new ManageAccountViewModel { FullName = user.FullName };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(ManageAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (!string.IsNullOrEmpty(model.OldPassword) && !string.IsNullOrEmpty(model.NewPassword))
                {
                    var passwordCheck = await _userManager.CheckPasswordAsync(user, model.OldPassword);
                    if (passwordCheck)
                    {
                        var passwordChangeResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                        if (passwordChangeResult.Succeeded)
                        {
                            return RedirectToAction("Manage", new { Message = ManageMessageId.UpdateProfileSuccess });
                        }
                        else
                        {
                            foreach (var error in passwordChangeResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Incorrect old password.");
                    }
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Manage", new { Message = ManageMessageId.UpdateProfileSuccess });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
                    if (resetPassResult.Succeeded)
                    {
                        return RedirectToAction("ResetPasswordConfirmation", "Account");
                    }
                    foreach (var error in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User does not exist.");
                }
            }
            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        private async Task SendPasswordResetEmailAsync(string email, string link)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_configuration["EmailSettings:FromName"], _configuration["EmailSettings:FromAddress"]));
                message.To.Add(new MailboxAddress(string.Empty, email));
                message.Subject = "Password Reset";
                message.Body = new TextPart("plain") { Text = $"Please reset your password by clicking on this link: {link}" };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_configuration["EmailSettings:Host"], int.Parse(_configuration["EmailSettings:Port"]), SecureSocketOptions.StartTls);
                    _logger.LogInformation("Connected to SMTP server.");

                    await client.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                    _logger.LogInformation("Authenticated with SMTP server.");

                    await client.SendAsync(message);
                    _logger.LogInformation("Password reset email sent to {Email}.", email);

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email to {Email}.", email);
                throw; // Rethrow the exception to handle it in the calling method.
            }
        }

        public enum ManageMessageId
        {
            UpdateProfileSuccess,
            Error
        }
    }
}
