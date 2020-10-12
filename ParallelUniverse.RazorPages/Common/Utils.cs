using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ParallelUniverse.RazorPages
{
    public static class Utils
    {
        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var uid = claimsPrincipal.FindFirst(ClaimTypes.Sid).Value;
            return int.Parse(uid);
        }
    }
}
