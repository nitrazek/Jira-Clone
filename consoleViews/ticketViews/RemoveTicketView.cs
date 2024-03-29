﻿using JiraClone.utils;
using JiraClone.utils.consoleViewParts.layouts;
using JiraClone.utils.consoleViewParts.options;
using JiraClone.utils.validators;
using JiraClone.viewmodels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.views.TicketViews
{
    public class RemoveTicketView : ConsoleView
    {
		private TicketsViewModel viewModel;

		private VerticalMenu removeTicketForm;
		private HorizontalMenu actionMenu;
		private Input codeInput;
		private Button submitButton;
		private bool closeFlag = false;

		protected override void ResetView()
		{
			Clear();
			base.ResetView();
		}

		public RemoveTicketView(TicketsViewModel ticketsViewModel)
		{
			viewModel = ticketsViewModel;

			removeTicketForm = new VerticalMenu("USUWANIE ZADANIA", 3);
			actionMenu = new HorizontalMenu(2);

			codeInput = new Input("Kod", validationRule: new RequiredRule());
			submitButton = new Button("Zatwierdź", OnSubmit);

			removeTicketForm.Add(codeInput);

			actionMenu.Add(submitButton);
			actionMenu.Add(new Button("Powrót", () => { closeFlag = true; }));

			Add(new Text("Nacisnij CTRL+I aby zmienic interfejs"));
			Add(removeTicketForm);
			Add(actionMenu);
		}

        public Func<object>? Start()
        {
			ResetView();
			Print();

			while (true)
			{
                if (!Console.KeyAvailable)
                    continue;

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.I && keyInfo.Modifiers == ConsoleModifiers.Control)
                {
                    InterfaceController.CreateController().ChangeInterface();
                    EndLoop();
                    return null;
                }

                UseKey(keyInfo);

				if (closeFlag)
				{
					closeFlag = false;
					ResetView();
					return null;
				}
                if (nextView != null)
                {
                    Func<object> funcToSend = nextView;
                    nextView = null;
                    return funcToSend;
                }
            }
		}

		private bool AreInputsValid()
		{
			bool areValid = true;
			if (!codeInput.Validate()) areValid = false;
			return areValid;
		}

		private void OnSubmit()
		{
			if (!AreInputsValid()) return;

			string? error = viewModel.RemoveTicket(codeInput.Value);

			if (error != null)
			{
				submitButton.Error = error;
				Print();
			}
			else closeFlag = true;
		}
	}
}
