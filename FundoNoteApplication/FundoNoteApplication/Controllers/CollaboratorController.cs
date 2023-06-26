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
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CollaboratorController> logger;

        public CollaboratorController(ICollaboratorBL collaboratorBL, FundoContext context,IMemoryCache memoryCache,IDistributedCache distributedCache, ILogger<CollaboratorController> logger)
        {
            this.collaboratorBL = collaboratorBL;
            this.context = context;
            this.distributedCache = distributedCache;
            this.memoryCache = memoryCache;
            this.logger = logger;
        }


        [HttpPost]
        [Route("AddCollab")]
        public IActionResult AddCollab(Collaborator collaborator)
        {     
            try
            {

                var result = collaboratorBL.AddCollab(collaborator);
                if (result != null)
                {
                    logger.LogInformation("Collaborator Added sucessfully");
                    return this.Ok(new { success = true, msg = "Collaborator Added sucessfully", data = result });
                }
                else
                {
                    logger.LogInformation("Unable to add Collaborator");
                    return this.BadRequest(new { success = false, msg = "Unable to add Collaborator" });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

        [HttpDelete]
        [Route("DeleteCollab")]
        public IActionResult DeleteCollab(long collabId)
        {
            try
            {
                var delete = collaboratorBL.DeleteCollab(collabId);
                if (delete != null)
                {
                    logger.LogInformation("Collaborator Deleted Successfully");
                    return this.Ok(new { Success = true, message = "Collaborator Deleted Successfully" });
                }
                else
                {
                    logger.LogInformation("Unable to Delete Collaborator");
                    return this.BadRequest(new { Success = false, message = "Unable to Delete Collaborator" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetAllCollaborator")]
        public IActionResult GetAllCollaborator()
        {
            try
            {
                List<CollaboratorEntity> result = collaboratorBL.GetAllCollaborator();
                if (result != null)
                {
                    logger.LogInformation("got collaborator Successfully");
                    return this.Ok(new { Success = true, message = "got collaborator Successfully", data = result });
                }
                else
                {
                    logger.LogInformation("Collaborator not Available");
                }
                    return this.BadRequest(new { Success = false, message = "Collaborator not Available" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
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
