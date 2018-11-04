using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CandidatesBrowser3.DAL;

namespace CandidatesBrowser3.ViewModel
{
   public class MainListViewModel:INotifyPropertyChanged
    {
        private ICandidateRepository candidateRepository;


        private ObservableCollection<Candidate> candidates;
        public ObservableCollection<Candidate> Candidates
        {
            get { return candidates; }
            set {
                candidates = value;
                RaisePropertyChange("Candidates");
            }
        }

        private Candidate selectedCandidate;
        public Candidate SelectedCandidate
        {
            get { return selectedCandidate; }
            set {
                selectedCandidate = value;
                RaisePropertyChange("SelectedCandidate");
            }
        }

        public MainListViewModel(ICandidateRepository CandidateRepository)
        {
            this.candidateRepository = CandidateRepository;
            try
            {
                LoadData();
            }
            catch(Exception e)
            {

            }
            
        }

        private void LoadData()
        {
            Candidates = candidateRepository.GetCandidates();

        }

        public void RaisePropertyChange(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
