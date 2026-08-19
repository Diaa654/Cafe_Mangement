using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS
{
    public class UserDto
    {

        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public List<string> Roles { get; set; } = default!;

    }
}
