using AutoMapper;
using ImageGallery.Api.Attributes;
using ImageGallery.Api.Helpers;
using ImageGallery.Api.Models.Json;
using ImageGallery.Api.Models.Json.Gallery;
using ImageGallery.Contracts.Common;
using ImageGallery.Contracts.Common.Pagination;
using ImageGallery.Contracts.Models;
using ImageGallery.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using static ImageGallery.Contracts.Constants.Constants;

namespace ImageGallery.Api.Controllers.V1
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    [ApiVersion(ApiVertions.V_1_0)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class GalleryController
        : ApiBaseController
    {
        private readonly IGalleryService _galleryService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUriService _uriService;

        public GalleryController(IGalleryService galleryService,
            IMapper mapper,
            UserManager<AppUser> userManager,
            IUriService uriService)
            : base(userManager)
        {
            _galleryService = galleryService
                ?? throw new ArgumentNullException(nameof(galleryService));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
            _uriService = uriService
                ?? throw new ArgumentNullException(nameof(uriService));
        }

        /// <summary>
        /// Get galleries for user.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(JsonPagedResponse<JsonGalleryResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAllGalleriesForUserAsync([FromQuery] JsonPaginationQuery query)
        {
            var filter = query is not null
                ? _mapper.Map<PaginationFilter>(query)
                : new PaginationFilter(new GalleryPaginationOptions());

            var userId = _userManager.GetUserId(User);

            var result = await _galleryService.GetGalleriesByUserIdAsync(userId, filter);
            if (!result.Succeeded)
                return CreateInternalStatusErrorResponse("Something went wrong");

            var response =
                PaginationHelper.CreatePaginationResponse(_uriService,
                    filter,
                    _mapper.Map<IEnumerable<JsonGalleryResponse>>(result.Data));

            return Ok(response);
        }

        /// <summary>
        /// Get gallery by id.
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        [GalleryAccess("id")]
        [ProducesResponseType(typeof(JsonGalleryResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetGalleryByIdAsync(int id)
        {
            var result = await _galleryService.GetGalleryByIdAsync(id);
            if (!result.Succeeded)
                return CreateInternalStatusErrorResponse("Something went wrong");

            return Ok(result.Data);
        }

        /// <summary>
        /// Add a new gallery.
        /// </summary>
        /// <param name="gallery"></param>
        [HttpPost]
        [ProducesResponseType(typeof(JsonGalleryResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateGalleryAsync([FromBody] JsonGalleryRequest gallery)
        {
            if (gallery is null)
                return BadRequest(new JsonErrorResponse { Message = "Request model for creating a new gallery is null." });

            var newGallery = _mapper.Map<Gallery>(gallery);
            var user = await _userManager.GetUserAsync(User);

            var result = await _galleryService.CreateGalleryAsync(user, newGallery);
            if (!result.Succeeded)
                return CreateInternalStatusErrorResponse("Something went wrong");

            return Created("", _mapper.Map<JsonGalleryResponse>(result.Data));
        }

        /// <summary>
        /// Update an existing gallery. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gallery"></param>
        [HttpPut("{id}")]
        [GalleryAccess]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateGalleryAsync(int id, [FromBody] JsonGalleryRequest gallery)
        {
            if (gallery is null)
                return BadRequest(
                    new JsonErrorResponse { Message = "Request model for updating an existing gallery is null." });

            var updatedGallery = _mapper.Map<Gallery>(gallery);

            var result = await _galleryService.UpdateGalleryAsync(id, updatedGallery);
            if (!result.Succeeded)
                return CreateInternalStatusErrorResponse("Something went wrong");

            return Ok(result.Succeeded);
        }

        /// <summary>
        /// Delete galleries by ids.
        /// </summary>
        /// <param name="ids"></param>
        [HttpDelete]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteGalleriesAsync([FromBody] IEnumerable<int> ids)
        {
            if (ids is null)
                return BadRequest(
                    new JsonErrorResponse { Message = "Request model for updating an existing gallery is null." });
            if (!ids.Any())
                return BadRequest(
                    new JsonErrorResponse { Message = "There are no galleries for deleting." });

            var userId = _userManager.GetUserId(User);

            var isUserManagesGalleries = await _galleryService.CheckAccessGalleriesAsync(ids, userId);
            if (!isUserManagesGalleries)
                return BadRequest(
                    new JsonErrorResponse { Message = "Access denied." });

            var result = await _galleryService.DeleteGalleriesAsync(ids, userId);
            if (!result.Succeeded)
                return CreateInternalStatusErrorResponse("Something went wrong");

            return Ok(result.Succeeded);
        }

        /// <summary>
        /// Delete gallery by id.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [GalleryAccess]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteGalleryAsync(int id)
        {
            var userId = _userManager.GetUserId(User);

            var result = await _galleryService.DeleteGalleryAsync(id, userId);
            if (!result.Succeeded)
                return CreateInternalStatusErrorResponse("Something went wrong");

            return Ok(result.Succeeded);
        }
    }
}
