using BusinessLayer.Interface;
using CommonLayer.Model;
using DataLayer.Db;
using DataLayer.Interface;
using DataLayer.Services;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Text;

namespace BusinessLayer.Services
{
    public class NotesBL : INotesBL
    {
        private readonly INotesDL notesDL;
        public static readonly object NotesTable;
        public NotesBL(INotesDL notesDL)
        {
            this.notesDL = notesDL;
        }
            
        public NotesEntity AddNote(NotesModel notes)
        {
			try
			{                
                return notesDL.AddNote(notes);
            }
			catch (Exception)
			{

				throw;
			}
        }

        public bool CheckUserId(long userID)
        {
            try
            {
                return notesDL.CheckUserId(userID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity DeleteNote(long noteId)
        {
            try
            {
                return notesDL.DeleteNote(noteId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity UpdateNote(NotesModel noteModel, long NoteId)
        {
            try
            {
                return notesDL.UpdateNote(noteModel, NoteId);
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
                return notesDL.GetNote(NoteId);
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
                return notesDL.GetNotebyUserId(userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<NotesEntity> GetAllNote()
        {
            try
            {
                return notesDL.GetAllNote();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Pinned(long noteId)
        {
            try
            {
                return notesDL.Pinned(noteId);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Archieved(long noteId)
        {
            try
            {
                return notesDL.Archieved(noteId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool Trashed(long noteId)
        {
            try
            {
                return notesDL.Trashed(noteId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
