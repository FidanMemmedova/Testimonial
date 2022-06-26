using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Areas.AdminPanel.Models
{
    [Area("AdminPanel")]
    public class Admin:IdentityUser
    {
    }
}
