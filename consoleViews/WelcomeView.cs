﻿using JiraClone.utils;
using JiraClone.utils.consoleViewParts;
using JiraClone.utils.consoleViewParts.layouts;
using JiraClone.utils.consoleViewParts.options;
using System;
using System.Windows.Input;

namespace JiraClone.views
{
    public class WelcomeView: ConsoleView
    {
        private VerticalMenu menu;
		private Logo logo;

		protected override void ResetView()
		{
			Clear();
			base.ResetView();
		}

        public WelcomeView(LoginView loginView, RegisterView registerView)
        {
			menu = new VerticalMenu("MENU GŁÓWNE", 2);
			menu.Add(new Button("Zaloguj się", () => { StartNewConsoleView(loginView.Start); }));
            menu.Add(new Button("Zarejestruj się", () => { StartNewConsoleView(registerView.Start); }));

			logo = new Logo();

			Add(new Text("Nacisnij CTRL+I aby zmienic interfejs"));
            Add(logo);
			Add(menu);
        }

		public Func<object>? Start()
		{
			ResetView();
			Print();
			StartLoop(logo.ShiftToSide);

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
				if(nextView != null)
				{
					Func<object> funcToSend = nextView;
					nextView = null;
					return funcToSend;
				}
			}
		}
	}
}
