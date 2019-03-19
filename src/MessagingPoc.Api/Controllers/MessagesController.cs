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
    [Route("api/folders")]
    [ApiController]
    [SwaggerTag("Messages", Description = "Endpoints within the messages URL")]
    public class MessagesController : ControllerBase
    {
        const string serverErrorMessage = "An internal server error occurred when attempting to ";
        const string notFoundMessage = "The data you are trying to access was not found.";

        /// <summary>Returns all messages in the specified folder.</summary>
        [SwaggerResponse(HttpStatusCode.OK, typeof(List<MessageDto>))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(IActionResult), Description = notFoundMessage)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(IActionResult), Description = "${serverErrorMessage}get messages.")]
        [HttpGet, Route("{folderId}/messages")]
        public IActionResult GetMessages(int folderId)
        {
            var messages = MockMessages.Current.Folders
                .FirstOrDefault(f => f.id == folderId).messages;

            if (messages == null)
            {
                return NotFound();
            }

            return Ok(messages);
        }

        /// <summary>Returns specific message in the specified folder.</summary>
        [SwaggerResponse(HttpStatusCode.OK, typeof(MessageDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(IActionResult), Description = notFoundMessage)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(IActionResult), Description = "${serverErrorMessage}get message.")]
        [HttpGet, Route("{folderId}/messages/{messageId}")]
        public IActionResult GetMessage(int folderId, int messageId)
        {
            var message = MockMessages.Current.Folders
                .FirstOrDefault(f => f.id == folderId).messages
                .FirstOrDefault(m => m.id == messageId);

            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        /// <summary>Creates a new message in 'Send' folder.</summary>
        [SwaggerResponse(HttpStatusCode.OK, typeof(MessageDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(IActionResult), Description = notFoundMessage)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(IActionResult), Description = "${serverErrorMessage}create message.")]
        [HttpPost, Route("{folderId}")]
        public IActionResult CreateNewMessage([FromBody] MessageDto message)
        {
            if (message == null)
            {
                return BadRequest();
            }

            var folder = MockMessages.Current.Folders.FirstOrDefault(f => f.name == "Sent");
            message.id = folder.messages.Count() + 1;
            folder.messages.Add(message);

            return Ok(folder.messages);
        }

        /// <summary>Replaces/updates a message in the specified folder.</summary>
        [SwaggerResponse(HttpStatusCode.OK, typeof(MessageDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(IActionResult), Description = notFoundMessage)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(IActionResult), Description = "${serverErrorMessage}update message.")]
        [HttpPut, Route("{folderId}/messages/{messageId}")]
        public IActionResult PutMessage(int folderId, int messageId, [FromBody] MessageDto newMessage)
        {
            if (newMessage == null)
            {
                return BadRequest();
            }

            var folder = MockMessages.Current.Folders.FirstOrDefault(f => f.id == folderId);
            var oldMessage = folder.messages.FirstOrDefault(m => m.id == messageId);
            newMessage.id = oldMessage.id;

            folder.messages.Remove(oldMessage);
            folder.messages.Add(newMessage);

            return Ok(newMessage);
        }

        /// <summary>Deletes a message in the specified folder.</summary>
        [SwaggerResponse(HttpStatusCode.OK, typeof(MessageDto))]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(IActionResult), Description = notFoundMessage)]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(IActionResult), Description = "${serverErrorMessage}delete message.")]
        [HttpDelete, Route("{folderId}/messages/{messageId}")]
        public IActionResult DeleteFolder(int folderId, int messageId)
        {
            var folder = MockMessages.Current.Folders.FirstOrDefault(f => f.id == folderId);
            var message = folder.messages.FirstOrDefault(m => m.id == messageId);
            folder.messages.Remove(message);
            return Ok(folder.messages);
        }
    }
}