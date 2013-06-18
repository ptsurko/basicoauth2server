using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.Messaging.Bindings;

namespace OAuth2Server.Infrastructure
{
    public class InMemoryNonceStore : INonceStore
    {
        public bool StoreNonce(string context, string nonce, DateTime timestampUtc)
        {
            throw new NotImplementedException();
        }
    }
}