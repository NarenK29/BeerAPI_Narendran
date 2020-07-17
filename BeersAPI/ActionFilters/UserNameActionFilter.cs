using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace BeersAPI
{
    public class UserNameActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {            
            try
            {
                if (!actionContext.ModelState.IsValid)
                {
                    actionContext.Response = actionContext.Request
                        .CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
                }
            }
            catch (Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Beers_API";
                    eventLog.WriteEntry("Error Action Filter PreProcess - UserName Validation: " + ex.ToString(), EventLogEntryType.Error, 101, 1);
                    //preProcessMessage = ex.ToString();
                }
            }
            
        }
       
    }
}