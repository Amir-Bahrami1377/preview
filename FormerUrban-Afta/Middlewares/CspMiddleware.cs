using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace FormerUrban_Afta.Middlewares
{
    public class CspMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _baseCspPolicy;

        public CspMiddleware(RequestDelegate next, string cspPolicy)
        {
            _next = next;
            _baseCspPolicy = cspPolicy;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Response.Headers.ContainsKey("Content-Security-Policy"))
            {
                // Inject nonce into the existing CSP script-src directive
                //string cspPolicy = AddNonceToScriptSrc(_baseCspPolicy, nonce);

                // Generate a secure nonce
                string nonce = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
                string cspPolicy = _baseCspPolicy;

                cspPolicy = AddNonceToDirective(cspPolicy, "script-src", nonce);
                cspPolicy = AddNonceToDirective(cspPolicy, "style-src", nonce);


                // Set CSP header
                context.Response.Headers.Add("Content-Security-Policy", cspPolicy);

                // Store nonce in HttpContext.Items for use in Razor views
                context.Items["CSP-Nonce"] = nonce;
            }

            await _next(context);
        }

        private string AddNonceToDirective(string policy, string directive, string nonce)
        {
            var pattern = $@"{directive}([^;]*)";
            var match = Regex.Match(policy, pattern);

            if (match.Success)
            {
                var updatedDirective = match.Groups[1].Value.Trim();

                // Remove unsafe-inline if present
                updatedDirective = Regex.Replace(updatedDirective, @"'unsafe-inline'", string.Empty).Trim();

                // Avoid adding multiple nonces
                if (!updatedDirective.Contains("'nonce-"))
                {
                    updatedDirective += $" 'nonce-{nonce}'";
                }

                return Regex.Replace(policy, pattern, $"{directive} {updatedDirective}");
            }
            else
            {
                // If the directive doesn't exist, add it
                return $"{policy}; {directive} 'self' 'nonce-{nonce}'";
            }
        }
    }
}
