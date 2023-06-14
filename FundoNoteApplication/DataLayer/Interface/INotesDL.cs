using DataLayer.Db;
using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;

namespace DataLayer.Interface
{
    public interface INotesDL
    {
        public NotesEntity AddNote(NotesModel notes);
        public NotesEntity DeleteNote(long NoteId);
        public NotesEntity UpdateNote(NotesModel noteModel, long NoteId);
        public List<NotesEntity> GetNote(long NoteId);
        public List<NotesEntity> GetNotebyUserId(long userId);
        public List<NotesEntity> GetAllNote();
        public bool Pinned(long NoteID, long userId);
        public bool Trashed(long NoteID, long userId);
        public bool Archieved(long NoteID, long userId);
        public NotesEntity ColorNote(long NoteId, string color);
        public string Imaged(long NoteID, long userId, IFormFile image);
        public bool CheckUserId(long userID);
    }

}
