using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using DataLayer.Db;
using DataLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DataLayer.Services
{
    public class NotesDL : INotesDL
    {

        private readonly FundoContext context;

        public NotesDL(FundoContext context, IConfiguration config)
        {
            this.context = context;
        }
        /// <summary>
        /// CheckUsedId method is used to check weather the UsedId exist in the database or not
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// AddNote method is used to add notes in the application,we can add as many notes as we want
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
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
        /// <summary>
        /// DeleteNote method is used to delete notes from the application with the help of noteId
        /// </summary>
        /// <param name="NoteId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// We can update the existing notes with the help of UpdateNote method by using userId
        /// </summary>
        /// <param name="notesModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public NotesEntity UpdateNote(NotesModel notesModel, long userId)
        {
            try
            {

                var update = context.NotesTable.Where(x => x.UserId == notesModel.UserId).FirstOrDefault();
                if (update != null)
                {
                    update.Title = notesModel.Title;
                    update.Note = notesModel.Note;
                    update.Color = notesModel.Color;
                    update.IsArchive = notesModel.IsArchive;
                    update.IsPin = notesModel.IsPin;
                    update.IsTrash = notesModel.IsTrash;
                    update.Createat = notesModel.Createat;
                    update.UserId = notesModel.UserId;
                    context.Add(update);
                    context.SaveChanges();

                    return update;
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
        /// Pinned method is used to pin/unpin a certain note whenever the method is called 
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public bool Pinned(long noteId)
        {
            NotesEntity notesEntity = new NotesEntity();
            var result = context.NotesTable.Where(x => x.NoteID == noteId).FirstOrDefault();
            result.IsPin = !result.IsPin;
            context.SaveChanges();
            return result.IsPin;

        }
        /// <summary>
        /// Trashed method is used to send the note to trash 
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public bool Trashed(long noteId)
        {
            var result = context.NotesTable.Where(x => x.NoteID == noteId).FirstOrDefault();
            result.IsTrash = !result.IsTrash;
            context.SaveChanges();
            return result.IsTrash;
        }
        /// <summary>
        /// Whenever Archived method is called the note will be archived
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public bool Archieved(long noteId)
        {

            var result = context.NotesTable.Where(x => x.NoteID == noteId).FirstOrDefault();
            result.IsArchive = !result.IsArchive;
            context.SaveChanges();
            return result.IsArchive;
        }
        /// <summary>
        /// ColorNote method will be used to add color to the notes by selecting different color for the note
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public NotesEntity ColorNote(long noteId, string color)
        {
            try
            {
                var result = context.NotesTable.Where(x => x.NoteID == noteId).FirstOrDefault();
                if (result != null)
                {

                    result.Color = color;
                    context.NotesTable.Update(result);
                    context.SaveChanges();
                    return result;
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
        /// GetNote method is used to retrive a particular note from the database with the help of noteId
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public List<NotesEntity> GetNote(long noteId)
        {
            try
            {
                var getNoteId = context.NotesTable.Where(x => x.NoteID == noteId).FirstOrDefault();
                if (getNoteId != null)
                {
                    return context.NotesTable.Where(u => u.NoteID == noteId).ToList();
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
        /// GettAllNotes method is used to retrive all the notes available in the database
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// This method is used to retrive note from database by using userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Image method is used to upload image in the cloudinary for a particular note with help of noteId
        /// </summary>
        /// <param name="noteID"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string Image(long noteID, IFormFile image)
        {

            try
            {
                var result = context.NotesTable.Where(x => x.NoteID == noteID).FirstOrDefault();
                if (result != null)
                {
                    Account account = new Account(
                                      "dcyfdzhuw",
                                      "351725126393698",
                                      "qBCiF-p57Ui7KSp-oCExQ_uepng");

                    Cloudinary cloudinary = new Cloudinary(account);
                    var uploadParameters = new ImageUploadParams()
                    {
                        File = new FileDescription(image.FileName, image.OpenReadStream()),
                    };
                    var uploadResult = cloudinary.Upload(uploadParameters);
                    string imagePath = uploadResult.Url.ToString();
                    result.Image = image.FileName;
                   
                    context.SaveChanges();
                    return "Image Upload Successfully";
                }
                else
                {
                    return null;
                }
            
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        
        }
        /// <summary>
        /// Find method is used to search the notes having particular lines and word in the notesDescription
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public IQueryable<NotesEntity> Find(string note)
        {
            try
            {
              //  string query = note;

                IQueryable<NotesEntity> queryable = context.Set<NotesEntity>().AsQueryable();
                var find = context.NotesTable.Where(x=>x.Note==note);
                if (find != null)
                {
                    queryable = queryable.Where(x => x.Note.Contains(note));
                    return queryable;
                   // return context.NotesTable.Where(u => u.Note == note).ToList();
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
