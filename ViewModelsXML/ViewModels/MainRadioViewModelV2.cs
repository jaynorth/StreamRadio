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
using ViewModelsXML.ViewModels.Helpers;

namespace ViewModelsXML.ViewModels
{
    public  class MainRadioViewModelV2 : BaseViewModel
    {
        public MainRadioViewModelV2()
        {
            LoadXML();
            _currentStation = StationList.FirstOrDefault();
            Browse = new RelayCommand(BrowseForXML);
            Save = new RelayCommand(SaveListToXML);
            ToBeProcessed = new RelayCommand(ProcessFolderToXML);
            CleanList = new RelayCommand(CleanMainList);
        
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
                    
        public RadioStation    CurrentStation
        {
            get { return _currentStation; }
            set {

                _currentStation = value;
                OnPropertyChanged();

            }
        }

        public RelayCommand Browse { get; set; }
        public RelayCommand Save { get; set; }
        public RelayCommand ToBeProcessed { get; set; }
        public RelayCommand CleanList { get; set; }

        public ObservableCollection<RadioStation> RadioList { get; set; }


        private void LoadXML()
        {
            string path = @"D:\Visual Studio Projects\Projects\RetroWebRadio\ViewModelsXML\XML\RadioStations.xml";
            XDocument doc = XDocument.Load(path);

            List<RadioStation> list = (from station in doc.Descendants("station")
                                       select new RadioStation()
                                       {
                                           Id = (int)station.Attribute("id"),
                                           Name = (string)station.Element("name"),
                                           Category = (string)station.Element("category"),
                                           Country = (string)station.Element("country"),
                                           Url = (string)station.Element("url")

                                       }).ToList();
            int Id = 10;

            foreach (var item in list)
            {
                item.Id = Id;
                Id++;
            }

            StationList = new ObservableCollection<RadioStation>(list);
        }


        private void ProcessFolderToXML()
        {
            string dirPath = @"D:\Visual Studio Projects\Projects\RetroWebRadio\ViewModelsXML\XML\ToBeProcessed";

            if (Directory.Exists(dirPath))
            {
                ProcessFilesInDirectory(dirPath);
            }
            else
            {
                MessageBox.Show("No XML Files in this Directory: " + dirPath);
            }
        }

        private void ProcessFilesInDirectory(string directoryPath)
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


            string xmlString;
            string destinationFile;
            string destinationDir = @"D:\Visual Studio Projects\Projects\RetroWebRadio\ViewModelsXML\XML\Processed";
            foreach (var item in fileEntries)
            {
                //convert file to string
                xmlString = File.ReadAllText(item);

                //Process to List and update
                XMLtolist(xmlString);

                //Move file to Processed Folder
                //GetsFileName
                fileName = Path.GetFileName(item);
                // Processed Folder + filename
                destinationFile = destinationDir + @"\" + fileName;
                //Move File
                File.Move(item, destinationFile);

                //Clear String (optional)
                xmlString = "";

            }



        }

        private void BrowseForXML()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"D:\Visual Studio Projects\Projects\RetroWebRadio\ViewModelsXML\XML\";
            openFileDialog.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string fileText = File.ReadAllText(openFileDialog.FileName);
                MessageBox.Show("xml document copied");
                XMLtolist(fileText);
            }
            else
            {
                MessageBox.Show("failed opening");
            }
        }
        

        private void XMLtolist(string xmlFileText)
        {

            XDocument doc = XDocument.Parse(xmlFileText);

            List<RadioStation> newlist = (from station in doc.Descendants("station")
                                          select new RadioStation()
                                          {
                                              //  Id = (int)station.Element("id"),
                                              Name = (string)station.Element("name"),
                                              Category = (string)station.Element("category"),
                                              Country = (string)station.Element("country"),
                                              Url = (string)station.Element("url")

                                          }).ToList();
            RadioList = new ObservableCollection<RadioStation>(newlist);

            //find existing top Id in Main List

            //var topId = MainRadioViewModel.StationList.Max(t => t.Id);
            var topId = StationList.Max(t => t.Id);

            //This will update the list that displays the stations on the main window
            foreach (var item in RadioList)
            {
                item.Id = topId + 1;
                //MainRadioViewModel.StationList.Add(item);
                StationList.Add(item);
                topId++;
            }





        }
        private void CreateXmlFromList(ObservableCollection<RadioStation> MainRadiolist)
        {
            XDocument Xmldoc = new XDocument(

                new XDeclaration("1.0", "utf-8", "yes"),

                new XComment("Main List of Radio Stations XML"),

                new XElement("stations",

                    from station in MainRadiolist
                    select new XElement("station", new XAttribute("id", station.Id),
                            new XElement("url", station.Url),
                            new XElement("name", station.Name),
                            new XElement("category", station.Category),
                            new XElement("country", station.Country))


                ));

            Xmldoc.Save(@"D:\Visual Studio Projects\Projects\RetroWebRadio\ViewModelsXML\XML\RadioStations.xml");
        }

        private void SaveListToXML()
        {
            CleanMainList();
            CreateXmlFromList(StationList);
            MessageBox.Show("XML File created and saved");
        }

        private void CleanMainList()
        {
            var query = StationList.GroupBy(x => x.Url.ToUpper()).Select(y => y.Last()).ToList();

          StationList = new ObservableCollection<RadioStation>(query);

            MessageBox.Show("Main List cleaned");

        }

    

}
}
