﻿using Autofac;
using Plugin.GoogleAnalytics;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Target.Interfaces;
using Target.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Target.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPageBase<AboutPageViewModel>, IAboutPage
    {
        Page termspage;
        public AboutPage()
        {
            InitializeComponent();
            ViewModel = (AboutPageViewModel)App.Container.Resolve<IAboutPageViewModel>();
            var policypage = (Page)App.Container.Resolve<IPolicyPage>();
            if (Constants.IsTermsPageEnabled)
            {
                termspage = (Page)App.Container.Resolve<ITermsPage>();
            }

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(ViewModel, vm => vm.Greeting, x => x.lbl.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.FontSize, x => x.lbl.FontSize, vmToViewConverterOverride: bindingIntToDoubleConverter)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.FontSize, x => x.btnPolicy.FontSize, vmToViewConverterOverride: bindingIntToDoubleConverter)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.FontSize, x => x.btnTerms.FontSize, vmToViewConverterOverride: bindingIntToDoubleConverter)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(ViewModel, vm => vm.IsTermsOn, x => x.btnTerms.IsVisible)
                            .DisposeWith(disposables);
                        this.btnPolicy.Events().Clicked.Throttle(TimeSpan.FromMilliseconds(150), RxApp.MainThreadScheduler)
                            .Do((x) =>
                            {
                                Navigation.PushAsync(policypage);
                            })
                            .Subscribe().DisposeWith(disposables);
                        this.btnTerms.Events().Clicked.Throttle(TimeSpan.FromMilliseconds(150), RxApp.MainThreadScheduler)
                            .Do((x) =>
                            {
                                Navigation.PushAsync(termspage);
                            })
                            .Subscribe().DisposeWith(disposables);
                        this
                            .OneWayBind(ViewModel, vm => vm.Version, x => x.lblVersion.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.FontSize, x => x.lblVersion.FontSize, vmToViewConverterOverride: bindingIntToDoubleConverter)
                            .DisposeWith(disposables);
                        this
                           .OneWayBind(ViewModel, vm => vm.AppName, x => x.lblAppName.Text)
                           .DisposeWith(disposables);
                        this
                           .OneWayBind(ViewModel, vm => vm.HTMLSource, x => x.webview.Source)
                           .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.FontSize, x => x.lblAppName.FontSize, vmToViewConverterOverride: bindingIntToDoubleConverter)
                            .DisposeWith(disposables);                        
                    });
            
        }
        protected override void OnAppearing()
        {
            // Cannot be depended on in Android when navigating back to page
            base.OnAppearing();
            
            GoogleAnalytics.Current.Tracker.SendView("AboutPage");
        }
    }
}