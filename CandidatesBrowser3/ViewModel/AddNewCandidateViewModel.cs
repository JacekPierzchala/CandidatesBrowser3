﻿using CandidatesBrowser3.DAL;
using CandidatesBrowser3.Model;
using CandidatesBrowser3.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CandidatesBrowser3.ViewModel
{
    public class AddNewCandidateViewModel: INotifyPropertyChanged
    {
        

        private Candidate newCandidate;
        private ICandidateRepository candidateRepository;

        public Candidate NewCandidate
        {
            get { return newCandidate; }
            set {
                newCandidate = value;
                RaisePropertyChange("NewCandidate"); 
                }
        }
        public ObservableCollection<Candidate> CandidatesCollection { get; set; }
        public ICommand SaveCommand { get; set; }

        #region SaveCommand

        private void Save(object obj)
        {
            NewCandidate.IsNew = true;
            if (CandidatesCollection.Where
                    (   e=>e.FirstName.ToLower().Equals(NewCandidate.FirstName.ToLower())
                        && e.LastName.ToLower().Equals(NewCandidate.LastName.ToLower())).Any()                
                    )
            {
                if (MessageBox.Show("You have candidate with same first and last names. Continue anyway?", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
             }

            //int newID = 0;
            try
            {
                NewCandidate.ID = candidateRepository.AddCandidtate(NewCandidate);
                CandidatesCollection.Add(NewCandidate);
                MessengerCandidateCollection.Default.Send<ObservableCollection<Candidate>>(CandidatesCollection);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Adding new candidate failed." + ex.Message, "");

            }




        }
        private bool CanSave(object obj)
        {
            return !string.IsNullOrWhiteSpace(NewCandidate.FirstName) && !string.IsNullOrWhiteSpace(NewCandidate.LastName);
        }

        #endregion

        public AddNewCandidateViewModel(ICandidateRepository candidateRepository)
        {
            this.candidateRepository = candidateRepository;
            CandidatesCollection= candidateRepository.GetCandidates();
            SaveCommand = new CustomCommand(Save, CanSave);
            NewCandidate = new Candidate();

            MessengerCandidateCollection.Default.Register<UpdateListMessageCandidateCollection>(this, OnUpdateListMessageReceived);
        }

        private void OnUpdateListMessageReceived(UpdateListMessageCandidateCollection obj)
        {
            
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
