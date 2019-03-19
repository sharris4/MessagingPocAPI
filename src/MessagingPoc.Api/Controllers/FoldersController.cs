using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MessagingPocApi.Models;
using NSwag.Annotations;
using System.Net;

namespace MessagingPoc.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Folders", Description = "Endpoints within the api/folders URL")]
    public class FoldersController : ControllerBase
    {
        const string serverErrorMessage = "An internal server error occurred when attempting to ";
        const string notFoundMessage = "The data you are trying to access was not found.";

        /// <summary>Returns all folders available.</summary>
        [SwaggerResponse(HttpStatusCode.OK, typeof(List<FolderDto>))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(IActionResult), Description = notFoundMessage)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(IActionResult), Description = "${serverErrorMessage}get folders.")]
        [HttpGet]
        public IActionResult GetMessageFolders()
        {
            return Ok(MockMessages.Current.Folders);
        }

        /// <summary>Returns a specific folder and it's content.</summary>
        [SwaggerResponse(HttpStatusCode.OK, typeof(FolderDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(IActionResult), Description = notFoundMessage)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(IActionResult), Description = "${serverErrorMessage}get folder.")]
        [HttpGet, Route("{id}")]
        public IActionResult GetMessageFolder(int id)
        {
            var folder = MockMessages.Current.Folders.FirstOrDefault(m => m.id == id);
            if (folder == null)
            {
                return NotFound();
            }

            return Ok(folder);
        }

        /// <summary>Creates a new folder.</summary>
        [SwaggerResponse(HttpStatusCode.OK, typeof(FolderDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(IActionResult), Description = notFoundMessage)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(IActionResult), Description = "${serverErrorMessage}create folder.")]
        [HttpPost]
        public IActionResult CreateNewFolder([FromBody] FolderDto newFolder)
        {
            if (newFolder == null)
            {
                return BadRequest();
            }

            newFolder.id = MockMessages.Current.Folders.Count + 1;
            MockMessages.Current.Folders.Add(newFolder);

            return Ok(newFolder);
        }

        /// <summary>Replaces/updates the specified folder.</summary>
        [SwaggerResponse(HttpStatusCode.OK, typeof(FolderDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(IActionResult), Description = notFoundMessage)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(IActionResult), Description = "${serverErrorMessage}update folder.")]
        [HttpPut, Route("{folderId}")]
        public IActionResult PutFolder(int folderId, [FromBody] FolderDto newFolder)
        {
            if (newFolder == null)
            {
                return BadRequest();
            }

            var oldFolder = MockMessages.Current.Folders.FirstOrDefault(f => f.id == folderId);
            MockMessages.Current.Folders.Remove(oldFolder);
            MockMessages.Current.Folders.Add(newFolder);

            return Ok(newFolder);
        }

        /// <summary>Deletes the specified folder.</summary>
        [SwaggerResponse(HttpStatusCode.OK, typeof(FolderDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(IActionResult), Description = notFoundMessage)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(IActionResult), Description = "${serverErrorMessage}delete folder.")]
        [HttpDelete, Route("{id}")]
        public IActionResult DeleteFolder(int id)
        {
            var folder = MockMessages.Current.Folders.FirstOrDefault(f => f.id == id);
            MockMessages.Current.Folders.Remove(folder);
            return Ok(folder);
        }
    }
}
