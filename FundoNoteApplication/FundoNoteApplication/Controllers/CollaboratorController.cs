using BusinessLayer.Interface;
using BusinessLayer.Services;
using CommonLayer.Model;
using DataLayer.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FundoNoteApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        ICollaboratorBL collaboratorBL;
        private readonly FundoContext context;

        public CollaboratorController(ICollaboratorBL collaboratorBL, FundoContext context)
        {
            this.collaboratorBL = collaboratorBL;
            this.context = context;

        }

        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(AddCollab))]
        public IActionResult AddCollab(Collaborator collaborator)
        {     
            try
            {

                var result = collaboratorBL.AddCollab(collaborator);
                if (result != null)
                {
                    return this.Ok(new { success = true, msg = "Collaborator Added sucessfully", data = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, msg = "Unsuccessfull  Collaborator" });
                }

            }
            catch (System.Exception)
            {

                throw;
            }

        }

        [AllowAnonymous]
        [HttpDelete]
        [Route(nameof(DeleteCollab))]
        public IActionResult DeleteCollab(long collabId)
        {
            try
            {
                var delete = collaboratorBL.DeleteCollab(collabId);
                if (delete != null)
                {
                    return this.Ok(new { Success = true, message = "Collaborator Deleted Successfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to Delete Collaborator" });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new { success = false, message = ex.Message });
            }
        }

    }
}
