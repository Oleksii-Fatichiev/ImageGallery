using ImageGallery.Contracts.Models;
using System;

namespace ImageGallery.Api.Factories
{
    internal static class UserFactory
    {
        internal static AppUser CreateUser(string userName)
        {
            return new AppUser
            {
                UserName = userName,
                SecurityStamp = new Guid().ToString()
            };
        }
    }
}
