using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ImageGallery.Contracts.Models
{
    public class AppUser
        : IdentityUser
    {
        public virtual ICollection<Gallery> Galleries { get; set; }

        public AppUser() => Galleries = new HashSet<Gallery>();
    }
}
