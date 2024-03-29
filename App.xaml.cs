﻿using JiraClone.db;
using JiraClone.db.repositories;
using JiraClone.graphicViews;
using JiraClone.graphicViews.commentsViews;
using JiraClone.graphicViews.projectsViews;
using JiraClone.graphicViews.ticketViews;
using JiraClone.models;
using JiraClone.utils;
using JiraClone.viewmodels;
using JiraClone.views;
using JiraClone.views.CommentViews;
using JiraClone.views.ProjectViews;
using JiraClone.views.TicketViews;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace JiraClone
{
    public partial class App : Application
    {
        private static IHost? AppHost { get; set; }

        private IHost BuildAppHost()
        {
			var builder = Host.CreateDefaultBuilder();
			builder.ConfigureServices((context, services) =>
			{
				services.AddDbContext<SqliteDbContext>(options =>
				{
					//dodać connection string
					options.UseSqlite($"Data Source={AppDomain.CurrentDomain.BaseDirectory}/sqlite.db");
                    options.UseLazyLoadingProxies().UseSqlite($"Data Source={AppDomain.CurrentDomain.BaseDirectory}/sqlite.db");
				});
        
                //States
                services.AddSingleton<ApplicationState>();
                //ConsoleViews
                services.AddSingleton<WelcomeView>();
                services.AddSingleton<LoginView>();
                services.AddSingleton<RegisterView>();
                services.AddSingleton<ProjectsView>();
                services.AddSingleton<ProjectDetailsView>();
                services.AddSingleton<AddProjectView>();
                services.AddSingleton<RemoveProjectView>();
                services.AddSingleton<ShareProjectView>();
                services.AddSingleton<RevokeProjectView>();
				services.AddSingleton<TicketsView>();
                services.AddSingleton<TicketDetailsView>();
				services.AddSingleton<AddTicketView>();
				services.AddSingleton<RemoveTicketView>();
				services.AddSingleton<AssignTicketView>();
                services.AddSingleton<UnassignTicketView>();
                services.AddSingleton<ChangeStatusView>();
                services.AddSingleton<CommentsView>();
                services.AddSingleton<AddCommentView>();
                //GraphicViews
                services.AddSingleton<MainWindow>();
                services.AddSingleton<WelcomePage>();
                services.AddSingleton<LoginPage>();
                services.AddSingleton<RegisterPage>();
                services.AddSingleton<TicketsPage>();
                services.AddSingleton<CommentsPage>();
                services.AddSingleton<ProjectsPage>();
				//ViewModels
				services.AddSingleton<LoginViewModel>();
                services.AddSingleton<RegisterViewModel>();
                services.AddSingleton<ProjectsViewModel>();
                services.AddSingleton<TicketsViewModel>();
                services.AddSingleton<CommentsViewModel>();
                //Repositories
                services.AddSingleton<IAccountRepository, AccountRepository>();
                services.AddSingleton<IProjectRepository, ProjectRepository>();
                services.AddSingleton<ITicketRepository, TicketRepository>();
                services.AddSingleton<IStatusRepository, StatusRepository>();
                services.AddSingleton<ICommentRepository, CommentRepository>();

                using var serviceProvider = services.BuildServiceProvider();
				var dbContext = serviceProvider.GetRequiredService<SqliteDbContext>();
				dbContext.Database.Migrate();
			});
            builder.UseDefaultServiceProvider(options => options.ValidateScopes = false);
			return builder.Build();
		}

        public App() {
            AppHost = BuildAppHost();

            MainWindow window = AppHost.Services.GetRequiredService<MainWindow>();
            WelcomeView console = AppHost.Services.GetRequiredService<WelcomeView>();
            InterfaceController.CreateController(window, console);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}
