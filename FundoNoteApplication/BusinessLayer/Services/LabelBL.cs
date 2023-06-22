using BusinessLayer.Interface;
using CommonLayer.Model;
using DataLayer.Db;
using DataLayer.Interface;
using DataLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class LabelBL : ILabelBL
    {
        private readonly ILableDL labelDL;

        public LabelBL(ILableDL labelDL )
        {
            this.labelDL = labelDL;
        }
        public LabelEntity AddLable(LabelNotes labelNotes)
        {
            try
            {
                return labelDL.AddLable(labelNotes);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public LabelEntity DeleteLabel(long labelId)
        {
            try
            {
                return labelDL.DeleteLabel(labelId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<LabelEntity> GetAllLabel()
        {
            try
            {
                return labelDL.GetAllLabel();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<LabelEntity> GetByLabelId(long labelId)
        {
            try
            {
                return labelDL.GetByLabelId(labelId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public LabelEntity UpdateLabel(LabelNotes labelNotes, int noteId)
        {
            try
            {
                return labelDL.UpdateLabel(labelNotes,noteId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
