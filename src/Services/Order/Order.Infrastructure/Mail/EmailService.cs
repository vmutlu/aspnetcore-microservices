﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Order.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Order.Infrastructure.Mail
{
    public class EmailService : Application.Contracts.Infrastructure.IEmailService
    {
        public EmailSettings _emailSettings { get; }
        public ILogger<EmailService> _logger { get; }

        public EmailService(IOptions<EmailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmail(Email email)
        {
            var client = new SendGridClient(_emailSettings.APIKey);

            var subject = email.Subject;
            var to = new EmailAddress(email.To);
            var emailBody = email.Body;

            var from = new EmailAddress
            {
                Email = _emailSettings.FromAddress,
                Name = _emailSettings.FromName
            };

            var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
            var response = await client.SendEmailAsync(sendGridMessage).ConfigureAwait(false);

            _logger.LogInformation("Email sent.");

            if (response.StatusCode is System.Net.HttpStatusCode.Accepted || response.StatusCode is System.Net.HttpStatusCode.OK) return true;

            _logger.LogError("Email sending failed.");

            return false;
        }
    }
}
