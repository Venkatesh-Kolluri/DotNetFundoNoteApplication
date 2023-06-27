using CommonLayer.Model;
using DataLayer.Db;
using DataLayer.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Services
{
    public class LabelDL : ILableDL
    {
        private readonly FundoContext context;
        private readonly IConfiguration config;

        public LabelDL(FundoContext context,IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }
        /// <summary>
        /// AddLabel method help to add labels to the note 
        /// </summary>
        /// <param name="labelNotes"></param>
        /// <returns>added label</returns>
        public LabelEntity AddLable(LabelNotes labelNotes)
        {
            try
            {
                LabelEntity labelEntity = new LabelEntity();

                labelEntity.LabelName = labelNotes.LabelName;
                labelEntity.NoteId = labelNotes.NoteId;
                labelEntity.UserId = labelNotes.UserId;
                context.Add(labelEntity);
                context.SaveChanges();
               
                if (labelEntity != null)
                {
                    return labelEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// DeleteLabel method is used to delete labels from the notes with hep of labelId
        /// </summary>
        /// <param name="labelId"></param>
        /// <returns>deleted label</returns>
        public LabelEntity DeleteLabel(long labelId)
        {

            try
            {
                var deleteLabel = context.LabelTable.Where(x => x.LabelId == labelId).FirstOrDefault();
                if(deleteLabel != null)
                {
                    context.LabelTable.Remove(deleteLabel);
                    context.SaveChanges();
                    return deleteLabel;
                }
                else
                {
                    return null;
                }
               
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// GetAllLabel method will return all the labels present in the note
        /// </summary>
        /// <returns>all available labels</returns>
        public List<LabelEntity> GetAllLabel()
        {
            var getLabels = context.LabelTable.FirstOrDefault();
            if (getLabels != null)
            {
                return context.LabelTable.ToList();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// This method is used to gives you a particular label from all the labels with the help of labelId
        /// </summary>
        /// <param name="labelId"></param>
        /// <returns>particular label</returns>
        public List<LabelEntity> GetByLabelId(long labelId)
        {
            try
            {
                var getlabel = context.LabelTable.Where(x => x.LabelId == labelId).FirstOrDefault();
                if (getlabel != null)
                {
                    return context.LabelTable.Where(label => label.LabelId == labelId).ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// UpdateLabel method is used to update the existing label in the note
        /// </summary>
        /// <param name="labelNotes"></param>
        /// <param name="noteId"></param>
        /// <returns>updated label</returns>
        public LabelEntity UpdateLabel(LabelNotes labelNotes,int noteId)
        {
            try
            {
                var result = context.LabelTable.Where(x => x.NoteId == noteId).FirstOrDefault();
                if (result != null)
                {
                    LabelEntity labelEntity = new LabelEntity();
                    labelEntity.LabelName = labelNotes.LabelName;
                    labelEntity.UserId = labelNotes.UserId;
                    labelEntity.NoteId = labelNotes.NoteId;
                    context.Add(labelEntity);
                    context.SaveChanges();

                    return labelEntity; 

                }
                else
                {
                    return null;
                }
            
            
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
