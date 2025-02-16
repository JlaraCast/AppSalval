﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppSalval.ViewModels
{
    
    public class InicioDesarrolladorViewModel : BindableObject
    {
        public ICommand NavigateToRecomPageCommand { get; }
        public ICommand NavigateToRiskPageCommand { get; }

        public InicioDesarrolladorViewModel(INavigation navigation)
        {
            NavigateToRecomPageCommand = new Command(async () => await navigation.PushAsync(new Views.RecomPage()));
            NavigateToRiskPageCommand = new Command(async () => await navigation.PushAsync(new Views.RiskPage()));
        }
    }
}
