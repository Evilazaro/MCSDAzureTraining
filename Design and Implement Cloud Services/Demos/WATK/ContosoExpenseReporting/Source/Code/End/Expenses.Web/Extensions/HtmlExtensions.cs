// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace Expenses.Web.Extensions
{
    using System;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class HtmlExtensions
    {
        public static MvcHtmlString MenuActionLink(
            this HtmlHelper htmlHelper,
            string linkText,
            string action,
            string controller,
            object routeValues = null)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            var classValues = linkText.ToLowerInvariant().Replace(" ", string.Empty);

            if (action.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase) &&
                controller.Equals(currentController, StringComparison.InvariantCultureIgnoreCase))
            {
                var request = htmlHelper.ViewContext.RequestContext.HttpContext.Request;
                if (request["status"] == null && routeValues == null)
                {
                    classValues += " selected";
                }
                else if (request["status"] != null && routeValues != null)
                {
                    Type t = routeValues.GetType();
                    PropertyInfo p = t.GetProperty("status");
                    var status = (string)p.GetValue(routeValues, null);

                    if (request["status"].Equals(status, StringComparison.InvariantCultureIgnoreCase))
                    {
                        classValues += " selected";
                    }
                }
            }

            return htmlHelper.ActionLink(linkText, action, controller, routeValues, new { @class = classValues });
        }
    }
}