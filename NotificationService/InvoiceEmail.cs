using System;
using System.Net.Mail;
using System.Net;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;

namespace NotificationService
{
    public class InvoiceEmail
    {
        private readonly ILogger<InvoiceEmail> _logger;

        public InvoiceEmail(ILogger<InvoiceEmail> logger)
        {
            _logger = logger;
        }

        [Function("KafkaTriggerFunction")]
        public async Task Run(
        [KafkaTrigger(
        "kafka:9092",
        "invoice-email-topic",
        ConsumerGroup = "function-consumer-group")]
        KafkaMessage message)
        {
            _logger.LogInformation("Odebrano wiadomo�� z Kafki");

            try
            {
                var json = message.Value;
                var order = JsonSerializer.Deserialize<OrderMessage>(json);

                if (order != null)
                {
                    await SendEmailAsync(order);
                    _logger.LogInformation($"Wys�ano maila do: {order.Email}");
                }
                else
                {
                    _logger.LogWarning("Nie uda�o si� sparsowa� wiadomo�ci.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"B��d podczas obs�ugi wiadomo�ci: {ex.Message}");
            }
        }

        static async Task SendEmailAsync(OrderMessage order)
        {
            try
            {
                string smtpHost = Environment.GetEnvironmentVariable("smtpHost");
                int smtpPort = Int32.Parse(Environment.GetEnvironmentVariable("smtpPort"));
                string smtpUsername = Environment.GetEnvironmentVariable("smtpUsername");
                string smtpPassword = Environment.GetEnvironmentVariable("smtpPassword");

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword)
                };

                var bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine("Dzi�kujemy za zam�wienie! Oto szczeg�y:\n");

                foreach (var item in order.Items)
                {
                    bodyBuilder.AppendLine($"- {item.ProductName} x {item.Quantity}");
                }

                bodyBuilder.AppendLine($"\n��czna kwota: {order.Total:C}");

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUsername),
                    Subject = "Twoje zam�wienie w WandShop",
                    Body = bodyBuilder.ToString(),
                    IsBodyHtml = false
                };

                mailMessage.To.Add(order.Email);

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"B��d podczas wysy�ania e-maila: {ex.Message}");
            }
        }
    }
}
