using System.Net;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using OAuth2Server.ViewModels.OAuth;

namespace OAuth2Server.Controllers
{
    public class OAuthController : Controller
    {
        //private readonly AuthorizationServer authorizationServer = new AuthorizationServer(new AuthorizationServerHost());
        //
        // GET: /OAuth/
        [HttpGet]
        public ActionResult Authorize()
        {
            var pendingRequest = MvcApplication.AuthorizationServer.ReadAuthorizationRequest(Request);
			if (pendingRequest == null) {
				throw new HttpException((int)HttpStatusCode.BadRequest, "Missing authorization request.");
			}

            var model = new AccountAuthorizeModel
            {
                ClientIdentifier = pendingRequest.ClientIdentifier,
                Scope = pendingRequest.Scope,
                AuthorizationRequest = pendingRequest
            };
            this.ViewData["request"] = this.Request.Url;
            return View(model);
        }

        [HttpPost]
        public ActionResult Authorize(AccountAuthorizeModel viewModel, bool approval)
        {
            var authRequest = MvcApplication.AuthorizationServer.ReadAuthorizationRequest(Request);

            IProtocolMessage responseMessage;
            if (approval)
            {
                var grantedResponse = MvcApplication.AuthorizationServer.PrepareApproveAuthorizationRequest(
                    authRequest, "123", authRequest.Scope);
                responseMessage = grantedResponse;
            }
            else
            {
                var rejectionResponse = MvcApplication.AuthorizationServer.PrepareRejectAuthorizationRequest(authRequest);
                rejectionResponse.Error = "error 1234";
                responseMessage = rejectionResponse;
            }

            var response = MvcApplication.AuthorizationServer.Channel.PrepareResponse(responseMessage);
            return response.AsActionResult();
			
        }
    }
}
