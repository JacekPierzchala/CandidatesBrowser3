using CandidatesBrowser3.DAL;
using CandidatesBrowser3.Model;
using CandidatesBrowser3.Utilities;
using CommonUlitlities;

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
using System.Threading;

namespace CandidatesBrowser3.ViewModel
{
    public class ProjectsReportViewModel: INotifyPropertyChanged
    {
        #region fields
        public static string[] Columns =
           {
                "FirstName",
                "LastName",
                "Position",
                "CompanyName",
                "FirstEmail",
                "FirstPhone",
                "StatusName",
                "Comments"
            };

        private IConfigProjectRepository configProjectRepository;
        private ICandidateHistoryRepository candidateHistoryRepository;
        private IDialogService dialogService;
        private ConfigProjectMessenger configProjectMessenger;
        #endregion

        #region commands
        public ICommand ProjectSelectionChangeCommand { get; set; }
        public ICommand ExportToFileCommand { get; set; }
        public ICommand ReadJDCommand { get; set; }
        public ICommand AddJDCommand { get; set; }
        public ICommand DeleteJDCommand { get; set; }
        #endregion


        #region collections
        private ObservableCollection<CandidateHistory> candidateHistoryCollection;
        public ObservableCollection<CandidateHistory> CandidateHistoryCollection
        {
            get { return candidateHistoryCollection; }
            set
            {
                candidateHistoryCollection = value;
                RaisePropertyChange("CandidateHistoryCollection");
            }
        }

        private List<Attachment> attachments;
        public List<Attachment> Attachments
        {
            get { return attachments; }
            set
            {
                attachments = value;
                RaisePropertyChange("Attachments");
            }
        }

        private ObservableCollection<ConfigProject> configProjectCollection;
        public ObservableCollection<ConfigProject> ConfigProjectCollection
        {
            get { return configProjectCollection; }
            set
            {
                configProjectCollection = value;
                RaisePropertyChange("ConfigProjectCollection");
            }
        }
        #endregion


        #region properties
        private ConfigProject selectedProject;
        public ConfigProject SelectedProject
        {
            get { return selectedProject; }
            set
            {
                selectedProject = value;
                RaisePropertyChange("SelectedProject");
            }
        }


