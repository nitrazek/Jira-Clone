﻿using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.utils.views
{
	public class Option : IOption
	{
		public readonly string? Title;
		public readonly int X;
		public readonly int Y;

		public Option(string title, int x, int y)
		{
			Title = title;
			X = x;
			Y = y;
		}

		public void Select()
		{
			Console.SetCursorPosition(X, Y);
			Console.ForegroundColor = ConsoleColor.DarkBlue;
			Console.Write(Title);
		}

		public void Unselect()
		{
			Console.SetCursorPosition(X, Y);
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(Title);
		}

		public override string ToString()
		{
			return Title ?? "";
		}
	}
}
