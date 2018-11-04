using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CandidatesBrowser3.DAL;

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

       void AppStartUp(object sender, StartupEventArgs e)
        {
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
    }
}
