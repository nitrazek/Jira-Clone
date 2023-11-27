﻿using JiraClone.db.dbmodels;
using JiraClone.db.repositories;
using JiraClone.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JiraClone.viewmodels
{
	public class TicketsViewModel
	{
		private ITicketRepository _ticketRepository;
		private IStatusRepository _statusRepository;
		private IAccountRepository _accountRepository;
		private ApplicationState _applicationState;
		private ObservableCollection<KeyValuePair<string, List<Ticket>>> _statusList;

		public TicketsViewModel(
			ITicketRepository ticketRepository,
			IStatusRepository statusRepository,
			IAccountRepository accountRepository,
			ApplicationState applicationState
		) {
			_ticketRepository = ticketRepository;
			_statusRepository = statusRepository;
			_accountRepository = accountRepository;
			_applicationState = applicationState;
			_statusList = new();
		}

		public void GetStatuses()
		{
			List<Status> statusList = _statusRepository.GetStatusesFromProject(Project);
			_statusList.Clear();
			foreach (Status status in statusList)
			{
				List<Ticket> ticketList = _ticketRepository.GetTicketsFromProject(Project, status);
				_statusList.Add(new KeyValuePair<string, List<Ticket>>(status.Name, ticketList));
			}
		}

		public void AddTicket(string title, string? description, string type)
		{
			Account? loggedUser = _applicationState.GetLoggedUser();
			if (loggedUser == null)
				return;

			//obliczanie kodu zadania

			List<Status> statusList = _statusRepository.GetStatusesFromProject(Project);

			_ticketRepository.AddTicket(new Ticket
			{
				Title = title,
				Description = description,
				Type = type,
				Code = "P-1",
				CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
				ProjectId = Project.Id,
				ReporterId = loggedUser.Id,
				StatusId = statusList.First().Id
			});

			GetStatuses();
		}

		public string? RemoveTicket(string code)
		{
			Account? account = _applicationState.GetLoggedUser();
			if (account == null)
				return "Użytkownik nie jest zalogowany";

			Ticket? ticket = _ticketRepository.GetTicketByCode(code);
			if (ticket == null)
				return "Zadanie o podanej nazwie nie istnieje";

			_ticketRepository.RemoveTicket(ticket);
			GetStatuses();
			return null;
		}

		public string? AssignTicket(string ticketCode, string userLogin)
		{
			Account? loggedAccount = _applicationState.GetLoggedUser();
			if (loggedAccount == null)
				return "Użytkownik nie jest zalogowany";

			Ticket? ticket = _ticketRepository.GetTicketByCode(ticketCode);
			if (ticket == null)
				return "Zadanie o podanej nazwie nie istnieje";

			Account? account = _accountRepository.GetAccountByLogin(userLogin);
			if (account == null)
				return "Użytkownik o podanej nazwie nie istnieje";

			ticket.AssigneeId = account.Id;
			_ticketRepository.UpdateTicket(ticket);
			GetStatuses();
			return null;
		}

		public string? ChangeStatus(string ticketCode, string statusName)
		{
			Account? loggedAccount = _applicationState.GetLoggedUser();
			if (loggedAccount == null)
				return "Użytkownik nie jest zalogowany";

			Ticket? ticket = _ticketRepository.GetTicketByCode(ticketCode);
			if (ticket == null)
				return "Zadanie o podanej nazwie nie istnieje";

			Status? status = _statusRepository.GetStatusByName(statusName);
			if (status == null)
				return "Status o podanej nazwie nie istnieje";

			Status? previousStatus = _statusRepository.GetStatusById(ticket.StatusId);
			if (previousStatus != null && previousStatus.Id == status.Id)
				return "Podane zadanie ma już ten status";

			ticket.StatusId = status.Id;
			_ticketRepository.UpdateTicket(ticket);
			GetStatuses();
			return null;
		}

		public ObservableCollection<KeyValuePair<string, List<Ticket>>> StatusList
		{
			get { return _statusList; }
		}

		public Project Project { get; set; }
	}
}