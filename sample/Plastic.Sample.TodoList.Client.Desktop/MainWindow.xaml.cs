﻿using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Plastic.Sample.TodoList.Client.Desktop.ViewModels;

namespace Plastic.Sample.TodoList.Client.Desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // HACK: just sample, don't use like this...
        this.DataContext = App.Provider.GetRequiredService<TodoListViewModels>();
    }
}
