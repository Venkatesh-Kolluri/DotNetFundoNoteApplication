using BusinessLayer.Interface;
using CommonLayer.Model;
using DataLayer.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Linq;
using System;
using Microsoft.Exchange.WebServices.Data;
using BussinesLayer.Services;
using DataLayer.Services;
using BusinessLayer.Services;
using DataLayer.Interface;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace FundoNoteApplication.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        INotesBL notesBL;
        private readonly ILogger<NotesController> logger;
        private readonly FundoContext context;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;


        public NotesController(INotesBL notesBL, FundoContext context, IMemoryCache memoryCache, IDistributedCache distributedCache, ILogger<NotesController> logger)
        {
            this.notesBL = notesBL;
            this.context = context;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.logger = logger;
        }

        [HttpPost]
        [Route("addnotes")]
        public IActionResult AddNotes(NotesModel notesModel)
        {
            try
            {

                var userId = notesModel.UserId;
                var check = notesBL.CheckUserId(userId);
                if (check != true)
                {
                    logger.LogInformation("User not available");
                    return this.BadRequest(new { sucess = false, msg = "User not available" });
                }
                var result = notesBL.AddNote(notesModel);
                if (result != null)
                {
                    logger.LogInformation("Notes Added sucessfully");
                    return this.Ok(new { success = true, msg = "Notes Added sucessfully", data = result });
                }
                else
                {
                    logger.LogInformation("Unsuccessfull in adding notes");
                    return this.BadRequest(new { success = false, msg = "Unsuccessfull in adding notes" });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

        [HttpDelete]
        [Route("deletenotes")]
        public IActionResult DeleteNotes(long NoteId)
        {
            try
            {

                var delete = notesBL.DeleteNote(NoteId);
                if (delete != null)
                {
                    logger.LogInformation("Notes Deleted Successfully");
                    return this.Ok(new { Success = true, message = "Notes Deleted Successfully" });
                }
                else
                {
                    logger.LogInformation("Unable to Delete notes");
                    return this.BadRequest(new { Success = false, message = "Unable to Delete notes" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpPut]
        [Route("updatenote")]
        public IActionResult UpdateNote(NotesModel notesModel, long NoteId)
        {
            try
            {
                var userId = notesModel.UserId;
                var check = notesBL.CheckUserId(userId);
                if (check != true)
                {
                    logger.LogInformation("UserId not available");
                    return this.BadRequest(new { sucess = false, msg = "UserId not available" });
                }
                var result = notesBL.UpdateNote(notesModel, NoteId);
                if (result != null)
                {
                    logger.LogInformation("Notes Updated Successfully");
                    return this.Ok(new { Success = true, message = "Notes Updated Successfully", data = result });
                }
                else
                {
                    logger.LogInformation("No Notes Found");
                    return this.BadRequest(new { Success = false, message = "No Notes Found" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut]
        [Route("ispinned")]
        public IActionResult IsPinned(long noteId)
        {
            try
            {
                var result = notesBL.Pinned(noteId);
                if (result != null)
                {
                    logger.LogInformation("pin successfull");
                    return this.Ok(new { Success = true, message = "pin successfull", data = result });
                }
                else
                {
                    logger.LogInformation("Unable to Pin");
                    return this.BadRequest(new { Success = false, message = "Unable to Pin" });
                }


            }

            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut]
        [Route("archived")]
        public IActionResult Archived(long noteId)
        {
            try
            {
                var result = notesBL.Archieved(noteId);
                if (result != null)
                {
                    logger.LogInformation("archived successfull");
                    return this.Ok(new { Success = true, message = "archived successfull", data = result });
                }
                else
                {
                    logger.LogInformation("Unable to Archive");
                    return this.BadRequest(new { Success = false, message = "Unable to Archive" });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut]
        [Route("trash")]
        public IActionResult Trash(long noteId)
        {
            try
            {
                var result = notesBL.Trashed(noteId);
                if (result != null)
                {
                    logger.LogInformation("moved to trash successfully");
                    return this.Ok(new { Success = true, message = "moved to trash successfully", data = result });
                }
                else
                {
                    logger.LogInformation("Unable to execute Trash");
                    return this.BadRequest(new { Success = false, message = "Unable to execute Trash" });
                }


            }

            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("getnote")]
        public IActionResult GetNote(long NoteId)
        {
            try
            {
                List<NotesEntity> result = notesBL.GetNote(NoteId);
                if (result != null)
                {
                    logger.LogInformation("got note Successfully");
                    return this.Ok(new { Success = true, message = "got note Successfully", data = result });
                }
                else
                {
                    logger.LogInformation("Unsuccessfull in adding notes");
                    return this.BadRequest(new { Success = false, message = "Note not Available" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("getnotebyuserid")]
        public IActionResult GetNoteByUserID(long userId)
        {
            try
            {
                //  var userId = notesModel.UserId;
                var check = notesBL.CheckUserId(userId);
                if (check != true)
                {
                    logger.LogInformation("UserId not Created");
                    return this.BadRequest(new { sucess = false, msg = "UserId not Created" });
                }
                List<NotesEntity> result = notesBL.GetNotebyUserId(userId);
                if (result != null)
                {
                    logger.LogInformation("Note Available");
                    return this.Ok(new { Success = false, message = "Note Available" });
                }
                else
                {                   
                     logger.LogInformation("Note not Available");
                    return this.BadRequest(new { Success = false, message = " Not not available " });

                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("getallnote")]
        public IActionResult GetAllNote()
        {
            try
            {
                List<NotesEntity> result = notesBL.GetAllNote();
                if (result != null)
                {
                    logger.LogInformation("got notes Successfully");
                    return this.Ok(new { Success = true, message = "got notes Successfully", data = result });
                }
                else
                {
                    logger.LogInformation("Notes not Available");
                    return this.BadRequest(new { Success = false, message = "Notes not Available" });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut]
        [Route("Image")]
        public IActionResult Image(long noteId, IFormFile image)
        {
            try
            {

                var result = notesBL.Image(noteId, image);
                if (result != null)
                {
                    logger.LogInformation("Image Uploaded Successfully");
                    return Ok(new { Status = true, Message = "Image Uploaded Successfully", Data = result });
                }
                else
                {
                    logger.LogInformation("Image Uploaded Unsuccessfully");
                    return BadRequest(new { Status = true, Message = "Image Uploaded Unsuccessfully", Data = result });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("FindNotes")]
        public IActionResult FindNotes(string note)
        {
            try
            {
                IQueryable<NotesEntity> result = notesBL.Find(note);
                if (result != null)
                {
                    logger.LogInformation("note found Successfully");
                    return Ok(new { Status = true, Message = "note found Successfully", Data = result });
                }
                else
                {
                    logger.LogInformation("notes are unavailable ");
                    return BadRequest(new { Status = true, Message = "notes are unavailable ", Data = result });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            try
            {
                logger.LogInformation("Using Rdis Cache getting all available notes ");
                var cacheKey = "NotesList";
                string serializedNotesList;
                var notesList = new List<NotesEntity>();
                var redisNotesList = await distributedCache.GetAsync(cacheKey);
                if (redisNotesList != null)
                {
                    serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                    notesList = JsonConvert.DeserializeObject<List<NotesEntity>>(serializedNotesList);
                }
                else
                {
                    notesList = await context.NotesTable.ToListAsync();
                    serializedNotesList = JsonConvert.SerializeObject(notesList);
                    redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey, redisNotesList, options);
                }
                return Ok(notesList);

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }

        }
    }
}
