using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApi.Models
{
	public class MyRole : IdentityRole
    {
        public string Description { get; set; }
    }
}