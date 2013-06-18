using System;
using System.Net;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.OpenId.Provider;
using OAuth2Server.App_Start;
using OAuth2Server.Infrastructure;

namespace OAuth2Server
{
    using System.Web;
    using System.Web.Routing;
    
    public class MvcApplication : HttpApplication
    {
        public static AuthorizationServer AuthorizationServer;

        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // DotNetOpenAuth only issues access tokens when the client uses an HTTPS connection. As we will most
            // likely run the server on our local development machine with only a self-signed SSL certificate, setting up 
            // connection to the server will fail as the SSL certificate is considered invalid by the .NET framework. 
            // To circumvent this, we add the line below that will consider all SSL certificates as valid, including
            // self-signed certificaties. Note: this should only be used for testing purposes.
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            // Use a default in-memory provider application store. This is a class that is ideal for use in test
            // applications as it requires no further setup and can be used as both the crypto key- and nonce store.
            // In real-life situations you would of course implement your own crypto key- and nonce store, which will
            // most likely use some kind of persistent storage to store keys and nonces. As the nonces are kept in memory
            // only, it is not possible to refresh tokens as the issued tokens will have been removed from memory the moment
            // the refresh token request is being processed
            var standardProviderApplicationStore = new StandardProviderApplicationStore();
            var clientRepository = new InMemoryClientRepository();
            clientRepository.Save(new Client()
                {
                    Id = "demo-client-auth-code-identifier",
                    SecretKey = "demo-client-auth-code-secret-key",
                    CallbackUrl = new Uri("http://localhost/OAuth2WebClient/")
                });
            clientRepository.Save(new Client()
            {
                Id = "demo-client-credentials-identifier",
                SecretKey = "demo-client-credentials-secret-key"
            });
            clientRepository.Save(new Client()
            {
                Id = "demo-client-res-owner-identifier"
            });


            var userRepository = new InMemoryUserRepository();
            userRepository.Save(new User
                {
                    Name = "demo-user-username",
                    Password = "demo-user-password"
                });
            
            
            var authorizationServerHost = new AuthorizationServerHost(standardProviderApplicationStore, standardProviderApplicationStore, clientRepository, userRepository);

            // Create our authorization server using our own, custom IAuthorizationServerHost implementation
            AuthorizationServer = new AuthorizationServer(authorizationServerHost);
        
        }
    }


}