using CommonLayer.Model;
using DataLayer.Db;
using DataLayer.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public LabelEntity DeleteLabel(long labelId)
        {
            throw new NotImplementedException();
        }

        public List<LabelEntity> GetAllLabel()
        {
            throw new NotImplementedException();
        }

        public List<LabelEntity> GetByLabelId()
        {
            throw new NotImplementedException();
        }

        public LabelEntity UpdateLabel(long labelId, int noteId)
        {
            throw new NotImplementedException();
        }
    }
}