        public string DestinationDirectory
        {
            get
            {
                return Path.Combine(ConfigProject.FolderPath, SelectedProject.ID.ToString());

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
        #endregion


        #region ReadJDCommand2
        private void ReadJD(object o)
        {
            string[] files = null;
            try
            {
                files = Directory.GetFiles(ConfigProject.FolderPath + SelectedProject.ID.ToString() + @"\");

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

        private bool CanReadJD(object o)
        {
            return SelectedProject!=null && SelectedProject.JdUploaded == true;           
        }

        #endregion

        #region AddJDCommand
        private void AddJD(object o)
        {
            DocumentToAction.Action = ActionType.Save;
            MessengerCandidate.Default.Send<Document>(DocumentToAction);

            dialogService.ShowDetailDialog();

            if (DocumentToAction.DocumentNames != null && DocumentToAction.DocumentNames.Count() > 0)
            {
                if (GlobalFunctions.SaveFile(DocumentToAction.DocumentNames, SelectedProject.ID.ToString() ,DestinationDirectory,Attachments))
                {
                    SelectedProject.JdUploaded = true;
                }
                else
                {
                    return;
                }

                configProjectRepository.UpdateConfigProjectDocumentInfo(SelectedProject);
               
                    //,candidateRepository.UpdateCandidateDocumentInfo(SelectedCandidate);
               
            }
        }

        private bool CanAddJD(object o)
        {
            return SelectedProject != null; ;

        }

        #endregion

        #region DeleteJobDescriptionCommand
        private void DeleteJobDescription(object o)
        {
            DocumentToAction.Action = ActionType.Delete;
            DocumentToAction.FolderPath = DestinationDirectory;
            MessengerCandidate.Default.Send<Document>(DocumentToAction);

            dialogService.ShowDetailDialog();

            if (DocumentToAction.DocumentNames != null && DocumentToAction.DocumentNames.Count() > 0)
            {
                if (GlobalFunctions.DeleteFile(DocumentToAction.DocumentNames, Attachments) && Attachments.Count == 0)
                {
                    SelectedProject.JdUploaded = false;
                }
                else
                {
                    return;
                }

                configProjectRepository.UpdateConfigProjectDocumentInfo(SelectedProject);


            }

        }

        private bool CanDeleteJobDescription(object o)
        {
            return SelectedProject != null && SelectedProject.JdUploaded;
        }

        #endregion

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
                    File.Copy(sourceFilePath, DestinationDirectory + @"\" + fileName);


                    MessageBox.Show("File attached succesfully ", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    result = true;
                    Attachments.Add(new Attachment(DestinationDirectory + @"\" + fileName));

                }
                catch (Exception ex)
                {
                    MessageBox.Show("File was not attached! " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                    // result =false;
                }
            }

            return result;




        }

        #region ProjectSelectionChangeCommand
        private bool CanProjectSelectionChange(object obj)
        {
            if (SelectedProject !=null)
            {
                return true;
            }
            return false;
        }

        private void ProjectSelectionChange(object obj)
        {
            CandidateHistoryCollection = candidateHistoryRepository.LoadHistorysByProjectID(SelectedProject.ID);
            Attachments = new List<Attachment>();
            loadAttachments();

        }
        #endregion

        #region ExportToFileCommand 
        private bool CanExportToFile(object obj)
        {
            return (obj != null && ((ObservableCollection<CandidateHistory>)obj).Count > 0);
        
        }

        private void ExportToFile(object obj)
        {
            new Thread(() =>
            {
                //Thread.CurrentThread.IsBackground = true;

                GlobalFunctions.ExportToExcel(GlobalFunctions.ToDataTableFromList(CandidateHistoryCollection.ToList(),Columns),SelectedProject.ProjectName);

            }).Start();
           
           
           // CandidateHistoryCollection = candidateHistoryRepository.LoadHistorysByProjectID(SelectedProject.ID);
        }

        #endregion

        public ProjectsReportViewModel(IConfigProjectRepository configProjectRepository, ICandidateHistoryRepository candidateHistoryRepository,
            IDialogService dialogService)
        {
            this.configProjectRepository = configProjectRepository;
            this.candidateHistoryRepository = candidateHistoryRepository;
            this.dialogService = dialogService;
            //MessengerConfigProject.Default.Register<ConfigProject>(this, OnConfigProjectReceived);
            configProjectMessenger = ConfigProjectMessenger.Instance;
            configProjectMessenger.ConfigProjectChanged += OnConfigProjectChanged;
            DocumentToAction = new Document();
            loadData();
            commandsInitialize();
        }

        private void OnConfigProjectChanged(object sender, ConfigProjectValueChangedEventArgs e)
        {
            var receivedConfigProject = e.ConfigProject;
            if (!ConfigProjectCollection.Any(cp => cp.Id == receivedConfigProject.ConfigProjectLibID))
            {
                ConfigProjectCollection.Add(receivedConfigProject);
            }
        }

        private void OnConfigProjectReceived(ConfigProject obj)
        {
            loadData();
        }

        private void commandsInitialize()
        {
            ProjectSelectionChangeCommand = new CustomCommand(ProjectSelectionChange, CanProjectSelectionChange);
            ExportToFileCommand = new CustomCommand(ExportToFile, CanExportToFile);
            ReadJDCommand = new CustomCommand(ReadJD, CanReadJD);
            AddJDCommand = new CustomCommand(AddJD, CanAddJD);
            DeleteJDCommand = new CustomCommand(DeleteJobDescription, CanDeleteJobDescription);
        }

      

        private void loadData()
        {
            ConfigProjectCollection = configProjectRepository.GetConfigProjects().OrderBy(e=>e.ProjectName).ToObservableCollection();
         
        }

        private void loadAttachments()
        {
            if (SelectedProject.JdUploaded)
            {
                string[] files = Directory.GetFiles(DestinationDirectory);

                foreach (string file in files)
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
