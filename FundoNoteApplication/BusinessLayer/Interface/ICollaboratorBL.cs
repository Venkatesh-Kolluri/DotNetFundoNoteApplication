using CommonLayer.Model;
using DataLayer.Db;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface ICollaboratorBL
    {
        public CollaboratorEntity AddCollab(Collaborator collaborator);
        public CollaboratorEntity DeleteCollab(long CollabId);
       // public List<CollaboratorEntity> GetAllCollaborator();
    }
}
