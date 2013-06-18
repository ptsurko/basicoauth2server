using System.ComponentModel.DataAnnotations;

namespace OAuth2WebClient.ViewModels.Home
{
    public class AuthorizationCodeGrantViewModel
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string ClientSecret { get; set; }

        public string Scope { get; set; }
    }
}