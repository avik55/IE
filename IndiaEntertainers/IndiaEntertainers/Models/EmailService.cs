using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace IndiaEntertainers.Models
{
    public class EmailServicess
    {
        public bool SendMail(EmailMessageModel message)
        {
            try
            {
                string username = "postmaster@mbteea.org";  //"admin@mbteea.org";
                string password = "Ayaz2015#";     //"%Lor99j7";

                //// Credentials:
                //var sendGridUserName = "yourSendGridUserName";
                //var sentFrom = "whateverEmailAdressYouWant";
                //var sendGridPassword = "YourSendGridPassword";

                //create the mail message 
                System.Net.Mail.MailMessage mymail = new System.Net.Mail.MailMessage();

                //set the addresses 
                mymail.From = new MailAddress("postmaster@mbteea.org"); //IMPORTANT: This must be same as your smtp authentication address.
                mymail.To.Add(message.Destination);

                //set the content 
                mymail.Subject = message.Subject;
                mymail.Body = message.Body;
                mymail.IsBodyHtml = true;

                //send the message 
                SmtpClient smtp = new SmtpClient("mail.mbteea.org");

                //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                NetworkCredential Credentials = new NetworkCredential(username, password);
                smtp.Credentials = Credentials;
                smtp.Send(mymail);

                return true;
            }
            catch (Exception e)
            {
                var error = e.Message.ToString();
                return false;
            }

        }

        public bool SendByGmail(EmailMessageModel message)
        {
            try
            {
                //Send mail using Gmail SMTP
                var GmailHost = "smtp.gmail.com";
                var GmailPort = 587; // Gmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.

                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential("mokcrm@gmail.com", "India0091#");
                smtp.Port = GmailPort;
                smtp.Host = GmailHost;
                smtp.Timeout = 10000;
                smtp.EnableSsl = true;
                //  smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //  smtp.UseDefaultCredentials = false;

                var message1 = new MailMessage();
                message1.From = new MailAddress("mokcrm@gmail.com");
                message1.To.Add(new MailAddress(message.Destination));
                message1.CC.Add(new MailAddress("info@indiaentertainers.com"));
               // message1.CC.Add(new MailAddress("athar@mokshasolutions.com"));
                message1.Subject = message.Subject;
                message1.Body = message.Body;
                message1.IsBodyHtml = true;
                smtp.Send(message1);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}