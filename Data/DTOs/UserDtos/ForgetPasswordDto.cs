using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.UserDtos
{
    public class ForgetPasswordDto
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
        public string RepeatPassword { get; set; }
    }
}
