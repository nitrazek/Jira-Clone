﻿using JiraClone.utils;
using JiraClone.utils.consoleViewParts;
using JiraClone.utils.consoleViewParts.layouts;
using JiraClone.utils.consoleViewParts.options;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JiraClone.views
{
    public class WelcomeView: IConsoleView
    {
        private CompoundPrintable layout;
        private Menu menu;

        public WelcomeView(LoginView loginView, RegisterView registerView)
        {
            Console.CursorVisible = false;

            menu = new Menu(0, Constants.MENU_WIDTH);

            menu.Add(new Button(5, menu.Width, "Zaloguj się", loginView.Start));
            menu.Add(new Button(5, menu.Width, "Zarejestruj się", registerView.Start));

            layout = new VerticalLayout(0, Constants.WINDOW_WIDTH);
            layout.Add(new Text(1, Constants.WINDOW_WIDTH, "Nacisnij CTRL+I aby zmienic interfejs"));
            layout.Add(new Logo(7, Constants.WINDOW_WIDTH));
			layout.Add(new Text(1, Constants.WINDOW_WIDTH, "MENU GŁÓWNE"));
			layout.Add(menu);
        }

        public void Start()
        {
            Console.Clear();
		    layout.Print(0, 0);
            menu.NavigateTop();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    menu.NavigateUp();
                    continue;
                }
                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    menu.NavigateDown();
                    continue;
                }
                menu.UseKey(keyInfo.KeyChar);
                Console.Clear();
                layout.Print();
                menu.NavigateTop();
                Console.CursorVisible = false;
            }
        }
    }
}