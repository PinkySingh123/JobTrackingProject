//using Azure.Core;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Gmail.v1;
//using Google.Apis.Gmail.v1.Data;
//using Google.Apis.Services;
//using Google.Apis.Util.Store;
using JobTrackingProject.Application.Interfaces;
using JobTrackingProject.Configuration;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
//using MimeKit;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace JobTrackingProject.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;
        private readonly bool _simulateEmail;
        private readonly bool _useGmailApi;
        private readonly string _clientSecretPath;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly SmtpSettings _smtpSettings;


        public EmailService(ILogger<EmailService> logger, IConfiguration configuration, IOptions<SmtpSettings> smtpSettings)
        {
            _logger = logger;
            _configuration = configuration;
            _simulateEmail = _configuration.GetValue<bool>("EmailSettings:SimulateEmail");
            _useGmailApi = _configuration.GetValue<bool>("EmailSettings:UseGmailApi");
            _clientSecretPath = _configuration.GetValue<string>("EmailSettings:ClientSecretPath");
            _senderEmail = _configuration.GetValue<string>("EmailSettings:SenderEmail");
            _senderName = _configuration.GetValue<string>("EmailSettings:SenderName");
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.From),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }



        // ✅ Gmail API implementation
        //private async Task SendUsingGmailApiAsync(string toEmail, string subject, string body)
        //{
        //    try
        //    {
        //        if (!File.Exists(_clientSecretPath))
        //            throw new FileNotFoundException("Client secret file not found", _clientSecretPath);

        //        UserCredential credential;
        //        using (var stream = new FileStream(_clientSecretPath, FileMode.Open, FileAccess.Read))
        //        {
        //            string credPath = "token.json"; // stores access token
        //            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
        //                GoogleClientSecrets.Load(stream).Secrets,
        //                new[] { GmailService.Scope.GmailSend },
        //                "user",
        //                CancellationToken.None,
        //                new FileDataStore(credPath, true));
        //        }

        //        var service = new GmailService(new BaseClientService.Initializer()
        //        {
        //            HttpClientInitializer = credential,
        //            ApplicationName = "JobTrackingApp",
        //        });

        //        var email = new MimeMessage();
        //        email.From.Add(new MailboxAddress(_senderName, _senderEmail));
        //        email.To.Add(MailboxAddress.Parse(toEmail));
        //        email.Subject = subject;
        //        email.Body = new TextPart("plain") { Text = body };

        //        using (var ms = new MemoryStream())
        //        {
        //            await email.WriteToAsync(ms);
        //            var rawMessage = Convert.ToBase64String(ms.ToArray())
        //                .Replace('+', '-').Replace('/', '_').Replace("=", "");

        //            var gmailMessage = new Message { Raw = rawMessage };
        //            await service.Users.Messages.Send(gmailMessage, "me").ExecuteAsync();

        //            _logger.LogInformation($"✅ [Gmail API] Email sent successfully to {toEmail}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "❌ Error sending email via Gmail API");
        //        throw;
        //    }
        //}

        // ✅ SMTP implementation (fallback)
        private async Task SendUsingSmtpAsync(string toEmail, string subject, string message)
        {
            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings");
                var smtpHost = emailSettings["SmtpHost"];
                var smtpPort = int.Parse(emailSettings["SmtpPort"]);
                var smtpUser = emailSettings["SmtpUser"];
                var smtpPass = emailSettings["SmtpPass"];
                var enableSsl = bool.Parse(emailSettings["EnableSsl"]);

                using (var client = new SmtpClient(smtpHost))
                {
                    client.Port = smtpPort;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    client.EnableSsl = enableSsl;

                    var mailMessage = new MailMessage(_senderEmail, toEmail, subject, message);
                    await client.SendMailAsync(mailMessage);

                    _logger.LogInformation($"Email sent successfully to {toEmail}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ SMTP error sending email");
                throw;
            }
        }
    }
}
