using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using ViewModelsXML.Tools;
using ViewModelsXML.ViewModels.Helpers;

namespace ViewModelsXML.ViewModels
{
    public class MainRadioViewModel : BaseViewModel
    {
        public MainRadioViewModel()
        {
  
            LoadXmlData();
            _currentStation = StationList.FirstOrDefault();
            
            //Application Folder containing XML Files waiting to be processed
            AppXmlFolderToBeProcessed = @"..\..\..\ViewModelsXML\XML\ToBeProcessed";
            GetFileListInApplicationFolder(AppXmlFolderToBeProcessed);


            //Show Message Box when XSD validation succeeded
            IsShowValidationSucceeded = false;

            _droppedItems = new ObservableCollection<string>();
            InitCommands();

           

        }

        private void InitCommands()
        {
            Import = new RelayCommand(AddDroppedXml);
            Browse = new RelayCommand(BrowseXml);
            Save = new RelayCommand(SaveListToXML);
            ToBeProcessed = new RelayCommand(ProcessFolderToXML);
            Remove = new RelayCommand(RemoveStation);
            Search = new RelayCommand(LoadListView);
        }

        public RelayCommand Import { get; set; }

        public RelayCommand Remove { get; set; }
        public RelayCommand Browse { get; set; }
        public RelayCommand Save { get; set; }

        public RelayCommand Search { get; set; }
        public RelayCommand ToBeProcessed { get; set; }

        public string searchstring { get; set; } = "search here";

        public ObservableCollection<RadioStation> UnfilteredList { get; set; }

        public bool IsShowValidationSucceeded { get; set; }

        public void DoFileDrop(IEnumerable<String> filePaths)
        {
            

            foreach (var item in filePaths)
            {
                if (Path.GetExtension(item).ToUpper() == ".XML")
                {
                    DroppedItems.Add(item);
                }

                else
                {
                    MessageBox.Show(item + " is not an XML file");
                }
            }

        }


        private ObservableCollection<string> _droppedItems;

        public ObservableCollection<string> DroppedItems
        {
            get { return _droppedItems; }
            set
            {
                _droppedItems = value;

                OnPropertyChanged();
            }
        }

        private ObservableCollection<RadioStation> _stationList;

        public ObservableCollection<RadioStation> StationList
        {
            get { return _stationList; }
            set
            {
                _stationList = value;
                OnPropertyChanged();
            }
        }

        private RadioStation _currentStation;

        public RadioStation CurrentStation
        {
            get { return _currentStation; }
            set
            {


                _currentStation = value;
                
                OnPropertyChanged();


            }
        }


       



        public string AppXmlFolderToBeProcessed { get; set; }


        private void LoadXmlData()
        {

            //xmlValidation.validate(path, false);

            RadioStationRepository rsRep = new RadioStationRepository();

            StationList = rsRep.GetStations(doc);
            UnfilteredList = StationList;

        }

     

        private void ProcessFolderToXML()
        {
 
            if (Directory.Exists(AppXmlFolderToBeProcessed))
            {
                PocessXmlFolder(AppXmlFolderToBeProcessed);
            }
            else
            {
                MessageBox.Show("Directory does not exist: " + AppXmlFolderToBeProcessed);
            }
        }

        private ObservableCollection<string> GetFileListInApplicationFolder(string dirPath)
        {
            _fileList = new ObservableCollection<string>();
            string[] fileEntries = Directory.GetFiles(dirPath, "*.xml");
            string fileName;

            foreach (var item in fileEntries)
            {
                fileName = Path.GetFileName(item);
                _fileList.Add(fileName);
            }

            return _fileList;
        }

        private ObservableCollection<string> _fileList;

        public ObservableCollection<string> FileList
        {
            get { return _fileList; }
            set
            {
                _fileList = value;

                OnPropertyChanged();

            }
        }

        private void AddDroppedXml()
        {
            
            foreach (var item in _droppedItems)
            {
       
                AddXml(item);
            }

            DroppedItems.Clear();
        }

        private void PocessXmlFolder(string directoryPath)
        {
            string[] fileEntries = Directory.GetFiles(directoryPath, "*.xml");

            string fileName;
            string s = "";
            int i = 1;

            foreach (var item in fileEntries)
            {
                fileName = Path.GetFileName(item);
                s += i.ToString() + ") " + fileName + '\n';
                i++;
            }

            MessageBox.Show("The following " + fileEntries.Count() + " files will be processed :" + '\n' + s);

            string destinationFile;
            string destinationDir = @"..\..\..\ViewModelsXML\XML\Processed";
            

            foreach (var item in fileEntries)
            {
               
                    AddXml(item);

                    //Move file to Processed Folder
                    //GetsFileName
                    fileName = Path.GetFileName(item);
                    //Rename File and append TimeStamp 
                    fileName = TimeStamp.AppendTimeStamp(fileName);
                    // Move File to Processed Folder 
                    destinationFile = destinationDir + @"\" + fileName;
                    File.Move(item, destinationFile);

                    //Update FileList
                    _fileList.Remove(fileName);
            }

            GetFileListInApplicationFolder(AppXmlFolderToBeProcessed);

        }

        private void BrowseXml()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = (openFileDialog.FileName);
                AddXml(filePath);
            }
            else
            {
                MessageBox.Show("no file was opened");
            }
        }


        private void AddXml(string filePath)
        {
            ValidateXML xmlValidate = new ValidateXML();

            if (xmlValidate.validate(filePath, IsShowValidationSucceeded))
            {
                XDocument NewDoc = XDocument.Load(filePath);

                RadioStationRepository rsRep = new RadioStationRepository();
                rsRep.AddStations(NewDoc, StationList);
                StationList = rsRep.CleanMainList(StationList);
            }
            
        }

        private void RemoveStation()
        {
            RadioStationRepository rsRep = new RadioStationRepository();
            StationList= rsRep.RemoveStation(CurrentStation, StationList);
            UnfilteredList = StationList;
        }

        private void SaveListToXML()
        {
            RadioStationRepository rsRep = new RadioStationRepository();
            StationList = UnfilteredList;
            StationList = rsRep.CleanMainList(StationList);
            XDocument doc =  rsRep.CreateNewXML(StationList);
            ValidateXML xmlValidate = new ValidateXML();
            if (xmlValidate.validate(doc, IsShowValidationSucceeded))
            {
                rsRep.SaveXML(doc);
                MessageBox.Show("XML File re-created and saved");
            }
            else
            {
                MessageBox.Show("XML not saved, error with validation");
            }
            
            
        }

        public void LoadListView()
        {
            if (UnfilteredList == null)
            {
                UnfilteredList = this.StationList;
            }
            else
            {
                this.StationList = UnfilteredList;
            }

            IEnumerable<RadioStation> StationList = this.StationList;
            string searchString = this.searchstring;

            IEnumerable<RadioStation> query;

            query =
            (from station in StationList
             where station.Name.ToUpper().Contains(searchString.ToUpper())

            || station.Country.ToUpper().Contains(searchString.ToUpper())
            || station.Category.ToUpper().Contains(searchString.ToUpper())

             orderby station.Name
             select station);


            this.StationList = new ObservableCollection<RadioStation>(query);
        }





    }
}
