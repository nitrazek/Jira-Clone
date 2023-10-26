﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.db.dbmodels
{
	public class Project
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public long CreationTimestamp { get; set; }
		public List<AccountProject>? AccountProjects { get; set; }
		public List<Ticket>? Tickets { get; set; }
		public List<Status>? Statuses { get; set; }
	}
}