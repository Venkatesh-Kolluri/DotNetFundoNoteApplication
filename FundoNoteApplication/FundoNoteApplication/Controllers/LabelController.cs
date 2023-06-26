using BusinessLayer.Interface;
using CommonLayer.Model;
using DataLayer.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FundoNoteApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
      
        private readonly ILabelBL labelBL;
        private readonly FundoContext context;
        private readonly IDistributedCache distributedCache;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<LabelController> logger;

        public LabelController(ILabelBL labelBL,FundoContext context, IMemoryCache  memoryCache,IDistributedCache distributedCache, ILogger<LabelController> logger)
        {
            this.labelBL = labelBL;
            this.context = context;
            this.distributedCache = distributedCache;
            this.memoryCache = memoryCache;
            this.logger = logger;
        }
  
        [HttpPost]
        [Route("addlabel")]
        public IActionResult AddLabel(LabelNotes labelNotes)
        {
            try
            {
                var label = labelBL.AddLable(labelNotes);
                if (label != null)
                {
                    logger.LogInformation("Label added Successfully");
                    return this.Ok(new { success = true, message="Label added Successfully", Data=label }); 
                }
                else
                {
                    logger.LogInformation("Label not added");
                    return this.BadRequest(new { success = false, message = "Label not added" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

        [HttpDelete]
        [Route("deletelabel")]
        public IActionResult DeleteLabel(long labelId)
        {
            try
            {
                var result = labelBL.DeleteLabel(labelId);
                if(result != null)
                {
                    logger.LogInformation("label deleted");
                    return this.Ok(new { success = true, message = "label deleted", data = result });

                }
                else
                {
                    logger.LogInformation("unable to delete label");
                    return this.Ok(new { success = false, message = "unable to delete label" });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        [Route("getalllabels")]
        public IActionResult GetAllLabels()
        {
            try
            {
                var result = labelBL.GetAllLabel();
                if (result != null)
                {
                    logger.LogInformation("All available labels for the note");
                    return this.Ok(new { success = true, message = "All available labels for the note", data = result });

                }
                else
                {
                    logger.LogInformation("Labels are not availabel");
                    return this.Ok(new { success = false, message = "Labels are not availabel" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        [Route("getbylabelId")]
        public IActionResult GetByLabelId(long labelId)
        {
            try
            {
                var result = labelBL.GetByLabelId(labelId);
                if (result != null)
                {
                    logger.LogInformation("labelId Available");
                    return this.Ok(new { success = true, message = "labelId Available", data = result });

                }
                else
                {
                    logger.LogInformation("labelId unavailable");
                    return this.Ok(new { success = false, message = "labelId unavailable" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPut]
        [Route ("updatelable")]
        public IActionResult UpdateLable(LabelNotes labelNotes, int noteId)
        {
            try
            {
                var result = labelBL.UpdateLabel(labelNotes,noteId);
                if (result != null)
                {
                    logger.LogInformation("label updated");
                    return this.Ok(new { success = true, message = "label updated", data = result });

                }
                else
                {
                    logger.LogInformation("unavailable to update label");
                    return this.Ok(new { success = false, message = "unavailable to update label" });
                }
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
            try
            {
                logger.LogInformation("getting all availabel label");
                var cacheKey = "LabelsList";
                string serializedLabelsList;
                var labelsList = new List<LabelEntity>();
                var redisLabelsList = await distributedCache.GetAsync(cacheKey);
                if (redisLabelsList != null)
                {
                    serializedLabelsList = Encoding.UTF8.GetString(redisLabelsList);
                    labelsList = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLabelsList);
                }
                else
                {
                    labelsList = await context.LabelTable.ToListAsync();
                    serializedLabelsList = JsonConvert.SerializeObject(labelsList);
                    redisLabelsList = Encoding.UTF8.GetBytes(serializedLabelsList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey, redisLabelsList, options);
                }
                return Ok(labelsList);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

    }
}
