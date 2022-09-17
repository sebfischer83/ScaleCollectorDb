using IdentityModel;
using k8s.KubeConfigModels;
using System.Security.Principal;

namespace ScaleCollectorDbServer.Services.UserResolver
{
    public class UserResolverService
    {
        private readonly IHttpContextAccessor _context;
        public UserResolverService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetUser()
        {
            var user = _context.HttpContext.User?.Identity;

            if (user == null)
                return "";

            return user.Name.Split('|')[1];
        }
    }
}
