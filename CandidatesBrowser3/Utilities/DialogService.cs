using CandidatesBrowser3.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CandidatesBrowser3.Utilities
{
    public class DialogService : IDialogService
    {
        Window Dialogwindow = null;
        

        public DialogService()
        {
        }

        public void ShowDetailDialog()
        {
            Dialogwindow = new FileDialogsView();
            Dialogwindow.ShowDialog();


        }

        public void CloseDetailDialog()
        {
            if (Dialogwindow != null)
                Dialogwindow.Close();
        }

        public void ShowAssignProjectDialog()
        {
            Dialogwindow = new AssignNewProjectView();
            Dialogwindow.ShowDialog();

        }
    }
}
