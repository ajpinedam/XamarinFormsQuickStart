﻿using ReactiveUI;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Core.ViewModels
{
    public class LoginViewModel : ReactiveObject, ILoginViewModel
    {
        private readonly ReactiveCommand<Unit, Unit> loginCommand;
        public LoginViewModel()
        {
            var canLogin = Observable.Return<bool>(true); // you could do some logic here instead
            this.loginCommand = ReactiveCommand.CreateFromObservable(
                this.LoginAsync,
                canLogin);
        }
        public ReactiveCommand<Unit, Unit> LoginCommand => this.loginCommand;
        private IObservable<Unit> LoginAsync() =>
            Observable
                .Return(new Random().Next(0, 2) == 1)
                //.Delay(TimeSpan.FromSeconds(1))
                .Do(
                    success =>
                    {
                        MessagingCenter.Send<ILoginViewModel, bool>(this, "LoginStatus", success);
                    }
                )
                .Select(_ => Unit.Default);
    }
}