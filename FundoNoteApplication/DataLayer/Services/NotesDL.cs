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
      //  private readonly NotesEntity userEntity;
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



        public NotesEntity UpdateNote(NotesModel notesModel, long userId)
        {
            try
            {
                //    NotesEntity update = new NotesEntity();
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
        public bool Pinned(long noteId)
        {
            NotesEntity notesEntity = new NotesEntity();
            var result = context.NotesTable.Where(x => x.NoteID == noteId).FirstOrDefault();
            result.IsPin = !result.IsPin;
            context.SaveChanges();
            return result.IsPin;

        }
        public bool Trashed(long noteId)
        {
            var result = context.NotesTable.Where(x => x.NoteID == noteId).FirstOrDefault();
            result.IsTrash = !result.IsTrash;
            context.SaveChanges();
            return result.IsTrash;
        }
        public bool Archieved(long noteId)
        {

            var result = context.NotesTable.Where(x => x.NoteID == noteId).FirstOrDefault();
            result.IsArchive = !result.IsArchive;
            context.SaveChanges();
            return result.IsArchive;
        }

        public NotesEntity ColorNote(long NoteId, string color)
        {
            throw new NotImplementedException();
        }
      
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
    }
}
