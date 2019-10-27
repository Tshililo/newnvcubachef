using MVCUbachef.Models;
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
    public class HomeController : Controller
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
            string RefNo = DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "") + RandomString(4, false);

            ViewBag.RefNo = RefNo;

            List<string> p = PeopleId.Split(',').ToList();

            string NoOfPeople = p[0];
            string numberofplates = p[1];
            string emailId = p[2];
            string phoneNo = p[3];
            string Occasionvalue = p[4];
            string EventsType = p[5];
            string CuisineType = p[6];

            return PartialView("Payments");


        }

        //public ActionResult SavePayments(string PeopleId, string numberofplates, string emailId = "", string phoneNo = "", string Occasionvalue = "", string EventsType = "", string CuisineType = "")
        //{
        //    string RefNo = "UbaChef" + RandomString(9, false);

        //    ViewBag.RefNo = RefNo;

        //    List<string> p = passed_params.Split(',').ToList();
        //    var rep = new AppReportXrMvc();
        //    rep.ObjId.Value = p[0];
        //    rep.Attention.Value = p[1];
        //    rep.FromDate.Value = p[2];
        //    rep.ToDate.Value = p[4];
        //    rep.ShowDual.Value = p[6];

        //    return PartialView("Payments");


        //}

        public void SendSMS(string PhoneNo, string RefNo, string Occasionvalue = "", string EventsType = "")
        {
            string to, msg;
            msg = "Dear Customer. You have Booked a Chef for Occasion: " + Occasionvalue + " for " + EventsType + " Event. RefNo:" + RefNo + ""; ;
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

        private void SendEmail(string EmailAddress, string RefNo, string Occasionvalue = "", string EventsType = "")
        {
            MailMessage Msg = new MailMessage();
            // Sender e-mail address.
            Msg.From = new MailAddress("ubachefnoreply@gmail.com");
            // Recipient e-mail address.
            Msg.To.Add(EmailAddress);
            Msg.Subject = "Dear Client";
            Msg.Body = "You have Booked a Chef for Occasion: " + Occasionvalue + " for " + EventsType + " Event. RefNo" + RefNo + "";
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

        private string insert(string PeopleId, string numberofplates, string emailId = "", string phoneNo = "", string Occasionvalue = "", string EventsType = "")
        {
            try
            {
                Guid ObjId = Guid.NewGuid();
                string NoOfPeople = PeopleId;
                string NoOfPlates = numberofplates;
                string Date = DateTime.Now.ToString();
                string occasionvalue = Occasionvalue;
                string RefNo = "UbaChef" + RandomString(9, false);
                string Eventstype = EventsType;
                string FNUmber = phoneNo + ",";

                ////This is my insert query in which i am taking input from the user through windows forms  
                //string Query = "insert into ubachef.Payments(ObjId,NoOfPeople,NoOfPlates,Date,RefNo,emailId,phoneNo,Occasion,EventsType) values('" + ObjId + "','" + 
                //    NoOfPeople + "','" + NoOfPlates + "','" + Date + "','" + RefNo + "','" + emailId + "','" + FNUmber + "','" + occasionvalue + "','" + EventsType + "');";
                //MySqlConnection MyConn2 = new MySqlConnection(gg.MyConnection2);
                //MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                //MySqlDataReader MyReader2;
                //MyConn2.Open();
                //MyReader2 = MyCommand2.ExecuteReader();     // Here our query will be executed and data saved into the database.  

                //while (MyReader2.Read())
                //{
                //    return "Procord Not Processed";
                //}
                //MyConn2.Close();


                SendEmail(emailId, RefNo, occasionvalue, Eventstype);
                SendSMS(emailId, RefNo, occasionvalue, Eventstype);
                return "Procord Processed";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }


        }
    }
}