using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CandidatesBrowser3.View
{
    /// <summary>
    /// Interaction logic for SplashScreenView.xaml
    /// </summary>
    public partial class SplashScreenView : Window, ISplashScreen
    {
        public SplashScreenView()
        {
            InitializeComponent();
        }

        public void AddMessage(string message)
        {
            Dispatcher.Invoke((Action)delegate ()
            {
                this.UpdateMessageTextBox.Text = message;
            });
        }

        //public void UpdateProgress(int progress)
        //{
        //    Dispatcher.Invoke((Action)delegate ()
        //    {
        //        this.progressBar.Value = progress;
        //    });
        //}
        public void LoadComplete()
        {
            Dispatcher.InvokeShutdown();
        }
    }

    public interface ISplashScreen
    {
        void AddMessage(string message);
        void LoadComplete();

        //void UpdateProgress(int progress);
    }
}
