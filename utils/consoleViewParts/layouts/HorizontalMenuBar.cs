﻿using JiraClone.utils.consoleViewParts.options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.utils.consoleViewParts.layouts
{
	public class HorizontalMenuBar : Layout, IMenu
	{
		private int _visibleCount;

		private int GetStartIndex()
		{
			if (selectedChild <= _visibleCount / 2) return 0;
			if (selectedChild >= children.Count - _visibleCount / 2)
				return children.Count - _visibleCount;
			return selectedChild - _visibleCount / 2;
		}

		public HorizontalMenuBar(int visibleCount): base()
		{
			Height = 4;
			Width = 5;
			_visibleCount = visibleCount;
		}

		public override void Print()
		{
			int cursorLeft = Left;
			int cursorTop = Top;

			int startIndex = GetStartIndex();

			Console.SetCursorPosition(cursorLeft, cursorTop + (Height - 1) / 2);
			if (startIndex > 0) Console.Write("<");
			else Console.Write(" ");
			cursorLeft += 2;

			for (int i = cursorTop; i < cursorTop + Height - 2; i++)
			{
				Console.SetCursorPosition(cursorLeft, i);
				Console.Write(new StringBuilder().Append(' ', Width - 4));
			}

			for (int i = startIndex; i < Math.Min(_visibleCount + startIndex, children.Count); i++)
			{
				Console.SetCursorPosition(cursorLeft, cursorTop);
				Printable child = children[i];
				child.Left = cursorLeft; child.Top = cursorTop;
				child.Print();
				cursorLeft += child.Width + 1;
			}

			Console.SetCursorPosition(cursorLeft, cursorTop + (Height - 1) / 2);
			if (startIndex < children.Count - _visibleCount) Console.Write(">");
			else Console.Write(" ");

			if (selectedChild != -1) children[selectedChild].Print();
		}

		public bool UseKey(ConsoleKeyInfo c)
		{
			if (selectedChild < 0) return false;
			
			switch (c.Key)
			{
				case ConsoleKey.LeftArrow:
					if (((VerticalMenu)children[selectedChild]).UseKey(c)) return true;
					bool leftResult = SelectPrevious();
					if (leftResult) Print();
					return leftResult;

				case ConsoleKey.RightArrow:
					if (((VerticalMenu)children[selectedChild]).UseKey(c)) return true;
					bool rightResult = SelectNext();
					if (rightResult) Print();
					return rightResult;

				default:
					return ((VerticalMenu)children[selectedChild]).UseKey(c);
			}
		}

		public override void Add(Printable child)
		{
			if (child is not VerticalMenu) throw new NotSupportedException();

			base.Add(child);
			if (children.Count > _visibleCount) return;
			if (children.Count > 1) Width++;
			Width += child.Width;
		}

		public override void Remove(Printable child)
		{
			if (child is not VerticalMenu) throw new NotSupportedException();

			base.Remove(child);
			if (children.Count > _visibleCount) return;
			if (children.Count > 1) Width--;
			Width -= child.Width;
		}

		public void UnselectSelected()
		{
			if (selectedChild == -1)
				return;

			((VerticalMenu)children[selectedChild]).UnselectSelected();
		}

		public bool SelectTop()
		{
			if (children.Count == 0)
				return false;

			UnselectSelected();

			selectedChild = 0;
			((VerticalMenu)children[selectedChild]).SelectTop();
			return true;
		}

		public bool SelectBottom()
		{
			if (children.Count == 0)
				return false;

			UnselectSelected();

			selectedChild = children.Count - 1;
			((VerticalMenu)children[selectedChild]).SelectTop();
			return true;
		}

		public bool SelectNext()
		{
			if (selectedChild == -1)
				return false;

			if (selectedChild == children.Count - 1)
				return false;

			UnselectSelected();

			((VerticalMenu)children[++selectedChild]).SelectTop();

			return true;
		}

		public bool SelectPrevious()
		{
			if (selectedChild == -1)
				return false;

			if (selectedChild <= 0)
				return false;

			UnselectSelected();

			((VerticalMenu)children[--selectedChild]).SelectTop();

			return true;
		}

		public bool CanSelect()
		{
			foreach (VerticalMenu child in children)
			{
				if (child.CanSelect()) return true;
			}
			return false;
		}

        public override void Clear()
        {
            foreach (VerticalMenu child in children)
			{
				child.Clear();
			}
        }

        public override void ClearChildren()
		{
			base.ClearChildren();
			Height = 4;
			Width = 5;
		}

		public bool Selected { get; set; }

		public override int Height
		{
			get { return Math.Max(4, children.Select(child => child.Height).DefaultIfEmpty().Max()); }
		}
	}
}
