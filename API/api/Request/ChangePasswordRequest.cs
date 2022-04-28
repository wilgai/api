using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Request
{
    public class ChangePasswordRequest
    {
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string NewPassword { get; set; }
    }

}
