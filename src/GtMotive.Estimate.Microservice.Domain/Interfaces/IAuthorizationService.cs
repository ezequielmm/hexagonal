using System.Security.Claims;
using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Checks policy based permissions for a user.
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Checks if a user meets a specific authorization policy.
        /// </summary>
        /// <param name="user">The user to check the policy against.</param>
        /// <param name="resource">An optional resource the policy should be checked with. If a resource is not required for policy evaluation you may pass null as the value.</param>
        /// <param name="policyName">The name of the policy to check against a specific context.</param>
        /// <returns>Indicates whether the user, and optional resource has fulfilled the policy. True when the policy has been fulfilled otherwise false.</returns>
        Task<bool> Authorize(ClaimsPrincipal user, object resource, string policyName);
    }
}
