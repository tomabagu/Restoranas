using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Restoranas.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Restoranas.Interfaces;

namespace Restoranas.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        public EmailService(string smtpServer, int smtpPort, string smtpUser, string smtpPass)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(_smtpUser);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = false;

                var smtpClient = new SmtpClient(_smtpServer)
                {
                    Port = _smtpPort,
                    Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                    EnableSsl = true,
                };

                smtpClient.Send(mail);
                Console.WriteLine("Email sent successfully. Press any key to continue");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                Console.ReadKey();
            }
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; //regex for email
            return Regex.IsMatch(email, pattern);
        }
    }
}
