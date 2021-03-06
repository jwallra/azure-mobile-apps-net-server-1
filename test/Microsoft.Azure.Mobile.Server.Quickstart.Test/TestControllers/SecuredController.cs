﻿// ---------------------------------------------------------------------------- 
// Copyright (c) Microsoft Corporation. All rights reserved.
// ---------------------------------------------------------------------------- 

using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.Mobile.Server.TestControllers
{
    [Authorize]
    public class SecuredController : ApiController
    {
        [AllowAnonymous]
        [Route("api/secured/anonymous")]
        public IHttpActionResult GetAnonymous()
        {
            return this.GetUserDetails();
        }

        [Route("api/secured/authorize")]
        public string GetApplication()
        {
            return this.Request.Headers.GetValues("x-zumo-auth").FirstOrDefault<string>();
        }

        private IHttpActionResult GetUserDetails()
        {
            ClaimsPrincipal user = this.User as ClaimsPrincipal;
            JObject details = null;
            if (user != null)
            {
                Claim userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                string userId = null;
                if (userIdClaim != null)
                {
                    userId = userIdClaim.Value;
                }
                details = new JObject
                {
                    { "id", userId }               
                };
            }

            return this.Json(details);
        }
    }
}
