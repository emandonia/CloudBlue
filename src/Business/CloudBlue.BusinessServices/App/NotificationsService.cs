using CloudBlue.Domain.DataModels;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace CloudBlue.BusinessServices.App;




public class NotificationsService(ILookUpsService lookUpsService, INotificationsRepository repo) : INotificationsService
{

    private async Task SendSmsNotification(List<LeadTicketInfoForEmail> leads)
    {
        foreach (var lead in leads)
        {
            if (string.IsNullOrEmpty(lead.AgentMobile))
            {
                continue;

            }


            var rawMessage =
                "You have a New lead assigned to you, name {0}, Tel {1}. Request {2} in {3}, PLZ handle ASAP";

            var message = string.Format(rawMessage, lead.ClientName, lead.ClientPhone, lead.PropertyTypeName,
                  lead.District);

            var log = new LeadTicketNotificationLog
            {
                AgentId = lead.AgentId,
                ClientId = lead.ClientId,
                RecipientPhone = lead.AgentMobile,
                LeadTicketId = lead.Id,
                SendingDate = DateTime.UtcNow,
                SendingDateNumeric = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")),
                SendingStatus = 1,
                IsEmail = false,
                SentMessage = message,
                SourceSystem = "CloudBlue CRM",
                RecipientEmail = "",



            };
            _logs.Add(log);
            await SendSms(message, lead.AgentMobile.TrimStart(new char[] { '0' }), log);
        }












    }



