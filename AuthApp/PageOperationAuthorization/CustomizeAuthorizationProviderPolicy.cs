using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace AuthApp.PageOperationAuthorization
{
    public class CustomizeAuthorizationProviderPolicy : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;
        private readonly ConcurrentDictionary<string, AuthorizationPolicy> _policies = new ConcurrentDictionary<string, AuthorizationPolicy>();

        public CustomizeAuthorizationProviderPolicy(IOptions<AuthorizationOptions> options)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => _fallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!_policies.TryGetValue(policyName, out var policy))
            {
                var parts = policyName.Split(':');
                if (parts.Length == 3)
                {
                    policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(new PageOperationRequirement(parts[1], parts[2], parts[0]))
                        .Build();

                    _policies[policyName] = policy;
                }
                if (parts.Length == 1)
                {
                    policy = new AuthorizationPolicyBuilder()
                   .AddRequirements(new PageOperationRequirement(string.Empty, string.Empty, parts[0]))
                   .Build();
                    _policies[policyName] = policy;
                }
            }

            return Task.FromResult(policy ?? _fallbackPolicyProvider.GetPolicyAsync(policyName).Result);
        }
    }
}
