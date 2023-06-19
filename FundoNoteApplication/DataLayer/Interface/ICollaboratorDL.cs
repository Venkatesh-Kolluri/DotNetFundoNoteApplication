using CommonLayer.Model;
using DataLayer.Db;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interface
{
    public interface ICollaboratorDL
    {
        public CollaboratorEntity AddCollab(Collaborator collaborator);
        public CollaboratorEntity DeleteCollab(long CollabId);
        public List<CollaboratorEntity> GetAllCollaborator();
    }
}
