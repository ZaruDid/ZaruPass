using System;
using Avalonia.Controls;
using ZaruPass.Core.DataBase;
using System.Threading.Tasks;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using ZaruPass.Core.Configuration;
using ZaruPass.Program.Features.Clipboard;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace ZaruPass.Program.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private const float _s = 0.1f;
    private const float _s2 = 0.4f;
    [ObservableProperty]
    private ViewModelBase _mainBlock;
    [ObservableProperty]
    private GridLength _sideBarWigth;
    private readonly IServiceProvider _serviceProvider;

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        OpenSitesList();
        SideBarWigth = new GridLength(_s, GridUnitType.Star);
    }
    [RelayCommand]
    public void ToggleSideBar()
    {
        if (SideBarWigth.Value == _s)
        {
            SideBarWigth = new GridLength(_s2, GridUnitType.Star);
        }
        else
        {
            SideBarWigth = new GridLength(_s, GridUnitType.Star);
        }
    }
    [RelayCommand]
    public void OpenSitesList()
    {
        MainBlock = _serviceProvider.GetRequiredService<SitesListViewModel>();
    }
    [RelayCommand]
    public void OpenAddSite()
    {
        MainBlock = _serviceProvider.GetRequiredService<AddSiteViewModel>();
    }
}
