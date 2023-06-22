using BusinessLayer.Interface;
using CommonLayer.Model;
using DataLayer.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FundoNoteApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL labelBL;
        private readonly FundoContext context;

        public LabelController(ILabelBL labelBL,FundoContext context)
        {
            this.labelBL = labelBL;
            this.context = context;
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
                    return this.Ok(new { success = true, message="Label added Successfully", Data=label }); 
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Label not added" });
                }
            }
            catch (System.Exception)
            {

                throw;
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
                    return this.Ok(new { success = true, message = "label deleted", data = result });

                }
                else
                {
                    return this.Ok(new { success = false, message = "unable to delete label" });
                }

            }
            catch (System.Exception)
            {

                throw;
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
                    return this.Ok(new { success = true, message = "All available labels for the note  ", data = result });

                }
                else
                {
                    return this.Ok(new { success = false, message = "Labels are not availabel" });
                }
            }
            catch (System.Exception)
            {

                throw;
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
                    return this.Ok(new { success = true, message = "label Available", data = result });

                }
                else
                {
                    return this.Ok(new { success = false, message = "label unavailable" });
                }
            }
            catch (System.Exception)
            {

                throw;
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
                    return this.Ok(new { success = true, message = "label Available", data = result });

                }
                else
                {
                    return this.Ok(new { success = false, message = "label unavailable" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

    }
}
