﻿using JiraClone.utils;
using JiraClone.utils.consoleViewParts;
using JiraClone.utils.consoleViewParts.layouts;
using JiraClone.utils.consoleViewParts.options;
using JiraClone.utils.validators;
using JiraClone.viewmodels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.views
{
    public class RegisterView : ConsoleView
	{
		private VerticalMenu registerForm;
		private HorizontalMenu actionMenu;
		private RegisterViewModel viewModel;
		private Input loginInput, passwordInput, emailInput, nameInput, surnameInput;
		private Button submitButton;
		private bool closeFlag = false;

		protected override void ResetView()
		{
			loginInput.Clear();
			passwordInput.Clear();
			emailInput.Clear();
			nameInput.Clear();
			surnameInput.Clear();

			base.ResetView();
		}

		public RegisterView(RegisterViewModel viewModel)
		{
			this.viewModel = viewModel;
            viewModel.PropertyChanged += EventHandler;

            registerForm = new VerticalMenu("REJESTRACJA", 3);
			actionMenu = new HorizontalMenu(2);

			loginInput = new Input("Login", validationRule: new RequiredRule());
			passwordInput = new Input("Hasło", true, validationRule: new RequiredRule());
			emailInput = new Input("Email", validationRule: new EmailRule());
			nameInput = new Input("Imię", validationRule: new RequiredRule());
			surnameInput = new Input("Nazwisko", validationRule: new RequiredRule());
			submitButton = new Button("Zatwierdź", OnSubmit);

            registerForm.Add(loginInput);
			registerForm.Add(passwordInput);
			registerForm.Add(emailInput);
			registerForm.Add(nameInput);
			registerForm.Add(surnameInput);

			actionMenu.Add(submitButton);
			actionMenu.Add(new Button("Powrót", () => { closeFlag = true; }));

			Add(new Text("Nacisnij CTRL+I aby zmienic interfejs"));
			Add(new Logo());
			Add(registerForm);
			Add(actionMenu);
		}

        private void EventHandler(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
        }

        public void Start()
		{
			ResetView();
			Print();

			while (true)
			{
                if (!Console.KeyAvailable)
                    continue;

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
				UseKey(keyInfo);

				if (closeFlag)
                {
                    closeFlag = false;
                    ResetView();
                    return;
                }
            }
		}

		private bool AreInputsValid()
		{
			bool areValid = true;
			if (!loginInput.Validate()) areValid = false;
			if (!passwordInput.Validate()) areValid = false;
			if (!emailInput.Validate()) areValid = false;
			if (!nameInput.Validate()) areValid = false;
			if (!surnameInput.Validate()) areValid = false;
			return areValid;
		}

		private void OnSubmit()
		{
			if (!AreInputsValid()) return;

			string? error = viewModel.RegisterUser(
				login: loginInput.Value,
				password: passwordInput.Value,
				email: emailInput.Value,
				name: nameInput.Value,
				surname: surnameInput.Value);

            if (error != null)
            {
                submitButton.Error = error;
                submitButton.Print();
                return;
            }
        }
	}
}
