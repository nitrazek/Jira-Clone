﻿using JiraClone.db.dbmodels;
using JiraClone.db.repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.viewmodels
{
    public class RegisterViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IAccountRepository accountRepository;

        public RegisterViewModel(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public string? RegisterUser(string login, string password, string email, string name, string surname)
        {
            Account? accountByLogin = accountRepository.GetAccountByLogin(login);
            if (accountByLogin != null)
                return "Konto o podanym loginie już istnieje";

            Account? accountByEmail = accountRepository.GetAccountByLogin(login);
            if (accountByEmail != null)
                return "Konto o podanym email'u już istnieje";

            Account newAccount = new Account
            {
                Id = 2,
                Login = login,
                Password = password,
                Email = email,
                Name = name,
                Surname = surname,
                CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };

            accountRepository.AddAccount(newAccount);

            //List<Account> accounts = accountRepository.GetAllAccounts();

            return null;
        }
    }
}