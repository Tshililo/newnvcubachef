using MVCUbachef.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MVCUbachef.Controllers
{

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEdit(string id)
        {
            ViewBag.CuizineType = id;
            return PartialView("CreateMainMenuEditPartial");

        }


        public ActionResult SavePayments(string PeopleId)
        {

            List<string> p = PeopleId.Split(',').ToList();
            try
            {
                string NoOfPeople = p[0];
                string numberofplates = p[1];
                string emailId = p[2];
                string phoneNo = p[3];
                //////////////////////////////////////////////////////////////
                string Occasionvalue = p[4];
                string EventsType = p[5];
                string CuisineType = p[6];
                string AddressId = p[7];
                //////////////////////////////////////////////////////////////
                string FromTime = p[8];
                string ToTime = p[9];
                string TotalPriceId = p[10];
                string RequestDate = p[11];
                
                string fPhoneNo = phoneNo + ",";
                //////////////////////////////////////////////////////////////
                string RefNo = DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "") + RandomString(4, false);

                ViewBag.RefNo = RefNo;
              //  SendEmail(emailId, RefNo, Occasionvalue, EventsType, CuisineType, TotalPriceId);
              //  SendSMS(fPhoneNo, RefNo, Occasionvalue, EventsType, CuisineType, TotalPriceId);
                insertrecords(NoOfPeople, numberofplates, emailId, fPhoneNo, Occasionvalue, EventsType, CuisineType, RefNo, AddressId, FromTime, ToTime, RequestDate);

            }
            catch (Exception)
            {

                throw;
            }


            return PartialView("Payments");


        }

       // ubachefEntities db = new ubachefEntities();

        public void insertrecords(string PeopleId, string numberofplates, string emailId = "", string phoneNo = "",
            string Occasionvalue = "", string EventsType = "", string CuisineType = "", string RefNo = "", string AddressId = "",
            string FromTime = "", string ToTime = "", string RequestDate = "")
        {
            try
            {
                //var model = db.bookings;

                booking Tosave = new booking();

                Tosave.ObjId = Guid.NewGuid();
                Tosave.people = PeopleId;
                //Tosave.plates = numberofplates;
                Tosave.email = emailId;
                Tosave.occasion = Occasionvalue;
                //Tosave.Active = false;
                Tosave.datecapture = DateTime.Now.ToString("yyyy/MM/dd");
                Tosave.EventsType = EventsType;
                Tosave.cuisinetype = CuisineType;
                Tosave.phoneno = phoneNo;
                Tosave.refno = RefNo;
                Tosave.address = AddressId;
                Tosave.FromTime = FromTime;
                Tosave.ToTime = ToTime;
                Tosave.RequestDate = RequestDate;
                Tosave.Accepted = "No";

                //This is my insert query in which i am taking input from the user through windows forms  
                string Query = "insert into ubachef.Booking(ObjId,NoOfPeople,email,occasion,datecapture,EventsType,cuisinetype,phoneno,refno,address,FromTime,ToTime,RequestDate,Accepted) values('" + 
                    Tosave.ObjId + "','" + Tosave.people + "','" + Tosave.email + "','" + Tosave.occasion + "','" + Tosave.datecapture + "','" + 
                    Tosave.EventsType + "','" + Tosave.cuisinetype
                    + "','" + Tosave.phoneno + "','" + Tosave.refno + "','" + Tosave.address 
                    + "','" + Tosave.FromTime + "','" + Tosave.ToTime + "','" + Tosave.RequestDate + "','" + Tosave.Accepted + "');";

                MySqlConnection MyConn2 = new MySqlConnection(connectionString);
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                MySqlDataReader MyReader2;
                MyConn2.Open();
                MyReader2 = MyCommand2.ExecuteReader();     // Here our query will be executed and data saved into the database.  

                //while (MyReader2.Read())
                //{
                //    return "Procord Not Processed";
                //}
                MyConn2.Close();

            }
            catch (Exception)
            {

                throw;
            }


        }

        public void SendSMS(string PhoneNo, string RefNo, string Occasionvalue = "", 
            string EventsType = "", string CuisineType = "", string TotalPriceId = "", string FromTime = "", string ToTime = "")
        {

            
            string to, msg;
            msg = "Dear Customer. You have Booked a Chef for " + CuisineType +" "+ "From: " + FromTime +" To: " + ToTime + "RefNo: " + RefNo + ". Amount to pay " + TotalPriceId;
            to = PhoneNo;
            WebClient client = new WebClient();
            // Add a user agent header in case the requested URI contains a query.
            client.Headers.Add("user-agent", "Mozilla/4.0(compatible; MSIE 6.0; Windows NT 5.2; .NET CLR1.0.3705;)");
            client.QueryString.Add("user", "RomeoMulaudzi");
            client.QueryString.Add("password", "dAAdJKAHTfOJYc");
            client.QueryString.Add("api_id", "3546405");
            client.QueryString.Add("to", to);
            client.QueryString.Add("text", msg);
            string baseurl = "http://api.clickatell.com/http/sendmsg";
            Stream data = client.OpenRead(baseurl);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            //data.Close();
            reader.Close();

        }

        private void SendEmail(string EmailAddress, string RefNo, string Occasionvalue = "", string EventsType = "", string CuisineType = "", string TotalPriceId = "")
        {
            MailMessage Msg = new MailMessage();
            // Sender e-mail address.
            Msg.From = new MailAddress("ubachefnoreply@gmail.com");
            // Recipient e-mail address.
            Msg.To.Add(EmailAddress);
            Msg.Subject = "Uba Chef Booking Details";
            Msg.Body = "Dear Customer. You have Booked a Chef for " + CuisineType + " " + "Occasion: " + Occasionvalue + " for " + EventsType + " Event. RefNo: " + RefNo + ". Amount to pay " + TotalPriceId;
            Msg.IsBodyHtml = true;
            // your remote SMTP server IP.
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("ubachefnoreply@gmail.com", "ubachefnoreply27");
            smtp.EnableSsl = true;
            smtp.Send(Msg);
        }

        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int Place = Source.LastIndexOf(Find);
            string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }

        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

    }
}