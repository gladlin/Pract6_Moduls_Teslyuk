using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Практическая_3.Pages;
using System.Windows;

namespace Практическая_3.Services
{
    internal class MailService
    {
        /// <summary>
        /// Отправляет письмо на почту с кодом подтверждения
        /// </summary>
        /// <param name="userEmail">адрес почты, на которую надо отправить письмо с кодом</param>
        /// <param name="sendcode">код подтверждения, отправленный для подтверждения почты</param>
        /// <returns>True если письмо получилось отправить, false если не получилось отправить письмо</returns>

        public MailService()
        {
        }

        public bool SendPasswordResetEmail(string userEmail, int sendcode)
        {
            MailAddress from = new MailAddress("alya.teslyuk13@inbox.ru", "Alina");
            MailAddress to = new MailAddress(userEmail);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Сброс пароля";
           
            m.Body = $"<h2>Ваш код подтверждения:{sendcode}</h2>";
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
            smtp.Credentials = new NetworkCredential("alya.teslyuk13@inbox.ru", "WY2VqdUMzBWTmrLJQYVx");
            smtp.EnableSsl = true;
            try
            {
                Console.WriteLine("Пытаюсь отправить письмо...");
                smtp.Send(m);
                Console.WriteLine("Письмо успешно отправлено.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при отправке письма: " + ex.Message);
                MessageBox.Show(ex.Message);
                return false;
            }

        }
    }
}
