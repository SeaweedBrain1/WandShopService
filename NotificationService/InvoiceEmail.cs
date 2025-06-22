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
            _logger.LogInformation("Odebrano wiadomoœæ z Kafki");

            try
            {
                var json = message.Value;
                var order = JsonSerializer.Deserialize<OrderMessage>(json);

                if (order != null)
                {
                    await SendEmailAsync(order);
                    _logger.LogInformation($"Wys³ano maila do: {order.Email}");
                }
                else
                {
                    _logger.LogWarning("Nie uda³o siê sparsowaæ wiadomoœci.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"B³¹d podczas obs³ugi wiadomoœci: {ex.Message}");
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
                bodyBuilder.AppendLine("Dziêkujemy za zamówienie! Oto szczegó³y:\n");

                foreach (var item in order.Items)
                {
                    bodyBuilder.AppendLine($"- {item.ProductName} x {item.Quantity}");
                }

                bodyBuilder.AppendLine($"\n£¹czna kwota: {order.Total:C}");

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUsername),
                    Subject = "Twoje zamówienie w WandShop",
                    Body = bodyBuilder.ToString(),
                    IsBodyHtml = false
                };

                mailMessage.To.Add(order.Email);

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"B³¹d podczas wysy³ania e-maila: {ex.Message}");
            }
        }
    }
}
