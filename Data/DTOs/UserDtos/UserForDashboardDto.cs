using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.UserDtos
{
    public class UserForDashboardDto
    {
        public int VerifiedUsers { get; set; }
        public int UnverifiedUsers { get; set; }
        public int TotalUsers { get; set; }
    }
}
