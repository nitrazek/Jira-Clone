﻿using JiraClone.db.dbmodels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.db.repositories
{
	public class ProjectRepository : IProjectRepository
	{
		private readonly SqliteDbContext _db;

		public ProjectRepository(SqliteDbContext db)
		{
			_db = db;
		}
		public List<Project> GetProjectsOwnedByUser(Account? account)
		{
			if(account == null)
				return new List<Project>();
			else return _db.Projects
				.Where(project => project.OwnerId == account.Id)
				.ToList();
		}

		public List<Project> GetProjectsSharedForUser(Account? account)
		{
			if(account == null)
				return new List<Project>();
			else return _db.Projects
					.Where(project => project.AssignedAccounts.Contains(account))
					.ToList();
		}


        public Project? GetProjectByName(string name)
		{
			if (name == null)
				return null;
			else return _db.Projects
					.Where(project => project.Name == name)
					.FirstOrDefault();
		}

		public void AddProject(Project project)
		{
			_db.Projects.Add(project);
			_db.SaveChanges();
		}

		public void UpdateProject(Project project)
		{
			_db.Projects.Update(project);
			_db.SaveChanges();
		}

		public void RemoveProject(Project project)
		{
			_db.Projects.Remove(project);
			_db.SaveChanges();
		}
	}
}
