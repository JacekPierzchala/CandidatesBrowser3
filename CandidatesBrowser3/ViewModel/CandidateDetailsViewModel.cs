using CandidatesBrowser3.Model;
using CandidatesBrowser3.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidatesBrowser3.ViewModel
{
    public class CandidateDetailsViewModel : INotifyPropertyChanged
    {
        private Candidate selectedCandidate;
        public Candidate SelectedCandidate
        {
            get { return selectedCandidate; }
            set
            {
                selectedCandidate = value;
                RaisePropertyChange("SelectedCandidate");
            }
        }

        private Candidate selectedCandidateTemp;
        public Candidate SelectedCandidateTemp
        {
            get { return selectedCandidateTemp; }
            set
            {
                selectedCandidateTemp = value;
                RaisePropertyChange("SelectedCandidateTemp");
            }
        }

        public CandidateDetailsViewModel()
        {
            SelectedCandidateTemp = new Candidate();
            Messenger.Default.Register<Candidate>(this, OnCandidateReceived);
        }


        public void OnCandidateReceived(Candidate selectedCandidate)
        {

            SelectedCandidate = selectedCandidate;
           
            GlobalFunctions.CopyProperties(SelectedCandidate, SelectedCandidateTemp);
        }




        public void RaisePropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
