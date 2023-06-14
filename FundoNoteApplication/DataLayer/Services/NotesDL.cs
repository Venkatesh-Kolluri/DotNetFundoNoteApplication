using CommonLayer.Model;
using DataLayer.Db;
using DataLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DataLayer.Services
{
    public class NotesDL : INotesDL
    {
        private readonly NotesEntity userEntity;
        private readonly FundoContext context;

        public NotesDL(FundoContext context, IConfiguration config)
        {
            this.context = context;
        }

        public bool CheckUserId(long userID)
        {
            try
            {

                var check = context.UserTable.FirstOrDefault(x => x.UserId == userID);
                if (check != null)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity AddNote(NotesModel notes)
        {
            try
            {
                NotesEntity notesEntity = new NotesEntity();

                notesEntity.Title = notes.Title;
                notesEntity.Note = notes.Note;
                notesEntity.Color = notes.Color;
                notesEntity.IsArchive = notes.IsArchive;
                notesEntity.IsPin = notes.IsPin;
                notesEntity.IsTrash = notes.IsTrash;
                notesEntity.Createat = notes.Createat;
                notesEntity.UserId = notes.UserId;
                context.Add(notesEntity);
                context.SaveChanges();

                if (notesEntity != null)
                {
                    return notesEntity;
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
        public NotesEntity DeleteNote(long NoteId)
        {
            var deleteNote = context.NotesTable.Where(a => a.NoteID == NoteId).FirstOrDefault();
            if (deleteNote != null)
            {
                context.NotesTable.Remove(deleteNote);
                context.SaveChanges(); 
                return deleteNote;
            }
            else
            {
                return null;
            }
        }

        public List<NotesEntity> GetAllNote()
        {
            try
            {
                var AllNotes = context.NotesTable.FirstOrDefault();
                if (AllNotes != null)
                {
                    return context.NotesTable.ToList();
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

        public List<NotesEntity> GetNote(long NoteId)
        {
            try
            {
                var getNote = context.NotesTable.Where(x => x.NoteID == NoteId).FirstOrDefault();
                if (NoteId != null)
                {
                    return context.NotesTable.Where(x => x.NoteID == NoteId).ToList();
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

        public List<NotesEntity> GetNotebyUserId(long userId)
        {
            try
            {
                var getUserId = context.NotesTable.Where(x => x.UserId == userId).FirstOrDefault();
                if (getUserId != null)
                {
                    return context.NotesTable.Where(u => u.UserId == userId).ToList();
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
     /*   public NotesEntity UpdateNote(NotesModel noteModel,long userId)
        {
            try
            {
                var getUserId = context.NotesTable.Where(x => x.UserId == noteModel.UserId).FirstOrDefault();
                if (getUserId != null)
                {
                    return null;    //context.NotesTable.Where(u => u.UserId == noteModel.UserId).ToList();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }*/
        public bool Archieved(long NoteID, long userId)
        {
            throw new NotImplementedException();
        }

        public NotesEntity ColorNote(long NoteId, string color)
        {
            throw new NotImplementedException();
        }      
        public string Imaged(long NoteID, long userId, IFormFile image)
        {
            throw new NotImplementedException();
        }
        public bool Pinned(long NoteID, long userId)
        {
            throw new NotImplementedException();
        }

        public bool Trashed(long NoteID, long userId)
        {
            throw new NotImplementedException();
        }

        public NotesEntity UpdateNote(NotesModel noteModel, long NoteId)
        {
            throw new NotImplementedException();
        }
    }
}
