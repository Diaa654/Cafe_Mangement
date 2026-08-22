using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS
{
    public class SaveDeviceTokenDto
    {
        public int UserId { get; set; }
        public string DeviceToken { get; set; } = string.Empty;
    }
}
