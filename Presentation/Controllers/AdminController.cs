using Microsoft.AspNetCore.Authorization;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize(Roles = nameof(AppRoles.Admin))]
    public class AdminController:ApiBaseController
    {

    }
}
