using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OAuth2;
using OAuth2Server.ViewModels.Home;
using OAuth2WebClient.ViewModels.Home;

namespace OAuth2WebClient.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Gets the description of the authorization server to which we will be connecting. The most important component
        /// is the token endpoint, which is the URL at which the server listens for token requests.
        /// </summary>
        /// <value>
        /// The authorization server description.
        /// </value>
        private readonly AuthorizationServerDescription _authServerDescription = new AuthorizationServerDescription
                    {
                        TokenEndpoint = new Uri("http://localhost/OAuth2Server/Tokens"),
                        AuthorizationEndpoint = new Uri("http://localhost/OAuth2Server/OAuth/Authorize"),
                        ProtocolVersion = ProtocolVersion.V20
                    };

        #region ClientCredentialsGrant
        /// <summary>
        /// This action will allow the user to expirement with the OAuth 2 client credentials grant workflow. 
        /// </summary>
        /// <remarks>See: http://tools.ietf.org/html/rfc6749#section-4.4 </remarks>
        /// <returns>The view result.</returns>
        [HttpGet]
        public ViewResult ClientCredentialsGrant()
        {
            // We will set-up correct default values to make it easier for the user to start testing
            var model = new ClientCredentialsGrantViewModel { ClientId = "demo-client-credentials-identifier", ClientSecret = "demo-client-credentials-secret-key", Scope = "user" };

            return this.View(model);
        }

        /// <summary>
        /// This action will show the user the result of his OAuth 2 client credentials grant workflow request. 
        /// </summary>
        /// <remarks>See: http://tools.ietf.org/html/rfc6749#section-4.4 </remarks>
        /// <returns>The view result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult ClientCredentialsGrant(ClientCredentialsGrantViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    // Create the client with which we will be connecting to the server.
                    var webServerClient = new WebServerClient(_authServerDescription, clientIdentifier: model.ClientId, clientSecret: model.ClientSecret);

                    // The scope that we request for the client. Note: this can also be null if we don't want to request any specific 
                    // scope or more than one scope if we want to request an access token that is valid for several scopes
                    var clientScopes = OAuthUtilities.SplitScopes(model.Scope ?? string.Empty);

                    // Request a new client access token for the specified scopes (http://tools.ietf.org/html/draft-ietf-oauth-v2-31#section-4.4)
                    // This method will use the client identifier and client secret used when constructing the WebServerAgentClient instance
                    this.ViewBag.AccessToken = webServerClient.GetClientAccessToken(clientScopes);
                }
                catch (Exception ex)
                {
                    this.ViewBag.Exception = ex;
                }
            }

            return this.View(model);
        } 
        #endregion

        #region ResourceOwnerCredentialsGrant
        /// <summary>
        /// This action will allow the user to expirement with the OAuth 2 resource owner credentials grant workflow.
        /// </summary>
        /// <remarks>See: http://tools.ietf.org/html/rfc6749#section-4.3 </remarks>
        /// <returns>The view result.</returns>
        [HttpGet]
        public ViewResult ResourceOwnerCredentialsGrant()
        {
            // We will set-up correct default values to make it easier for the user to start testing
            var model = new ResourceOwnerCredentialsGrantViewModel
            {
                Username = "demo-user-username",
                Password = "demo-user-password",
                ClientId = "demo-client-res-owner-identifier",
                Scope = "user"
            };

            return this.View(model);
        }

        /// <summary>
        /// This action will show the user the result of his OAuth 2 resource owner credentials grant workflow request. 
        /// </summary>
        /// <remarks>See: http://tools.ietf.org/html/rfc6749#section-4.3 </remarks>
        /// <returns>The view result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult ResourceOwnerCredentialsGrant(ResourceOwnerCredentialsGrantViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    // Create the client with which we will be connecting to the server.
                    var webServerClient = new WebServerClient(_authServerDescription, clientIdentifier: model.ClientId);

                    // The scope that we request for the user. Note: this can also be null if we don't want to request any specific 
                    // scope or more than one scope if we want to request an access token that is valid for several scopes
                    var userScopes = OAuthUtilities.SplitScopes(model.Scope ?? string.Empty);

                    // Request a new user access token for the specified user and the specified scopes (http://tools.ietf.org/html/draft-ietf-oauth-v2-31#page-35)
                    this.ViewBag.AccessToken = webServerClient.ExchangeUserCredentialForToken(model.Username, model.Password, userScopes);
                }
                catch (Exception ex)
                {
                    this.ViewBag.Exception = ex;
                }
            }

            return this.View(model);
        } 
        #endregion

        #region AuthorizationCodeGrant
        /// <summary>
        /// This action will allow the user to expirement with the OAuth 2 authorization code grant workflow. 
        /// </summary>
        /// <remarks>See: http://tools.ietf.org/html/rfc6749#section-4.4 </remarks>
        /// <returns>The view result.</returns>
        [HttpGet]
        public ActionResult AuthorizationCodeGrant(string code, string state)
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            var model = new AuthorizationCodeGrantViewModel { ClientId = "demo-client-auth-code-identifier", ClientSecret = "demo-client-auth-code-secret-key", Scope = "user" };

            if (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(state))
            {
                var client = new WebServerClient(_authServerDescription, model.ClientId, model.ClientSecret);

                this.ViewBag.AccessToken = client.ProcessUserAuthorization(this.Request);
            }

            this.ViewBag.AuthorizationCode = code;
            this.ViewBag.AuthorizationState = state;

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuthorizationCodeGrant(AuthorizationCodeGrantViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var client = new WebServerClient(_authServerDescription, viewModel.ClientId, viewModel.ClientSecret);

                var request = client.PrepareRequestUserAuthorization(new[] { viewModel.Scope });

                request.Send();
            }

            return View(viewModel);

        } 
        #endregion
    }
}
