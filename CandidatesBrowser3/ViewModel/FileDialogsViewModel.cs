using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Input;
using CandidatesBrowser3.Utilities;
using CandidatesBrowser3.Model;
using System.ComponentModel;
using Microsoft.Win32;

namespace CandidatesBrowser3.ViewModel
{

  
    public class FileDialogsViewModel
    {
        #region Common properties
        public virtual string Filter { get; set; }
        public virtual int FilterIndex { get; set; }
        public virtual string Title { get; set; }
        public virtual bool DialogResult { get; protected set; }
        public virtual string ResultFileName { get; protected set; }
        public virtual string FileBody { get; set; }
        private Utilities.IDialogService dialogService;

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

        #region commands
        public ICommand OpenCommand { get; set; }

        #endregion

        #region methods for commands

        #region OpenCommand
        private void Open(object o)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
   
            if (DocumentToAction.Action == ActionType.Delete)
            {
                openFileDialog.InitialDirectory =  DocumentToAction.FolderPath;
            }
            else 
            {
                openFileDialog.InitialDirectory =Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            
            if (DocumentToAction.Action != ActionType.Import)
            {
                openFileDialog.Multiselect = true;
            }
            else { openFileDialog.Multiselect = false; }

            openFileDialog.ShowDialog();

            DocumentToAction.DocumentNames = openFileDialog.FileNames;

            MessengerDocument.Default.Send<UpdateDocument>(new UpdateDocument());
        }

        private bool CanOpen(object o)
        {
            return true;
        }

        #endregion


        #endregion


        #region SaveFileDialogService specific properties
        public virtual string DefaultExt { get; set; }
        public virtual string DefaultFileName { get; set; }
        public virtual bool OverwritePrompt { get; set; }
        #endregion

        //protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }
        //protected IOpenFileDialogService OpenFileDialogService { get { return this.GetService<IOpenFileDialogService>(); } }

       

        public FileDialogsViewModel(Utilities.IDialogService dialogService)
        {
            this.dialogService = dialogService;
           
            Utilities.MessengerCandidate.Default.Register<Document>(this, OnDocumentReceived);
            OpenCommand = new CustomCommand(Open, CanOpen);
        }

        private void OnDocumentReceived(Document obj)
        {
            DocumentToAction = obj;
        }

        public void Save()
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
