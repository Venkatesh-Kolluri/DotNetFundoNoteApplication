using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace CommonLayer.Model
{
    public class MSMQModel
    {
        MessageQueue msgQueue = new MessageQueue();
        public void sendData2Queue(string Token)
        {
            msgQueue.Path = @".\private$\Token";
            if(MessageQueue.Exists(msgQueue.Path))
            {
                //Exists
            }
            else
            {
                MessageQueue.Create(msgQueue.Path);
            }
            msgQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            msgQueue.ReceiveCompleted += MsgQueue_ReceiveCompleted;
            msgQueue.Send("Desired Messages");
            msgQueue.BeginReceive();
            msgQueue.Close();

        }
        public void MsgQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = msgQueue.EndReceive(e.AsyncResult);
                string data = msg.Body.ToString();
                string subject = "Fundonote reset link";
                string body = data;
                var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("kollurivenkatesh97@gmail.com","usdhjficreugqlos"),
                    EnableSsl = true
                };
                smtp.Send("kollurivenkatesh97@gmail.com", body, subject, data);
                 msgQueue.BeginReceive();
            }
            catch (MessageQueueException qexception)
            {
                throw;
            }
        }

    }
}
