using GoWeb.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GoWeb.Shared.Models;

namespace GoWeb.Filters.Authorization
{
    public class OrginizerOrAdminRequirement : IAuthorizationRequirement
    {
    }

    public class CheckAdminHandler : AuthorizationHandler<OrginizerOrAdminRequirement, EventSummaryDTO>

    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrginizerOrAdminRequirement requirement, EventSummaryDTO resource)
        {
            if (context.User.IsInRole("Администратор"))
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

    public class CheckOrganizerHandler : AuthorizationHandler<OrginizerOrAdminRequirement, EventSummaryDTO>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrginizerOrAdminRequirement requirement, EventSummaryDTO resource)
        {
            if (context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value == resource.OrganizerId)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

}
