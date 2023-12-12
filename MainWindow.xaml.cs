﻿using JiraClone.graphicViews;
using JiraClone.graphicViews.ticketViews;
using JiraClone.utils;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JiraClone
{
    public partial class MainWindow : Window
    {
        //private WelcomePage _welcomePage;
        private TicketsPage _ticketsPage;
        public MainWindow(/*WelcomePage welcomePage,*/ TicketsPage ticketsPage)
        {
            InitializeComponent();
            //_welcomePage = welcomePage;
            _ticketsPage = ticketsPage;
            //PageFrame.Navigate(_welcomePage);
            PageFrame.Navigate(_ticketsPage);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            //if (e.Key == Key.I)
            //    InterfaceController.CreateController().ChangeInterface();
        }
    }
}
