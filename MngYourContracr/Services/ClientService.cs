﻿using System;
using System.Collections.Generic;
using System.Linq;
using MngYourContracr.MngYourContractDatabase;

namespace MngYourContracr.Service
{
    public class ClientService: Service<Client>
    {
        public Service<Project> project;
        public UserService userService;
        public ClientService(CompanyContext context)
            : base(context)
        {
        }

        public List<Project> GetCompletedProjects(String clientId)
        {
            List<Project> listToReturn = new List<Project>();
            var queryCompletedProjects = from proj in project.Get()
                                         where proj.ClientId == clientId && proj.EndDate < DateTime.Today
                                         select proj;
            foreach (Project p in queryCompletedProjects)
            {
                listToReturn.Add(p);
            }
            return listToReturn;
        }

        public List<Project> GetCurrentProjects(String clientId)
        {
            List<Project> listToReturn = new List<Project>();
            var queryCompletedProjects = from proj in project.Get()
                                         where proj.ClientId == clientId && proj.EndDate > DateTime.Today
                                         select proj;
            foreach (Project p in queryCompletedProjects)
            {
                listToReturn.Add(p);
            }
            return listToReturn;
        }

        public List<Project> GetAllProjects(String clientId)
        {
            List<Project> listToReturn = new List<Project>();
            var queryCompletedProjects = from proj in project.Get()
                                         where proj.ClientId == clientId
                                         select proj;
            foreach (Project p in queryCompletedProjects)
            {
                listToReturn.Add(p);
            }
            return listToReturn;
        }

        public void ChangeProjectDeadline(Project proj, DateTime deadLine, String clientID)
        {
            if (proj.ClientId == clientID)
            {
                proj.Deadline = deadLine;
                project.Update(proj);
            }
        }

        public void ChangeProjectDeadline(String projId, DateTime deadLine, String clientID)
        {
            ChangeProjectDeadline(project.GetByID(projId), deadLine, clientID);
        }
        public void createClient(string user_id) {

            var client = new Client
            {
                ClientId = user_id
            };
            this.Insert(client);
            this.Save();
     
        }

    }
}