﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.utils.consoleViewParts.options
{
	public interface ISelectable : IOption
	{
		public void UnselectSelected();
		public void SelectTop();
		public void SelectBottom();
		public bool SelectUp();
		public bool SelectDown();
	}
}
