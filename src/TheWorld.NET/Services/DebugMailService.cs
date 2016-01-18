using System;
using System.Diagnostics;
using TheWorld.Services;

namespace TheWorld
{
    public class DebugMailService : IMailService
    {
        public bool SendMail(string to, string from, string subject, string body)
        {
            Debug.WriteLine($"Sending mail: To: {to}, Subject: {subject}");
            return true;
        }
    }
}