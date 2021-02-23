using AutoMapper;
using ImageGallery.Api.Attributes;
using ImageGallery.Api.Helpers;
using ImageGallery.Api.Models.Json;
using ImageGallery.Api.Models.Json.Picture;
using ImageGallery.Contracts.Common.Pagination;
using ImageGallery.Contracts.Models;
using ImageGallery.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    [Route("api/[controller]")]
    [ApiVersion(ApiVertions.V_1_0)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class PictureController
        : ApiBaseController
    {
        private readonly IPictureService _pictureService;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly ILogger<PictureController> _logger;
        private readonly UserManager<AppUser> _userManager;

        public PictureController(IPictureService pictureService,
            IUriService uriService,
            IMapper mapper,
            ILogger<PictureController> logger,
            UserManager<AppUser> userManager)
            : base(userManager)
        {
            _pictureService = pictureService
                ?? throw new ArgumentNullException(nameof(pictureService));
            _uriService = uriService
                ?? throw new ArgumentNullException(nameof(uriService));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            _userManager = userManager
                ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <summary>
        /// Get pictures by gallery id.
        /// </summary>
        /// <param name="galleryId"></param>
        /// <returns></returns>
        [HttpGet]
        [GalleryAccess("galleryId")]
        [ProducesResponseType(typeof(JsonPictureResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetPicturesByGalleryIdAsync(int galleryId, [FromQuery] JsonPaginationQuery query)
        {
            var filter = query is not null
                ? _mapper.Map<PaginationFilter>(query)
                : new PaginationFilter(new PicturePaginationOptions());

            var result = await _pictureService.GetPicturesByGalleryIdAsync(galleryId, filter);
            if (!result.Succeeded)
                return CreateInternalStatusErrorResponse("Something went wrong");

            var response =
                PaginationHelper.CreatePaginationResponse(_uriService,
                    filter,
                    _mapper.Map<IEnumerable<JsonPictureResponse>>(result.Data));

            return Ok(_mapper.Map<IEnumerable<Picture>>(result.Data));
        }

        /// <summary>
        /// Create new pictures.
        /// </summary>
        /// <param name="galleryId"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreatePicturesAsync(int galleryId, [FromBody] IEnumerable<IFormFile> files)
        {
            if (files is null)
                return BadRequest(
                    new JsonErrorResponse { Message = "Request model for creating new pictures is null." });
            if (!files.Any())
                return BadRequest(
                    new JsonErrorResponse { Message = "There are no galleries for creating." });

            var result = await _pictureService.CreatePicturesAsync(files, galleryId);
            if (!result.Succeeded)
                return CreateInternalStatusErrorResponse("Something went wrong");

            return Created("", result.Succeeded);
        }

        /// <summary>
        /// Delete pictures by ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(JsonErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeletePicturesAsync(IEnumerable<int> ids)
        {
            if (ids is null)
                return BadRequest(
                    new JsonErrorResponse { Message = "Request model for deleting pictures is null or empty." });
            if (!ids.Any())
                return BadRequest(
                    new JsonErrorResponse { Message = "There are no pictures for deleting." });

            var user = await _userManager.GetUserAsync(User);

            var isUserManagesPictures = await _pictureService.CheckAccessPicturesAsync(ids, user.Id);
            if (!isUserManagesPictures)
                return BadRequest(
                    new JsonErrorResponse { Message = "Access denied." });

            var result = await _pictureService.DeletePicturesAsync(ids, user.Id);
            if (!result.Succeeded)
                return CreateInternalStatusErrorResponse("Something went wrong");

            return Ok(result.Succeeded);
        }
    }
}
