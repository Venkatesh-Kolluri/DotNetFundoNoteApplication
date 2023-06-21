using CommonLayer.Model;
using DataLayer.Db;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface INotesBL
    {
        public NotesEntity AddNote(NotesModel notes);
        public bool CheckUserId(long userID);
        public NotesEntity DeleteNote(long NoteId);
        public NotesEntity UpdateNote(NotesModel noteModel, long NoteId);
        public List<NotesEntity> GetNote(long NoteId);
        public List<NotesEntity> GetNotebyUserId(long userId);
        public List<NotesEntity> GetAllNote();
        public bool Pinned(long noteId);
        public bool Archieved(long noteId);
        public bool Trashed(long noteId);
        public string Image(long noteID, IFormFile image);
        public IQueryable<NotesEntity> Find(string note);

    }
}
