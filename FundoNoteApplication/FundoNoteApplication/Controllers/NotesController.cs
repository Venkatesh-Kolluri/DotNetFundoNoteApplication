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

namespace FundoNoteApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
         INotesBL notesBL;
        private readonly FundoContext context;
        public NotesController(INotesBL notesBL,FundoContext context)
        {
            this.notesBL = notesBL;
            this.context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(AddNotes))]
        public IActionResult AddNotes(NotesModel notesModel)
        {
            try
            {
                //  UserEntity userEntity = new UserEntity();
                // long userId = userEntity.UserId;
                //        long userId= notesModel.UserId;
                //long userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);

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

        [AllowAnonymous]
        [HttpDelete]
        [Route(nameof(DeleteNotes))]
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
  

        [AllowAnonymous]
        [HttpPut]
        [Route(nameof(UpdateNote))]
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

        [AllowAnonymous]
        [HttpPut]
        [Route(nameof(IsPinned))]
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
        [AllowAnonymous]
        [HttpPut]
        [Route(nameof(Archived))]
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
        [AllowAnonymous]
        [HttpPut]
        [Route(nameof(Trash))]
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
    
    [AllowAnonymous]
    [HttpGet]
    [Route(nameof(GetNote))]
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

        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(GetNoteByUserID))]
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

        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(GetAllNote))]
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
        [AllowAnonymous]
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
    }

}
