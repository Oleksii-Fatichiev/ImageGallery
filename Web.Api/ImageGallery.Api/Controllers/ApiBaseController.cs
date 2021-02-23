using ImageGallery.Api.Models.Json;
using ImageGallery.Contracts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ImageGallery.Api.Controllers
{
    public abstract class ApiBaseController
        : ControllerBase
    {
        //protected string UserId { get; }

        public ApiBaseController(UserManager<AppUser> userManager)
        {
            if (userManager is null)
                throw new ArgumentNullException(nameof(userManager));

            //UserId = userManager.GetUserId(User);
        }

        protected ObjectResult CreateInternalStatusErrorResponse(string message) =>
            StatusCode(
                StatusCodes.Status500InternalServerError,
                new JsonErrorResponse { Message = message });
    }
}
