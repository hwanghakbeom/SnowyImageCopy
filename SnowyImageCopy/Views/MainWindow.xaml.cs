﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using PerMonitorDpi.Models;
using PerMonitorDpi.Views;
using SnowyImageCopy.Models;
using SnowyImageCopy.ViewModels;

namespace SnowyImageCopy.Views
{
	public partial class MainWindow : PerMonitorDpiWindow
	{
		public MainWindow()
		{
			this.InitializeComponent();
		}


		#region Property

		public bool IsWindowPlacementReliable
		{
			get { return (bool)GetValue(IsWindowPlacementReliableProperty); }
			set { SetValue(IsWindowPlacementReliableProperty, value); }
		}
		public static readonly DependencyProperty IsWindowPlacementReliableProperty =
			DependencyProperty.Register(
				"IsWindowPlacementReliable",
				typeof(bool),
				typeof(MainWindow),
				new FrameworkPropertyMetadata(false));

		#endregion


		private MainWindowViewModel _mainWindowViewModel;

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			IsWindowPlacementReliable = true; // This must be set before loading WindowPlacement.

			new WindowPlacement().Load(this, !CommandLine.MakesWindowStateMinimized);

			_mainWindowViewModel = this.DataContext as MainWindowViewModel;
			if (_mainWindowViewModel == null)
				return;

			if (CommandLine.StartsAutoCheck)
			{
				if (_mainWindowViewModel.CheckCopyAutoCommand.CanExecute())
					_mainWindowViewModel.CheckCopyAutoCommand.Execute();
			}

			SetDestinationColorProfile(this.WindowHandler.WindowColorProfilePath);
			this.WindowHandler.ColorProfileChanged += (sender_, e_) => SetDestinationColorProfile(e_.NewPath);
		}

		private void SetDestinationColorProfile(string colorProfilePath)
		{
			_mainWindowViewModel.DestinationColorProfile = File.Exists(colorProfilePath)
				? new ColorContext(new Uri(colorProfilePath))
				: null;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			if (e.Cancel)
				return;

			new WindowPlacement().Save(this);
		}
	}
}