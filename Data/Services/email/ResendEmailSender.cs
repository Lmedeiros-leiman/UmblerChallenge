using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Resend;

namespace UmbraChallenge.Data.Services
{
    public class ResendEmailSender<TUser> : IEmailSender<TUser> where TUser : class
    {
        private readonly ILogger _logger;
        private readonly IResend _resend;

        public ResendEmailSender(IResend resend, ILogger<ResendEmailSender<TUser>> logger) {
            _logger = logger;
            _resend = resend;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage) {
            _logger.LogInformation("Sending email to {email} with subject {subject}, this is the text message: \n {htmlMessage}", email, subject, htmlMessage);
            var message = new EmailMessage {
                From = "TestingMail@resend.dev",
                Subject = subject,
                HtmlBody = htmlMessage
            };
            message.To.Add(email);
            await _resend.EmailSendAsync(message);
        }
        

        public async Task SendConfirmationLinkAsync(TUser user, string email, string confirmationLink) {
            await SendEmailAsync(email, "Confirm your email", $"<a href=\"{confirmationLink}\">Click here to confirm your email</a>");
        }
        public async Task SendPasswordResetCodeAsync(TUser user, string email, string resetCode) {
            await SendEmailAsync(email, "Password Reset Code:", $"<h2>"+resetCode+"</h2>");
        }
        
        public async Task SendPasswordResetLinkAsync(TUser user, string email, string resetLink) {
            await SendEmailAsync(email, "Confirm your email", $"<a href=\"{resetLink}\">Click here to reset your password</a>");
        }

    }
}