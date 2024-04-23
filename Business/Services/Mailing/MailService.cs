using Business.Services.Token;
using Data.DTOs.BookingDtos;
using Data.DTOs.UserDtos;
using Data.Entities;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Mailing
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ITokenService _tokenService;

        public MailService(MailSettings mailSettings, ITokenService tokenService)
        {
            _mailSettings = mailSettings;
            _tokenService = tokenService;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = mailRequest.Body;

            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();

            smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

        }

        //Pass the user's info to the token method

        public async Task SendForgotPasswordEmail(User user, string token)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Reset Forgoten Password!";
            var builder = new BodyBuilder();
            var link = "http://localhost:3000/forgotPassword/" + token;
            var filePath = Directory.GetCurrentDirectory() + "\\HTMLTemplates\\ChangePasswordTemplate.html";
            var template = File.ReadAllText(filePath);
            var htmlBody = template.Replace("{user_name}", user.Name).Replace("{link}", link);
            builder.HtmlBody = htmlBody;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendVerifyAccountEmail(UserCreateDto user)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Verify your email";
            var filePath = Directory.GetCurrentDirectory() + "\\HTMLTemplates\\VerifyEmailTemplate.html";
            var template = File.ReadAllText(filePath);
            var builder = new BodyBuilder();
            var token = user.AccountVerificationToken;
            var link = "http://localhost:3000/verifyAccount/" + token;

            var htmlBody = string.Format(template, link);
            builder.HtmlBody = htmlBody;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);

            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

        }

        public async Task SendEmailToCostumerWhenBookingAcceptedAsync(User user, BookingDto booking)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(user.Email));

            email.Subject = "Booking Accepted";

            var filePath = Directory.GetCurrentDirectory() + "\\HTMLTemplates\\OrderAcceptedTemplate.html";
            var builder = new BodyBuilder();

            var htmlBody = await File.ReadAllTextAsync(filePath);
            htmlBody = htmlBody.Replace("{{user.name}}", user.Name)
                                .Replace("{{user.surname}}", user.Surname)
                                .Replace("{{booking.id}}", booking.Id.ToString())
                                .Replace("{{bookig.status}}", booking.BookingStatuses.ToString())
                                .Replace("{{booking.totalprice}}", booking.TotalPrice.ToString())
                                .Replace("{booking.pickupdate}", booking.PickUpDateTime.ToString())
                                .Replace("{{booking.Returndatetime}}", booking.ReturnDateTime.ToString());

            builder.HtmlBody = htmlBody;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

        }



    }

}
