using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace MVCUbachef.Controllers
{
    // [Authorize]
    public class BaseController : Controller
    {
        internal void CopyProperties(object source, object target, string[] skip = null)
        {
            var customerType = target.GetType();
            foreach (var prop in source.GetType().GetProperties())
            {
                if (skip != null && skip.Contains(prop.Name))
                    continue;
                // prop.Attributes

                if (customerType.GetProperty(prop.Name) == null)
                    continue;

                var propSetter = customerType.GetProperty(prop.Name).GetSetMethod();
                var propGetter = prop.GetGetMethod();
                var valueToSet = propGetter.Invoke(source, null);

                try
                {
                    propSetter.Invoke(target, new[] { valueToSet });
                }
                catch (Exception e)
                {
                    var exm = e.Message;
                    var st = e.StackTrace;
                }
            }
        }


      public string  connectionString = "SERVER=localhost;DATABASE=ubachef;UID=root;PASSWORD=mukoni;";

      //  public ubachefEntities db = new ubachefEntities();

    }


}