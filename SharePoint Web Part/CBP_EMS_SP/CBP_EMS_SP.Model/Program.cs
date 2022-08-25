using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBP_EMS_SP.Model
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var db = new CyberportEMS_EDM())
            {

                var cf = new ContactForm { 
                    Name = "Cato Yeung",
                    Email = "cyeung@kbquest.com",
                    Phone = "98765432",
                    Inquiry = "Some questions"
                };
                db.ContactForms.Add(cf);
                db.SaveChanges();
            }
        } 
    }
}
