﻿using CandidatesBrowser3.DAL;
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
using System.IO;
using System.Windows;
using Prism.Interactivity.InteractionRequest;

namespace CandidatesBrowser3.ViewModel
{
    public class CandidateDetailsViewModel : INotifyPropertyChanged
    {
        #region fields
        private ICandidateHistoryRepository candidateHistoryRepository;
        private ICandidateRepository candidateRepository;
        private IDialogService dialogService;
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

        private List<Attachment> attachments;
        public List<Attachment> Attachments
        {
            get { return attachments; }
            set {
                attachments = value;
                RaisePropertyChange("Attachments");
            }
        }

        public string DestinationDirectory
        {
            get
            {
                return Path.Combine(Candidate.FolderPath, SelectedCandidateTemp.ID.ToString());
                    
            }

        }

        private Document documentToAction;
        public Document DocumentToAction
        {
            get { return documentToAction; }
            set
            {
                documentToAction = value;
                RaisePropertyChange("DocumentToAction");
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

        //public List<WPFMenuItem> OrderMenuOptions
        //{
        //    get
        //    {
        //        return CreateMenus();
        //    }
        //}
        //private List<WPFMenuItem> CreateMenus()
        //{
        //    var menu = new List<WPFMenuItem>();

        //    var miAddCV = new WPFMenuItem("Add new CV");
        //    miAddCV.IconUrl = @"\Resources\clip.ico";
        //    miAddCV.Command = ReadCVCommand;
        //    menu.Add(miAddCV);

        //    var miReadCV = new WPFMenuItem("Read CV's");
        //    miReadCV.IconUrl = @"\Resources\magnifying_glass.ico";
        //    miReadCV.Command = ReadCVCommand;
        //    menu.Add(miReadCV);

        //    var miDeleteCV = new WPFMenuItem("Delete CV");
        //    miDeleteCV.IconUrl = @"\Resources\removeIcon.ico";
        //    miDeleteCV.Command = ReadCVCommand;
        //    menu.Add(miDeleteCV);

        //    return menu;

        //}

        //private ICommand openDialogCommand = null;
        //public ICommand OpenDialogCommand
        //{
        //    get { return this.openDialogCommand; }
        //    set { this.openDialogCommand = value; }
        //}

        // public InteractionRequest<IConfirmation> OpenFileRequest { get; private set; }

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
        public ICommand AddCVCommand { get; set; }
        public ICommand ReadCVCommand { get; set; }
        public ICommand DeleteCVCommand { get; set; }
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

        #region ReadCVCommand
        private void ReadCV(object o)
        {
            string[] files = null;
            try
            {
                files = Directory.GetFiles(Candidate.FolderPath + SelectedCandidateTemp.ID.ToString() + @"\");

            }
            catch (Exception ex)
            {
                MessageBox.Show("requested folder as not found " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach (string file in files.Where(e => !e.Contains("~")).ToList())
            {
                try
                {
                    System.Diagnostics.Process.Start(file);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("requested file " + Path.GetFileName(file) + " cannot be open" + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanReadCV(object o)
        {
            if (SelectedCandidateTemp.CvUploaded)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region AddCVCommand
        private void AddCV(object o)
        {
            DocumentToAction.Action = ActionType.Save;           
            MessengerCandidate.Default.Send<Document>(DocumentToAction);

            dialogService.ShowDetailDialog();

            if (DocumentToAction.DocumentNames != null)
            {
                if (saveFile(DocumentToAction.DocumentNames, SelectedCandidate.ID.ToString()))
                {
                    SelectedCandidateTemp.CvUploaded = true;
                }
                else
                {
                    return;
                }

                if (SelectedCandidateTemp.CvUploaded != SelectedCandidate.CvUploaded)
                {
                    SelectedCandidate.CvUploaded = true;
                    candidateRepository.UpdateCandidateDocumentInfo(SelectedCandidate);
                }
            }
        }

        private bool CanAddCV(object o)
        {
            return true;

        }

        #endregion

        #region DeleteCVCommand
        private void DeleteCV(object o)
        {
            DocumentToAction.Action = ActionType.Delete;
            DocumentToAction.FolderPath = DestinationDirectory;
            MessengerCandidate.Default.Send<Document>(DocumentToAction);

            dialogService.ShowDetailDialog();

            if (DocumentToAction.DocumentNames != null)
            {
                if (deleteFile(DocumentToAction.DocumentNames, Attachments) && Attachments.Count==0)
                {
                    SelectedCandidateTemp.CvUploaded = false;
                }
                else
                {
                    return;
                }
                if (SelectedCandidateTemp.CvUploaded != SelectedCandidate.CvUploaded)
                {
                    SelectedCandidate.CvUploaded = false;
                    candidateRepository.UpdateCandidateDocumentInfo(SelectedCandidate);

                }
            }

        }

        private bool CanDeleteCV(object o)
        {
            if (SelectedCandidate.CvUploaded) { return true; }
            return false;

        }

        #endregion

        //  private void OnShowConfirmation()
        //{
        //    var confirmation = new Confirmation();
        //    confirmation.Title = "Really?";
        //    confirmation.Content = "Here goes the question, doesn't it?";

        //    OpenFileRequest.Raise(confirmation);
        //}
        #endregion

        public CandidateDetailsViewModel(ICandidateHistoryRepository candidateHistoryRepository, IDialogService dialogService,
            ICandidateRepository candidateRepository)
        {
            this.candidateHistoryRepository = candidateHistoryRepository;
            this.dialogService = dialogService;
            this.candidateRepository = candidateRepository;

            SelectedCandidateTemp = new Candidate();
            try
            {
                DocumentToAction = new Document();

                MessengerDocument.Default.Register<UpdateDocument>(this, OnUpdateDocumentMessageReceived);

                MessengerCandidate.Default.Register<Candidate>(this, OnCandidateReceived);

            }

            catch (Exception x)
            {

            }



            loadCommands();
        }

        private void OnUpdateDocumentMessageReceived(UpdateDocument Obj)
        {
            dialogService.CloseDetailDialog();

        }

        private bool saveFile(string[] sourceFilePaths, string id)
        {
            var result = false;
            foreach (string sourceFilePath in sourceFilePaths)
            {
                string fileName = System.IO.Path.GetFileName(sourceFilePath);
               
                if (!Directory.Exists(DestinationDirectory))
                {
                    Directory.CreateDirectory(DestinationDirectory);
                }

                try
                {
                    File.Copy(sourceFilePath, DestinationDirectory + fileName);


                    MessageBox.Show("File attached succesfully ", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    result = true;
                    Attachments.Add(new Attachment(DestinationDirectory + fileName));

                }
                catch (Exception ex)
                {
                    MessageBox.Show("File was not attached! " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                    // result =false;
                }
            }

            return result;




        }


        private bool deleteFile(string[] sourceFilePaths, List<Attachment> attachmentsList)
        {
            var result = false;
            foreach (string sourceFilePath in sourceFilePaths)
            {
                try
                {
                    File.Delete(sourceFilePath);
                    MessageBox.Show("File deleted succesfully ", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    result = true;                   
                    Attachments.Remove(Attachments.Where(e => e.Path.Equals(sourceFilePath)).FirstOrDefault());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("File was not deleted! " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return result;
        }

        public void OnCandidateReceived(Candidate selectedCandidate)
        {
            SelectedProjectHistory = null;
            SelectedCandidate = selectedCandidate;

            GlobalFunctions.CopyProperties(SelectedCandidate, SelectedCandidateTemp);
            Attachments = new List<Attachment>();
            loadData();
            prepareCollection();
            loadAttachments();
        }

        private void prepareCollection()
        {
            CandidateHistoryCollectionLastStatus = CandidateHistoryCollection.ToList().GroupBy(e => e.ProjectID).Select(s => s.OrderByDescending(i => i.Seq).First()).ToList().ToObservableCollection();
        }

        private void loadData()
        {
            if (SelectedCandidate != null)
            {
                CandidateHistoryCollection = candidateHistoryRepository.GetCandidateHistorysByID(SelectedCandidate.ID);
            }

        }

        private void loadCommands()
        {
            ProjectSelectionChangeCommand = new CustomCommand(ProjectSelectionChange, CanProjectSelectionChange);
            ReadCVCommand = new CustomCommand(ReadCV, CanReadCV);
            AddCVCommand = new CustomCommand(AddCV, CanAddCV);
            DeleteCVCommand = new CustomCommand(DeleteCV, CanDeleteCV);
            // OpenFileRequest = new InteractionRequest<IConfirmation>();
        }

        private void loadAttachments()
        {
            if(SelectedCandidateTemp.CvUploaded)
            {
                string[] files=Directory.GetFiles(DestinationDirectory);
               
                foreach(string file in files)
                {
                    Attachments.Add(new Attachment(file));
                }
                

            }
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
