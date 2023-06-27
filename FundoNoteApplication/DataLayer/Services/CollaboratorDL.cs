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
    public class CollaboratorDL : ICollaboratorDL
    {
        private readonly FundoContext context;

        public CollaboratorDL(FundoContext context,IConfiguration config)
        {
            this.context = context;
        }
        /// <summary>
        /// AddCollab method is used to add an email to notes which an be shared and used by the collaborator
        /// </summary>
        /// <param name="collaborator"></param>
        /// <returns>added collaborator</returns>
        public CollaboratorEntity AddCollab(Collaborator collaborator)
        {
            try
            {
                CollaboratorEntity collabEntity = new CollaboratorEntity();
                collabEntity.CollaboratedEmail = collaborator.CollaboratedEmail;
                collabEntity.NoteID = collaborator.NoteId;
                collabEntity.UserId = collaborator.UserId;
                context.Add(collabEntity);
                context.SaveChanges();

                if (collabEntity != null)
                {
                    return collabEntity;
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
        /// GetAllCollaborator method will give all the available collabrator with certain note
        /// </summary>
        /// <returns>all available collaborator</returns>
        public List<CollaboratorEntity> GetAllCollaborator()
        {
            try
            {
                var result = context.CollaboratorTable.FirstOrDefault();
                if (result != null)
                {
                    return context.CollaboratorTable.ToList();
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
        /// DeleteCollab method is used to delete a collaborator with the help of collabId
        /// </summary>
        /// <param name="collabId"></param>
        /// <returns>deleted collaborator</returns>
        public CollaboratorEntity DeleteCollab(long collabId)
        {
            try
            {
                try
                {
                    var deleteCollab = context.CollaboratorTable.Where(x => x.CollabId == collabId).FirstOrDefault();
                    if (deleteCollab != null)
                    {
                        context.CollaboratorTable.Remove(deleteCollab);
                        context.SaveChanges();
                        return deleteCollab;
                    }
                    return null;
                }
                catch (Exception)
                {

                    throw;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }       
    }
}
