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
            //openFileDialog.Filter = "Word files(*.doc)|*.doc|PDF files(*.pdf)|*.pdf";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.ShowDialog();

            DocumentToAction.DcoumentName = openFileDialog.FileName.ToString();
            //OpenFileDialogService.Filter = Filter;
            //OpenFileDialogService.FilterIndex = FilterIndex;
            //DialogResult = OpenFileDialogService.ShowDialog();
            //if (!DialogResult)
            //{
            //    ResultFileName = string.Empty;
            //}
            //else
            //{
            //    IFileInfo file = OpenFileDialogService.Files.First();
            //    ResultFileName = file.Name;
            //    using (var stream = file.OpenText())
            //    {
            //        FileBody = stream.ReadToEnd();
            //    }
            //}
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
            //Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            //FilterIndex = 1;
            //Title = "Custom Dialog Title";
            //DefaultExt = "txt";
            //DefaultFileName = "Document1";
            //OverwritePrompt = true;
            Utilities.MessengerCandidate.Default.Register<Document>(this, OnDocumentReceived);
            OpenCommand = new CustomCommand(Open, CanOpen);
        }

        private void OnDocumentReceived(Document obj)
        {
            DocumentToAction = obj;
        }

        public void Save()
        { 
        //{
        //    SaveFileDialogService.DefaultExt = DefaultExt;
        //    SaveFileDialogService.DefaultFileName = DefaultFileName;
        //    SaveFileDialogService.Filter = Filter;
        //    SaveFileDialogService.FilterIndex = FilterIndex;
        //    DialogResult = SaveFileDialogService.ShowDialog();
        //    if (!DialogResult)
        //    {
        //        ResultFileName = string.Empty;
        //    }
        //    else
        //    {
        //        using (var stream = new StreamWriter(SaveFileDialogService.OpenFile()))
        //        {
        //            stream.Write(FileBody);
        //        }
        //    }
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
