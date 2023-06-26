using BusinessLayer.Interface;
using BusinessLayer.Services;
using CommonLayer.Model;
using DataLayer.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundoNoteApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        ICollaboratorBL collaboratorBL;
        private readonly FundoContext context;
        private readonly IDistributedCache distributedCache;
        private readonly IMemoryCache memoryCache;

        public CollaboratorController(ICollaboratorBL collaboratorBL, FundoContext context,IMemoryCache memoryCache,IDistributedCache distributedCache)
        {
            this.collaboratorBL = collaboratorBL;
            this.context = context;
            this.distributedCache = distributedCache;
            this.memoryCache = memoryCache;
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
        [AllowAnonymous]
        [HttpGet]
        [Route(nameof(GetAllCollaborator))]
        public IActionResult GetAllCollaborator()
        {
            try
            {
                List<CollaboratorEntity> result = collaboratorBL.GetAllCollaborator();
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = " collaborator got Successfully", data = result });
                }
                else
                    return this.BadRequest(new { Success = false, message = "Collaborator not Available" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpGet("redis")]
        public async Task<IActionResult> GetAllLabelsUsingRedisCache()
        {
            var cacheKey = "CollaboratorList";
            string serializedCollaboratorList;
            var collaboratorList = new List<CollaboratorEntity>();
            var redisCollaboratorList = await distributedCache.GetAsync(cacheKey);
            if (redisCollaboratorList != null)
            {
                serializedCollaboratorList = Encoding.UTF8.GetString(redisCollaboratorList);
                collaboratorList = JsonConvert.DeserializeObject<List<CollaboratorEntity>>(serializedCollaboratorList);
            }
            else
            {
                collaboratorList = await context.CollaboratorTable.ToListAsync();
                serializedCollaboratorList = JsonConvert.SerializeObject(collaboratorList);
                redisCollaboratorList = Encoding.UTF8.GetBytes(serializedCollaboratorList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisCollaboratorList, options);
            }
            return Ok(collaboratorList);

        }

    }
}
