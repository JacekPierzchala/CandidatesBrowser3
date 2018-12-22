using CandidatesBrowser3.DAL;
using CandidatesBrowser3.Model;
using CandidatesBrowser3.Utilities;
using CandidatesBrowser3.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CandidatesBrowser3.ViewModel
{
    public class CandidateDetailsViewModel : INotifyPropertyChanged
    {
        #region fields
        private ICandidateHistoryRepository candidateHistoryRepository;
        #endregion

        #region properties
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

        private CandidateHistory selectedCandidateHistory;
        public CandidateHistory SelectedCandidateHistory
        {
            get { return selectedCandidateHistory; }
            set {
                selectedCandidateHistory = value;
                RaisePropertyChange("SelectedCandidateHistory");
                }
        }

   


        #endregion

        #region collections

        ObservableCollection<CandidateHistory> candidateHistoryCollection;
        ObservableCollection<CandidateHistory> CandidateHistoryCollection
        {
            get { return candidateHistoryCollection; }
            set {
                candidateHistoryCollection = value;
                RaisePropertyChange("CandidateHistoryCollection");
                }
        }

        private ObservableCollection<CandidateHistory> candidateHistoryCollectionLastStatus;
        public ObservableCollection<CandidateHistory> CandidateHistoryCollectionLastStatus
        {
            get { return candidateHistoryCollectionLastStatus; }
            set {
                candidateHistoryCollectionLastStatus = value;
                RaisePropertyChange("CandidateHistoryCollectionLastStatus");
                }
        }


        private ObservableCollection<CandidateHistory> selectedProjectHistory;
        public ObservableCollection<CandidateHistory> SelectedProjectHistory
        {
            get { return selectedProjectHistory; }
            set {
                selectedProjectHistory = value;
                RaisePropertyChange("SelectedProjectHistory");
                }
        }

        #endregion
        #region Commands
            public ICommand ProjectSelectionChangeCommand { get; set; }
        #endregion

        #region methodsForCommands
        #region ProjectSelectionChangeCommand
        private bool CanProjectSelectionChange(object obj)
        {
            if (SelectedCandidateHistory != null)
            {
                return true;
            }
            return false;
        }
        private void ProjectSelectionChange(object obj)
        {
            SelectedProjectHistory = CandidateHistoryCollection.Where(e => e.ProjectID.Equals(SelectedCandidateHistory.ProjectID)).ToObservableCollection();
        }
        #endregion

        #endregion

        public CandidateDetailsViewModel(ICandidateHistoryRepository candidateHistoryRepository)
        {
            this.candidateHistoryRepository = candidateHistoryRepository;

            SelectedCandidateTemp = new Candidate();
            Messenger.Default.Register<Candidate>(this, OnCandidateReceived);

            loadCommands();
        }


        public void OnCandidateReceived(Candidate selectedCandidate)
        {
            SelectedProjectHistory = null;
            SelectedCandidate = selectedCandidate;

            GlobalFunctions.CopyProperties(SelectedCandidate, SelectedCandidateTemp);
            loadData();
            prepareCollection();
        }

        private void prepareCollection()
        {
            CandidateHistoryCollectionLastStatus = CandidateHistoryCollection.ToList().GroupBy(e => e.ProjectID).Select(s => s.OrderByDescending(i => i.Seq).First()).ToList().ToObservableCollection();
        }

        private void loadData()
        {
            if (SelectedCandidate!=null)
            {
                CandidateHistoryCollection = candidateHistoryRepository.GetCandidateHistorysByID(SelectedCandidate.ID);
            }
            
        }

        private void loadCommands()
        {
            ProjectSelectionChangeCommand = new CustomCommand(ProjectSelectionChange, CanProjectSelectionChange);
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
