﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;

namespace NWebsec.Mvc.HttpHeaders.Internals
{
    public abstract class HttpHeaderAttributeBase : ActionFilterAttribute
    {
        private static readonly Object MarkerObject = new Object();
        private string _contextKey;

        private string ContextKey => _contextKey ?? (_contextKey = "NWebsecHeaderSet" + ContextKeyIdentifier);

        internal virtual string ContextKeyIdentifier => GetType().Name;

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context.Items[ContextKey] != null)
            {
                base.OnActionExecuted(filterContext);
                return;
            }

            context.Items[ContextKey] = MarkerObject;
            SetHttpHeadersOnActionExecuted(filterContext);
            base.OnActionExecuted(filterContext);
        }

        public abstract void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext);

        /// <summary>
        /// Creates an exception with message prefixed with the current type name, to give a hint about the current attribute.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected Exception CreateAttributeException(string message, Exception e = null)
        {
            var errorMessage = $"{GetType().Name}: {message}";
            if (e != null)
            {
                errorMessage += "\nDetails: " + e.Message;
            }
            return new ArgumentException(errorMessage);
        }
    }
}
