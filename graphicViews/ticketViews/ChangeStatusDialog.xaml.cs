﻿using JiraClone.db.dbmodels;
using JiraClone.viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JiraClone.graphicViews.ticketViews
{
    public partial class ChangeStatusDialog : Window
    {
        private TicketsViewModel _viewModel;
        private Ticket _ticket;

        private bool AreInputsValid()
        {
            bool areValid = true;
            if (Validation.GetHasError(statusTextBox)) areValid = false;
            return areValid;
        }

        private void OnSubmit(object sender, EventArgs e)
        {
            if (!AreInputsValid()) return;

            string? error = _viewModel.ChangeStatus(_ticket.Code, statusTextBox.Text);
            if(error != null)
            {
                formError.Content = error;
            }
            else
            {
                Close();
            }
        }

        public ChangeStatusDialog(TicketsViewModel viewModel, Ticket ticket)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _ticket = ticket;
        }

        public string Status { get; set; } = string.Empty;
    }
}
