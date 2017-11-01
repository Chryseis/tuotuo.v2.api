using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public class EmailService: IEmailService
    {
        private  static SmtpClient smtpClient;

        private  static MailConfigurationSection mailConfig;

        //锁
        private static object obj = new object();

        private RetryPolicy policy;

        public EmailService()
        {
            //发送失败延迟发送
            policy = Policy.Handle<Exception>()
                           .WaitAndRetryAsync(new[]
                                              {
                                                TimeSpan.FromSeconds(30),
                                                TimeSpan.FromSeconds(60),
                                                TimeSpan.FromSeconds(90)
                                              });
        }

        //public EmailService(SmtpClient _smtpClient, MailConfigurationSection _mailConfig)
        //{
        //    smtpClient = _smtpClient;

        //    mailConfig = _mailConfig;
        //}


        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await policy.ExecuteAsync(async () =>
            {
                //单例生成SmtpClient对象
                if (smtpClient == null)
                {
                    lock (obj)
                    {
                        if (smtpClient == null)
                        {
                            smtpClient = new SmtpClient();
                            smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                        }

                    }
                }

                //单例生成邮箱配置
                if (mailConfig == null)
                {
                    lock (obj)
                    {
                        if (mailConfig == null)
                        {
                            mailConfig = ConfigurationManager.GetSection("mailConfig") as MailConfigurationSection;
                            if (mailConfig == null)
                            {
                                throw new Exception("找不到邮箱配置");
                            }
                        }

                    }
                }

                var mimeMessage = GenerateMailMessage(to, subject, body, isBodyHtml);
                try
                {
                    //断线重连
                    if (!smtpClient.IsConnected)
                    {
                        lock (obj)
                        {
                            if (!smtpClient.IsConnected)
                            {

                                int port = int.Parse(mailConfig.Port.Text);
                                smtpClient.Connect(mailConfig.Server.Text, port, mailConfig.EnableSsl.Text.Equals("true"));
                            }

                        }

                    }
                    //未认证则需要认证一下
                    if (!smtpClient.IsAuthenticated)
                    {
                        lock (obj)
                        {
                            if (!smtpClient.IsAuthenticated)
                            {
                                smtpClient.Authenticate(mailConfig.UserName.Text, mailConfig.Password.Text);
                            }
                        }
                    }
                    await smtpClient.SendAsync(mimeMessage);
                }
                catch (Exception ex)
                {
                    //计入日志
                    throw ex;
                }
            });
           
        }

       
        private MimeMessage GenerateMailMessage(string to, string subject, string body, bool isBodyHtml)
        {       
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(mailConfig.UserName.Text));
            mimeMessage.To.Add(new MailboxAddress(to));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart(isBodyHtml ? TextFormat.Html : TextFormat.Plain)
            {
                Text = body
            };
            return mimeMessage;
        }

      

    }
}
