﻿using Infrastructure.Email.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Infrastructure.Email.Customs.Interface;
using System.Text.Encodings.Web;

namespace Infrastructure.Email.Customs.Concrete
{
    public class CustomEmailSender : ICustomEmailSender
    {
        private readonly EmailConfig _config;

        public CustomEmailSender(IOptions<EmailConfig> config)
        {
            _config = config.Value;
        }

        public async Task SendCustomEmailAsync(string email, string subject, string htmlMessage)
        {
            
                using var message = new MailMessage();
                message.To.Add(new MailAddress(email));
                message.From = new MailAddress("profitdeneme@yandex.com");
                message.Subject = subject;
                message.Body = htmlMessage;
                message.IsBodyHtml = true;

                using var client = new SmtpClient("smtp.yandex.com.tr", 587);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("profitdeneme@yandex.com", "mqpgpfyybwmonsks");

                await client.SendMailAsync(message);
            
            
        }


        public async Task SendConfirmationEmailAsync(string email, string callbackUrl)
        {
            using var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress("profitdeneme@yandex.com");
            message.Subject = "Cinemagnesia Hesabınızın Onaylanması İçin Lütfen E-Postanızı Onaylayın";
            message.Body = @"<html>
        <body style='font-family: Arial, sans-serif; font-size: 14px; color: #000000;'>
            <p style='color: #e50914;'>Sayın Film Sever,</p>
            <p>Cinemagnesia'ya hoş geldiniz! Hesabınızın güvenliği için, e-posta adresinizi doğrulamanız gerekiyor.</p>
            <p>Hesabınızı aktif hale getirmek için lütfen aşağıdaki linke tıklayarak e-postanızı onaylayınız:</p>
            <a href='" + HtmlEncoder.Default.Encode(callbackUrl) + @"' style='background-color: #e50914; border: none; color: #ffffff; padding: 10px 15px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; margin-bottom: 15px;'>Onaylama Linki</a>
            <p>Bu e-posta sizin tarafınızdan istenmiş ve hizmetimizle ilgili bilgilendirmeler almak istediğinizi belirttiğiniz için gönderilmiştir. Eğer bu e-postayı isteğiniz dışında aldıysanız, lütfen dikkate almayınız ve bize bildirin.</p>
            <p>İyi seyirler!</p>
            <p style='color: #e50914; font-weight: bold;'>Cinemagnesia Ekibi</p>
        </body>
    </html>";
            message.IsBodyHtml = true;

            using var client = new SmtpClient("smtp.yandex.com.tr", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("profitdeneme@yandex.com", "mqpgpfyybwmonsks");

            await client.SendMailAsync(message);
        }

        public async Task SendBannedNotificationEmailAsync(string email, int? days)
        {
            using var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress("profitdeneme@yandex.com");
            message.Subject = "Hesap Engelleme Bildirimi";
            message.Body = $@"<html>
                        <head>
                             <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>
                            <title>Hesap Engelleme Bildirimi</title>
                        </head>
                        <body style='font-family: Arial, sans-serif; font-size: 14px; color: #000000;'>
                            <p style='color: #e50914; font-size: 18px;'>Merhaba,</p>
                            <p>Hesabınız {(days.HasValue ? days.Value + " gün" : "kalıcı olarak")} engellendiğini bildirmek istiyoruz.</p>
                            <p>Eğer hesabınızın engellenmesiyle ilgili herhangi bir sorunuz varsa, lütfen bizimle iletişime geçin.</p>
                            <p>İyi günler!</p>
                            <p style='color: #e50914; font-weight: bold; font-size: 16px;'>Cinemagnesia Ekibi</p>
                        </body>
                    </html>";

            message.IsBodyHtml = true;
            using var client = new SmtpClient("smtp.yandex.com.tr", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("profitdeneme@yandex.com", "mqpgpfyybwmonsks");

            await client.SendMailAsync(message);
        }



        public async Task SendMutedNotificationEmailAsync(string email, int? days)
        {
            using var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress("profitdeneme@yandex.com");
            message.Subject = "Hesap Engelleme Bildirimi";
            message.Body = $@"<html>
                        <head>
                             <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>
                            <title>Hesap Engelleme Bildirimi</title>
                        </head>
                        <body style='font-family: Arial, sans-serif; font-size: 14px; color: #000000;'>
                            <p style='color: #e50914; font-size: 18px;'>Merhaba,</p>
                            <p>Hesabınızın yorum yapma özelliğinin {(days.HasValue ? days.Value + " gün" : "kalıcı olarak")} engellendiğini bildirmek istiyoruz.</p>
                            <p>Eğer hesabınızın engellenmesiyle ilgili herhangi bir sorunuz varsa, lütfen bizimle iletişime geçin.</p>
                            <p>İyi günler!</p>
                            <p style='color: #e50914; font-weight: bold; font-size: 16px;'>Cinemagnesia Ekibi</p>
                        </body>
                    </html>";

            message.IsBodyHtml = true;
            using var client = new SmtpClient("smtp.yandex.com.tr", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("profitdeneme@yandex.com", "mqpgpfyybwmonsks");

            await client.SendMailAsync(message);
        }

        public async Task SendForgotPasswordEmailAsync(string email, string callbackUrl)
        {
            using var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress("profitdeneme@yandex.com");
            message.Subject = "Cinemagnesia Şifre Sıfırlama Talebi";
            message.Body = @"<body style='font-family: Arial, sans-serif; font-size: 14px; color: #000000;'>
                            <p style='color: #e50914;'>Merhaba,</p>
                            <p>Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayabilirsiniz:</p>
                            <p style='color: red;'><a href='" + HtmlEncoder.Default.Encode(callbackUrl) + @"' style='background-color: #e50914; border: none; color: #ffffff; padding: 10px 15px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; margin-bottom: 15px;'>Şifre Sıfırlama</a></p>
                            <p>Eğer şifrenizi sıfırlamak istemediyseniz, bu mesajı görmezden gelebilirsiniz.</p>
                            <p>İyi günler!</p>
                            <p style='color: #e50914; font-weight: bold;'>Cinemagnesia Ekibi</p>
                            </body>";



            message.IsBodyHtml = true;
            using var client = new SmtpClient("smtp.yandex.com.tr", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("profitdeneme@yandex.com", "mqpgpfyybwmonsks");

            await client.SendMailAsync(message);
        }

        public async Task SendProductorRequestCreatedEmailAsync(string email)
        {
            using var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress("profitdeneme@yandex.com");
            message.Subject = "Yapımcı İsteği Alındı";

            message.Body = $@"<html>
                        <head>
                             <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>
                            <title>Yapımcı İsteği Alındı</title>
                        </head>
                        <body style='font-family: Arial, sans-serif; font-size: 14px; color: #000000;'>
                            <p style='color: #e50914; font-size: 18px;'>Merhaba,</p>
                            <p>Yapımcı olma isteğiniz başarıyla alındı.</p>
                            <p>İstekleriniz doğrultusunda size en kısa sürede geri dönüş yapılacaktır.</p>
                            <p>Bu arada, yapmak istediğiniz içeriklerle ilgili herhangi bir örnek veya detaylı bilgiye ihtiyacınız olursa bizimle iletişime geçebilirsiniz.</p>
                            <p>Teşekkürler!</p>
                            <p style='color: #e50914; font-weight: bold; font-size: 16px;'>Cinemagnesia Ekibi</p>
                        </body>
                    </html>";

            message.IsBodyHtml = true;
            using var client = new SmtpClient("smtp.yandex.com.tr", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("profitdeneme@yandex.com", "mqpgpfyybwmonsks");

            await client.SendMailAsync(message);
        }


        public async Task SendProductorRequestApprovedEmailAsync(string email)
        {
            using var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress("profitdeneme@yandex.com");
            message.Subject = "Yapımcı İsteğiniz Onaylandı";
            message.Body = $@"<html>
                        <head>
                            <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
                            <title>Yapımcı İsteğiniz Onaylandı</title>
                        </head>
                        <body style='font-family: Arial, sans-serif; font-size: 14px; color: #000000;'>
                            <p style='color: #e50914; font-size: 18px;'>Merhaba,</p>
                            <p>Yapımcı isteğiniz onaylandı. Artık yapımcı olarak siteye içerik ekleyebilirsiniz.</p>
                            <p>İyi günler!</p>
                            <p style='color: #e50914; font-weight: bold; font-size: 16px;'>Cinemagnesia Ekibi</p>
                        </body>
                    </html>";

            message.IsBodyHtml = true;
            using var client = new SmtpClient("smtp.yandex.com.tr", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("profitdeneme@yandex.com", "mqpgpfyybwmonsks");

            await client.SendMailAsync(message);
        }


        public async Task SendProductorRequestRejectedEmailAsync(string email)
        {
            using var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress("profitdeneme@yandex.com");
            message.Subject = "Yapımcı Olma İsteğiniz Reddedildi";
            message.Body = @"<html>
                        <head>
                            <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                            <title>Yapımcı Olma İsteğiniz Reddedildi</title>
                        </head>
                        <body style='font-family: Arial, sans-serif; font-size: 14px; color: #000000;'>
                            <p>Merhaba,</p>
                            <p>Yapımcı olma isteğiniz maalesef reddedildi. Reddedilme sebebiyle ilgili herhangi bir sorunuz varsa, lütfen bizimle iletişime geçin.</p>
                            <p>İyi günler!</p>
                            <p style='color: #e50914; font-weight: bold;'>Cinemagnesia Ekibi</p>
                        </body>
                    </html>";

            message.IsBodyHtml = true;
            using var client = new SmtpClient("smtp.yandex.com.tr", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("profitdeneme@yandex.com", "mqpgpfyybwmonsks");

            await client.SendMailAsync(message);
        }

    }
}
