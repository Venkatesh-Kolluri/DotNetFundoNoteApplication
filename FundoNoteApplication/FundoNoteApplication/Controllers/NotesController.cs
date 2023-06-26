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

namespace FundoNoteApplication.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
         INotesBL notesBL;
        private readonly FundoContext context;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
      

        public NotesController(INotesBL notesBL,FundoContext context,IMemoryCache  memoryCache, IDistributedCache distributedCache)
        {
            this.notesBL = notesBL;
            this.context = context;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
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
                    return this.BadRequest(new { sucess = false, msg = "Not Created" });
                }
                var result = notesBL.AddNote(notesModel);
                if (result != null)
                {
                    return this.Ok(new { success = true, msg = "Notes Added sucessfully", data = result }); 
                }
                else
                {
                    return this.BadRequest(new { success = false, msg = "Unsuccessfull in adding notes" });
                }

            }
            catch (System.Exception)
            {

                 throw;
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
                    return this.Ok(new { Success = true, message = "Notes Deleted Successfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to Delete notes" });
                }
            }
            catch (Exception ex)
            {
             
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
                    return this.BadRequest(new { sucess = false, msg = "Not Created" });
                }
                var result = notesBL.UpdateNote(notesModel, NoteId);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Notes Updated Successfully", data = result });
                }
                else
                {
                   
                    return this.BadRequest(new { Success = false, message = "No Notes Found" });
                }
            }
            catch (Exception ex)
            {
          
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
                    return this.Ok(new { Success = true, message = "successfull", data = result });
                }
                else

                return this.BadRequest(new { Success = false, message = "Unable to execute Pin" });
            }

            catch (Exception ex)
            {
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
                    return this.Ok(new { Success = true, message = "excuted successfully", data = result });
                }
                else
                    return this.BadRequest(new { Success = false, message = "Unable to execute Archived" });
            }
            catch (Exception ex)
            {
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
                    return this.Ok(new { Success = true, message = "moved to trash successfully", data = result });
                }
                else
                    return this.BadRequest(new { Success = false, message = "Unable to execute Trash" });
            
            }

            catch (Exception ex)
            {
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
                        return this.Ok(new { Success = true, message = " Note got Successfully", data = result });
                    }
                    else
                        return this.BadRequest(new { Success = false, message = "Note not Available" });
                }
                catch (Exception ex)
                {
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
                      return this.BadRequest(new { sucess = false, msg = "Not Created" });
                  }
                  List<NotesEntity> result = notesBL.GetNotebyUserId(userId);
                  if (result == null)
                  {
                      return this.Ok(new { Success = false, message = " Note not Available" });
                  }
                  else
                  {
                      if (result != null)
                      {
                          return this.Ok(new { Success = true, message = " Got note Successfully", data = result });
                      }
                      return this.BadRequest(new { Success = false, message = " error occured" });

                  }
              }
              catch (Exception ex)
              {
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
                      return this.Ok(new { Success = true, message = " Note got Successfully", data = result });
                  }
                  else
                      return this.BadRequest(new { Success = false, message = "Note not Available" });
              }
              catch (Exception ex)
              {
                  return BadRequest(new { success = false, message = ex.Message });
              }
          }
    
        [HttpPut]
        [Route("Image")]
        public IActionResult Image(long noteId, IFormFile image)
        {
            try
            {
                //long userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);

              /*  var userId = userId;
                var check = notesBL.CheckUserId(userId);
                if (check != true)
                {
                    return this.BadRequest(new { sucess = false, msg = "Not Created" });
                }*/
                var result = notesBL.Image(noteId, image);
                if (result != null)
                {
                  
                    return Ok(new { Status = true, Message = "Image Uploaded Successfully", Data = result });
                }
                else
                {
                    
                    return BadRequest(new { Status = true, Message = "Image Uploaded Unsuccessfully", Data = result });
                }
            }
            catch (Exception)
            {
               
                throw;
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

                    return Ok(new { Status = true, Message = "note found Successfully", Data = result });
                }
                else
                {

                    return BadRequest(new { Status = true, Message = "notes are unavailable ", Data = result });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
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
    }
}
