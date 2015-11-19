using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AUPPRB.Common.Utils.Web
{
    public class MailHelper
    {
        /// <summary>
        ///     Отправляет по указанному адресу сообщение
        /// </summary>
        /// <returns></returns>
        public static bool SendEmailMessage(string emailReceiver, string messageTheme, string messageBody,
            out string error)
        {
            try
            {
                //TODO:Указываем настройки почтового сервера
                var client = new SmtpClient(ConfigurationManager.AppSettings["host"],
                    int.Parse(ConfigurationManager.AppSettings["port"])) {Credentials = new MyCredentials()};

                //TODO:Формируем сообщение
                var message = new MailMessage();
                message.Subject = messageTheme;
                message.BodyEncoding = Encoding.UTF8;
                message.From = new MailAddress(ConfigurationManager.AppSettings["userFrom"]);
                message.To.Add(new MailAddress(emailReceiver));
                message.Body = messageBody;

                client.Send(message);
                error = "";
                return true;
            }
            catch (Exception ex)
            {
                error = string.Format("Error: {0} {1} {2}", ex.Message, ex.InnerException, ex.StackTrace);
                return false;
            }
        }
    }

    public class MyCredentials : ICredentialsByHost
    {
        #region ICredentialsByHost Members

        public NetworkCredential GetCredential(string host, int port, string authenticationType)
        {
            NetworkCredential credential;
            if (bool.Parse(ConfigurationManager.AppSettings["IsLocalSMTP"]))
            {
                credential = new NetworkCredential(ConfigurationManager.AppSettings["userName"],
                    ConfigurationManager.AppSettings["userPassword"],
                    ConfigurationManager.AppSettings["domain"]);
            }
            else
            {
                credential = new NetworkCredential(ConfigurationManager.AppSettings["userName"],
                    ConfigurationManager.AppSettings["userPassword"]);
            }

            return credential;
        }

        #endregion
    }
}
