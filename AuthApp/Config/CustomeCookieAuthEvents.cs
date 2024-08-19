
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthApp.Config
{
    public class CustomeCookieAuthEvents:CookieAuthenticationEvents
    {
        public override async Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            var request= context.Request;
            var returnUrl = request.Path.HasValue ? request.Path.Value : "/";
            string loginPath = "/Admin/Auth/Login";
            if (request.Path.Value.Contains("/Doctor"))
            {
                 loginPath = "/Doctor/Auth/Login";
            }else if(request.Path.Value.Contains("/Patient"))
            {
                loginPath = "/Patient/Auth/Login";
            }
            context.RedirectUri=loginPath+"?ReturnUrl="+Uri.EscapeUriString(returnUrl); 
            await base.RedirectToLogin(context);    
        } 
        
        public override async Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            var request=context.Request;
            string accessDeniedPath = "/Admin/Auth/AccessDenied";
            if (request.Path.Value.Contains("/Doctor"))
            {
                accessDeniedPath = "/Doctor/Auth/AccessDenied";
            }
            else if (request.Path.Value.Contains("/Patient"))
            {
                accessDeniedPath = "/Patient/Auth/AccessDenied";
            }
            context.RedirectUri = accessDeniedPath;
            await base.RedirectToAccessDenied(context); 
        }
    }
}
