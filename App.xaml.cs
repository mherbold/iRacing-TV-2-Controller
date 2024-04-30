
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

using HerboldRacing;

namespace iRacingTV
{
	public partial class App : Application
	{
		public const string AppName = "iRacing-TV 2";

		public static readonly string documentsFolderPath = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) + $"\\{AppName}\\";

		public LastUsedSettings LastUsedSettings { get; set; }

		public GeneralSettings GeneralSettings { get; private set; } = new GeneralSettings();
		public OverlaySettings OverlaySettings { get; private set; } = new OverlaySettings();

		public SettingsFileList GeneralSettingsFileList { get; private set; } = new SettingsFileList( GeneralSettings.defaultGeneralSettingsFolderPath );
		public SettingsFileList OverlaySettingsFileList { get; private set; } = new SettingsFileList( OverlaySettings.defaultOverlaySettingsFolderPath );

		public IRSDKSharper IRSDK { get; private set; } = new();

		private readonly DispatcherTimer dispatcherTimer = new();

		public static App? Instance { get; private set; } = null;

		public App()
		{
			Instance = this;

			IRSDK.OnException += OnException;
			IRSDK.OnConnected += OnConnected;
			IRSDK.OnDisconnected += OnDisconnected;
			IRSDK.OnSessionInfo += OnSessionInfo;
			IRSDK.OnTelemetryData += OnTelemetryData;
			IRSDK.OnEventSystemDataReset += OnEventSystemDataReset;
			IRSDK.OnEventSystemDataLoaded += OnEventSystemDataLoaded;
			IRSDK.OnStopped += OnStopped;

			dispatcherTimer.Tick += OnTick;
			dispatcherTimer.Interval = new TimeSpan( 0, 0, 1 );

			LastUsedSettings = LastUsedSettings.Load();

			GeneralSettings.Select( LastUsedSettings.lastUsedGeneralSettingsPath );
			OverlaySettings.Select( LastUsedSettings.lastUsedOverlaySettingsPath );
		}

		public void Start()
		{
			IRSDK.EnableEventSystem( GeneralSettings.TelemetryFolder );

			IRSDK.Start();

			dispatcherTimer.Start();
		}

		public void Stop()
		{
			MainWindow = null;

			IRSDK.Stop();

			dispatcherTimer.Stop();

			OnTick( null, new EventArgs() );
		}

		public void SetGeneralSettings( GeneralSettings generalSettings )
		{
			GeneralSettings = generalSettings;
		}

		public void SetOverlaySettings( OverlaySettings overlaySettings )
		{
			OverlaySettings = overlaySettings;
		}

		private void OnException( Exception exception )
		{
			Debug.WriteLine( "OnException() fired!" );

			IRSDK.Stop();

			Dispatcher.BeginInvoke( () =>
			{
				MessageBox.Show( exception.Message, "OnException()", MessageBoxButton.OK, MessageBoxImage.Exclamation );
			} );
		}

		private void OnConnected()
		{
			Debug.WriteLine( "OnConnected() fired!" );
		}

		private void OnDisconnected()
		{
			Debug.WriteLine( "OnDisconnected() fired!" );
		}

		private void OnSessionInfo()
		{
			Dispatcher.BeginInvoke( () =>
			{
				if ( MainWindow is MainWindow mainWindow )
				{
					mainWindow.SessionInfo_ViewControl.InvalidateVisual();
				}
			} );
		}

		private void OnTelemetryData()
		{
			Dispatcher.BeginInvoke( () =>
			{
				if ( MainWindow is MainWindow mainWindow )
				{
					mainWindow.HeaderData_ViewControl.InvalidateVisual();
					mainWindow.TelemetryData_ViewControl.InvalidateVisual();
				}
			} );
		}

		private void OnEventSystemDataReset()
		{
			Debug.WriteLine( "OnEventSystemDataReset() fired!" );

			Dispatcher.BeginInvoke( () =>
			{
				if ( MainWindow is MainWindow mainWindow )
				{
					mainWindow.Events_ListBox.ItemsSource = null;
				}
			} );
		}

		private void OnEventSystemDataLoaded()
		{
			Debug.WriteLine( "OnEventSystemDataLoaded() fired!" );

			Dispatcher.BeginInvoke( () =>
			{
				if ( MainWindow is MainWindow mainWindow )
				{
					var sortedDictionary = new SortedDictionary<string, EventSystem.EventTrack>( IRSDK.EventSystem.Tracks );

					mainWindow.Events_ListBox.ItemsSource = sortedDictionary;
				}
			} );
		}

		private void OnStopped()
		{
			Debug.WriteLine( "OnStopped() fired!" );
		}

		private void OnTick( object? sender, EventArgs e )
		{
			var isStopping = sender == null;

			LastUsedSettings.OnTick( isStopping );
			GeneralSettings.OnTick( isStopping );
			OverlaySettings.OnTick( isStopping );
		}
	}
}
