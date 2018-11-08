using CandidatesBrowser3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CandidatesBrowser3.DAL;
using System.Windows.Data;

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

        public CollectionView CandidatesView { get; set; }


        public MainListViewModel(ICandidateRepository CandidateRepository)
        {
            this.candidateRepository = CandidateRepository;
            
            try
            {
                LoadData();

                CandidatesView = (CollectionView)CollectionViewSource.GetDefaultView(Candidates);
                CandidatesView.Filter = UserFilter;


            }
            catch (Exception e)
            {

            }
            
        }

        private void LoadData()
        {
            Candidates = candidateRepository.GetCandidates();
      
        }


        private bool UserFilter(object item)
        {
            return true;
        }

        public void RaisePropertyChange(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
