using CommonLayer.Model;
using DataLayer.Db;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface ILabelBL
    {
        public LabelEntity AddLable(LabelNotes labelNotes);
        public LabelEntity DeleteLabel(long labelId);
        public LabelEntity UpdateLabel(LabelNotes labelNotes, int noteId);
        public List<LabelEntity> GetAllLabel();
        public List<LabelEntity> GetByLabelId(long labelId);
    }
}
