using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OAuth2.Messages;

namespace OAuth2Server.ViewModels.OAuth
{
    public class AccountAuthorizeModel
    {
        public string ClientIdentifier { get; set; }

        public HashSet<string> Scope { get; set; }

        public EndUserAuthorizationRequest AuthorizationRequest { get; set; }
    }
}