using EaglesNestMMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EaglesNestMMA.Controllers
{
    public class EmailController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Email
        public ActionResult Index()
        {
            return View();
        }

        // Called when user clicks the submit button and form values are passed into it
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendContactUsEmail(Contact contact)
        {
        
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();

                var name = contact.Name;
                var email = contact.Email;
                var phone = contact.PhoneNumber;
                var messages = contact.Message;
                var x = await EmailStatus(name, email, messages, phone);
                if (x == "sent")
                    ViewData["esent"] = "Your Message Has Been Sent";
                return RedirectToAction("Index", "Home");
            }

            var model = new Contact
            {
                Name = contact.Name,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
                Message = contact.Message
            };
            return View("Contact", model);

        }


        private async Task<string> EmailStatus(string name, string email, string messages, int phone)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress("etest1482@gmail.com"));    
            message.From = new MailAddress("etest1482@gmail.com");   
            message.Subject = "Message From" + email;
            message.Body = "Name: " + name + "\nFrom: " + email + "\nPhone: " + phone + "\nMessage: " + messages;
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "etest1482@gmail.com",  
                    Password = "Password1482%"   
                };

                // email server settings
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);

                return "sent";
            }
        }
    }
}