    private async Task SendSms(string smsText, string receiverMsisdn, LeadTicketNotificationLog smsLog)
    {
        var accountId = "200003486";
        var password = "Vodafone.1";
        var senderName = "CB EGYPT";

        var concatStr =
            $"AccountId={accountId}&Password={password}&SenderName={senderName}&ReceiverMSISDN={receiverMsisdn}&SMSText={smsText}";

        var secureHash = BitConverter.ToString((new HMACSHA256(Encoding.UTF8.GetBytes("4B6EEA3380014B91949B63BCE5C452F2")))
            .ComputeHash(Encoding.UTF8.GetBytes(concatStr))).Replace("-", "");

        var requestXml = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?><SubmitSMSRequest xmlns=\"http://www.edafa.com/web2sms/sms/model/\"><AccountId>{accountId}</AccountId><Password>{password}</Password><SecureHash>{secureHash}</SecureHash><SMSList><SenderName>{senderName}</SenderName><ReceiverMSISDN>{receiverMsisdn}</ReceiverMSISDN><SMSText>{smsText}</SMSText></SMSList></SubmitSMSRequest>";
        var destinationUrl = "https://e3len.vodafone.com.eg/web2sms/sms/submit/";
        smsLog.SentMessage = requestXml;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
        byte[] bytes;
        bytes = Encoding.UTF8.GetBytes(requestXml);
        //  request.ContentType = "text/xml; encoding='utf-8'";
        request.ContentType = "application/xml";
        request.Accept = "application/xml";
        request.ContentLength = bytes.Length;
        request.Method = "POST";
        string responseStr = "";

        try
        {
            var requestStream = request.GetRequestStream();
            await requestStream.WriteAsync(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)await request.GetResponseAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseStream = response.GetResponseStream();
                responseStr = await new StreamReader(responseStream).ReadToEndAsync();
                smsLog.SendingResponse = responseStr;
                smsLog.SendingStatus = 2;
            }
            else
            {
                smsLog.SendingResponse = responseStr;
                smsLog.SendingStatus = 3;
            }
        }
        catch (Exception e)
        {
            smsLog.SendingResponse = e.ToString();
            smsLog.SendingStatus = 3;
        }
    }

    List<LeadTicketNotificationLog> _logs = new List<LeadTicketNotificationLog>();
    private async Task SendEmail(string email, string msg, string[] ccList, LeadTicketNotificationLog log,
        string subject)
    {

        try
        {
            var mail = new MailMessage
            {
                From = new MailAddress("mycbsupport@cb-ia.com", "CloudBlue Support Team"),
                Subject = subject
            };

            mail.To.Add(new MailAddress(email));

            foreach (var eml in ccList)
            {
                mail.CC.Add(new MailAddress(eml));
            }

            mail.IsBodyHtml = true;
            mail.Body = msg;
            var smtp = new SmtpClient();

            //smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new NetworkCredential("mycbsupport@cb-ia.com", "P@password2023");

            //      ("tempemailforbetna@gmail.com", "cbbetna12312377");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.Port = 587;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            await smtp.SendMailAsync(mail);
            log.SendingStatus = 2;
            log.SendingResponse = "Email Sent Successfully";
        }
        catch (Exception ex)
        {
            log.SendingStatus = 3;
            log.SendingResponse = ex.Message;
        }

    }


    public async Task SendAssignedLeadTicketNotificationAsync(List<LeadTicketInfoForEmail> leads)
    {

        if (leads.Count <= 0)
        {
            return;
        }

        _logs = new();
        var services = await lookUpsService.GetServicesAsync();
        var propertyTypes = await lookUpsService.GetPropertyTypesAsync(0);
        var salesUsers = await lookUpsService.GetSalesPersonsAsync([], true);
        var tobeNotifiedAgentId = leads.First()
            .AgentId;

        var currentAgent = salesUsers.FirstOrDefault(z => z.AgentId == tobeNotifiedAgentId);

        if (currentAgent == null || string.IsNullOrEmpty(currentAgent.AgentEmail))
        {
            return;
        }

        leads.ForEach(z =>
        {
            z.AgentMobile = currentAgent.AgentPhone;
            z.AgentName = currentAgent.AgentName;
        });


        var lst = new List<int> { currentAgent.DirectManagerId };

        if (currentAgent.DirectManagerId != currentAgent.TopMostManagerId)
        {
            lst.Add(currentAgent.TopMostManagerId);
        }
        var recRecord = salesUsers.FirstOrDefault(z => z.PositionId == 16 && ((z.TeamsIds != null && z.TeamsIds.Length > 0 && z.TeamsIds.Contains(currentAgent.TopMostManagerId)) || z.BranchId == currentAgent.BranchId));

        if (recRecord != null)
        {
            lst.Add(recRecord.AgentId);
        }

        var ccList = salesUsers.Where(z => lst.Contains(z.AgentId) && string.IsNullOrEmpty(z.AgentEmail) == false).Select(z => z.AgentEmail).ToArray();

        var msglist = new List<string>();



        foreach (var record in leads)
        {
            var prptype = propertyTypes.FirstOrDefault(z => z.ItemId == record.PropertyTypeId);

            if (prptype != null)
            {
                record.PropertyTypeName = prptype.ItemName;
            }
            var service = services.FirstOrDefault(z => z.ItemId == record.ServiceId);

            if (service != null)
            {
                record.ServiceName = service.ItemName;
            }

            msglist.Add(string.Format(
                "Client Name: {0} - Service Type: {1} - Property Type: {2}. <a href='https://cloudblue-eg.comlead-tickets/manage/{3}'>Peek</a>  ",
                 record.ClientName, record.ServiceName,
                record.PropertyTypeName, record.Id));




        }

        var bodymsg = leads.Count > 1 ? "are new clients" : "is a new client";

        var msg = string.Format(
            "Dear {0}, <br/> A few minutes ago there {1} assigned to you,<br/>{2} <br/> CloudBlue Support",
            currentAgent.AgentName, bodymsg, string.Join("<br/>", msglist.ToArray()));


        var log = new LeadTicketNotificationLog
        {
            AgentId = currentAgent.AgentId,
            ClientId = 0,
            IsEmail = true,
            LeadTicketId = 0,
            RecipientEmail = currentAgent.AgentEmail,
            RecipientPhone = "",
            SendingDate = DateTime.UtcNow,
            SendingDateNumeric = UtilityFunctions.GetLongFromDate(DateTime.UtcNow),
            SendingStatus = 1,
            SentMessage = msg,
            SourceSystem = "CloudBlue CRM",


        };



        _logs.Add(log);
        await SendEmail(currentAgent.AgentEmail, msg, ccList, log, "Assigned Leads");

        await SendSmsNotification(leads);

        await repo.AddLeadTicketNotificationLogsAsync(_logs);

    }

    public async Task SendResetPasswordAsync(string password, string userEmail, string userFullName, int userId,
        string agentMobile)
    {


        var name = "";

        if (string.IsNullOrEmpty(userFullName) == false)
        {
            var idx = userFullName.IndexOf(" ");

            if (idx > -1)
            {
                name = userFullName.Substring(0, idx);
            }
            else
            {
                name = userFullName;
            }
        }
        _logs = new();

        if (string.IsNullOrEmpty(userEmail) == false)
        {
            var subject = "Your CloudBlue Temporary Password – Please Log In and Update";

            var msg =
                @$"Dear {name},<br/> Welcome! <br/>Your password has been reset, and a temporary password has been generated for your login.<br/> Temporary Password:{password}<br/> Please log in using the link below and change your password as soon as possible to secure your account:<br/> <a href='https://cloudBlue-eg.com/login'>Log In</a><br/> If you did not request this, or believe this was sent in error, please contact our support team immediately.<br/> Best regards,<br/> CLoudBlue Support Team<br/> ";


            var emailLog = new LeadTicketNotificationLog
            {
                AgentId = userId,
                ClientId = 0,
                IsEmail = true,
                LeadTicketId = 0,
                RecipientEmail = userEmail,
                RecipientPhone = "Reset Password",
                SendingDate = DateTime.UtcNow,
                SendingDateNumeric = UtilityFunctions.GetLongFromDate(DateTime.UtcNow),
                SendingStatus = 1,
                SentMessage = msg,
                SourceSystem = "CloudBlue CRM",
            };



            _logs.Add(emailLog);
            await SendEmail(userEmail, msg, [], emailLog, subject);
        }


        if (string.IsNullOrEmpty(agentMobile) == false)
        {
            var message =
                $"Hi {name}, your temporary password is:{password}. Please log in at https://clousblue-eg.com/login and change your password right away.";


            var log = new LeadTicketNotificationLog
            {
                AgentId = userId,
                ClientId = 0,
                RecipientPhone = agentMobile,
                LeadTicketId = 0,
                SendingDate = DateTime.UtcNow,
                SendingDateNumeric = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")),
                SendingStatus = 1,
                IsEmail = false,
                SentMessage = message,
                SourceSystem = "CloudBlue CRM",
                RecipientEmail = "Reset Password",
            };

            _logs.Add(log);
            await SendSms(message, agentMobile.TrimStart(new char[] { '0' }), log);

            await repo.AddLeadTicketNotificationLogsAsync(_logs);
        }

    }

    //}
    //catch (Exception)
    //{

    //}
}
