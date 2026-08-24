using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS
{
    public class GetAllUserDTO
    {
        public string FuName { get; set; } = default!;

        public string Email { get; set; }= default!;

        public bool IsActive { get; set; }
    }
}
