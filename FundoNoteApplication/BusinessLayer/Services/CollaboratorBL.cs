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
    public class CollaboratorBL : ICollaboratorBL
    {
        private readonly ICollaboratorDL collaboratorDL;

        public CollaboratorBL(ICollaboratorDL collaboratorDL)
        {
            this.collaboratorDL = collaboratorDL;
        }
        public CollaboratorEntity AddCollab(Collaborator collaborator)
        {
            try
            {
                return collaboratorDL.AddCollab(collaborator);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public CollaboratorEntity DeleteCollab(long collabId)
        {
            try
            {
                return collaboratorDL.DeleteCollab(collabId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
