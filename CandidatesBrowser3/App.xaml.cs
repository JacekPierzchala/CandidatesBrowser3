using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CandidatesBrowser3.DAL;
using CandidatesBrowser3.View;

namespace CandidatesBrowser3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public enum PcName
    {
        Michal,
        Zaneta
    }

    public enum Mode
    {
        Prod,
        Test
    }
    public partial class App : Application
    {
       string[] Args;
       public static PcName pcName;
       public static Mode mode;
       public static ISplashScreen splashScreen;

       private ManualResetEvent ResetSplashCreated;
       private Thread SplashThread;


        void AppStartUp(object sender, StartupEventArgs e)
        {

            ResetSplashCreated = new ManualResetEvent(false);

            // Create a new thread for the splash screen to run on
            SplashThread = new Thread(ShowSplash);
            SplashThread.SetApartmentState(ApartmentState.STA);
            SplashThread.IsBackground = true;
            SplashThread.Name = "Splash Screen";
            SplashThread.Start();

            // Wait for the blocker to be signaled before continuing. This is essentially the same as: while(ResetSplashCreated.NotSet) {}
            ResetSplashCreated.WaitOne();
          

            //SplashScreen sc = new SplashScreen(@"\Resources\cv_image.png");

            //sc.Show(false, true);
            //sc.Close(TimeSpan.FromMilliseconds(40));


            if (e.Args.Length > 1)
            {
                try
                {
                    Args = e.Args;
                    pcName = (PcName)Enum.Parse(typeof(PcName), Args[0]);
                    mode = (Mode)Enum.Parse(typeof(Mode), Args[1]);
                    switch (mode)
                    {
                        case Mode.Prod:
                            {
                                DBObjects.DBName = "Candidates";

                            }
                            break;
                        case Mode.Test:
                            {
                                DBObjects.DBName = "Candidates_test";

                            }
                            break;
                    }

                    DBObjects.SetConnectionString();

                    switch (pcName)
                    {
                        case PcName.Zaneta:
                            {
                                DBObjects.ConnectionString = DBObjects.ConnectionStringZaneta;
                            }
                            break;
                        case PcName.Michal:
                            {
                                DBObjects.ConnectionString = DBObjects.ConnectionStringMichal;
                            }
                            break;

                    }
                }

                catch(Exception ex)
                {
                    

                }

            }
            else
            {
                System.Windows.Application.Current.Shutdown();
                Environment.Exit(0);
            }


        }

        private void ShowSplash()
        {
            // Create the window
            SplashScreenView animatedSplashScreenWindow = new SplashScreenView();
            splashScreen = animatedSplashScreenWindow;

            // Show it
            animatedSplashScreenWindow.Show();

            // Now that the window is created, allow the rest of the startup to run
            ResetSplashCreated.Set();
            System.Windows.Threading.Dispatcher.Run();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(e.Exception.Message);
        }
    }
}
