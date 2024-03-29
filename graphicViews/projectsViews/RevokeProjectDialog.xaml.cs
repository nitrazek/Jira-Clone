﻿using JiraClone.viewmodels;
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

namespace JiraClone.graphicViews.projectsViews
{
    public partial class RevokeProjectDialog : Window
    {
        private ProjectsViewModel _viewModel;
        public RevokeProjectDialog(ProjectsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetAccountsToShare();
        }

        private bool AreInputValid()
        {
            bool areValid = true;
            if (Validation.GetHasError(userTextBox)) areValid = false;
            return areValid;
        }

        private void OnSubmit(object sender, EventArgs e)
        {
            if (!AreInputValid()) return;

            string? error = _viewModel.RevokeProject(ProjectName, userTextBox.Text);

            if(error != null)
            {
                formError.Content = error;
            } else Close();
        }

        public string ProjectName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
