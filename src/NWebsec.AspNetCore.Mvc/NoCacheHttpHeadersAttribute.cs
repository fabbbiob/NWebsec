﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.Filters;
using NWebsec.AspNetCore.Core.HttpHeaders.Configuration;
using NWebsec.AspNetCore.Mvc.Helpers;
using NWebsec.AspNetCore.Mvc.Internals;

namespace NWebsec.AspNetCore.Mvc
{
    /// <summary>
    /// Specifies whether appropriate headers to prevent browser caching should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoCacheHttpHeadersAttribute : HttpHeaderAttributeBase
    {
        private readonly SimpleBooleanConfiguration _config;
        private readonly HeaderConfigurationOverrideHelper _configurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoCacheHttpHeadersAttribute"/> class
        /// </summary>
        public NoCacheHttpHeadersAttribute()
        {
            _config = new SimpleBooleanConfiguration { Enabled = true };
            _configurationOverrideHelper = new HeaderConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper();
        }
        
        /// <summary>
        /// Gets of sets whether cache headers should be included in the response to prevent browser caching. The default is true.
        /// </summary>
        public bool Enabled { get { return _config.Enabled; } set { _config.Enabled = value; } }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _configurationOverrideHelper.SetNoCacheHeadersOverride(filterContext.HttpContext, _config);
            base.OnActionExecuting(filterContext);
        }

        public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            _headerOverrideHelper.SetNoCacheHeaders(filterContext.HttpContext);
        }
    }
}
