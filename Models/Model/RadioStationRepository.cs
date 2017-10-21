using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Models.Model
{
   public class RadioStationRepository
    {
        public RadioStationRepository()
        {
            
            list = new List<RadioStation>();
            StationList = new ObservableCollection<RadioStation>();
        }

        public ObservableCollection<RadioStation> StationList { get; set; }
        public List<RadioStation> list { get; set; }
        public ObservableCollection<RadioStation> GetStations(XDocument doc)
        {
           var list = (from station in doc.Descendants("station")
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
                TrimItems(item);
                FillEmptyString(item);
                Id++;
            }

            StationList = new ObservableCollection<RadioStation>(list);

            return StationList;
            
        }

        public void AddStations(XDocument doc, ObservableCollection<RadioStation> StationList)
        {
            List<RadioStation> newlist = (from station in doc.Descendants("station")
                                          select new RadioStation()
                                          {
                          
                                              Name = (string)station.Element("name") ,
                                              Category = (string)station.Element("category"),
                                              Country = (string)station.Element("country"),
                                              Url = (string)station.Element("url")

                                          }).ToList();

            //find existing top Id in Main List

            //var topId = MainRadioViewModel.StationList.Max(t => t.Id);
            var topId = StationList.Max(t => t.Id);

            //This will update the list that displays the stations on the main window
            foreach (var item in newlist)
            {
                item.Id = topId + 1;
                TrimItems(item);

                if (string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.Url) || item.Url.Length < 8)
                {
                    continue;
                }
                FillEmptyString(item);
                topId++;
                StationList.Add(item);

            }
        }

        private static void FillEmptyString(RadioStation item)
        {
            if (string.IsNullOrEmpty(item.Category))
            {
                item.Category = "--";
            }

            if (string.IsNullOrEmpty(item.Country))
            {
                item.Country = "--";
            }
        }

        private static void TrimItems(RadioStation item)
        {
            try
            {
                item.Name = item.Name.Trim();
                item.Url = item.Url.Trim();
                item.Category = item.Category.Trim();
                item.Country = item.Country.Trim();
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
   
        }

        public ObservableCollection<RadioStation> RemoveStation(RadioStation CurrentStation,  ObservableCollection<RadioStation> StationList)
        {
            RadioStation removeStation = CurrentStation;

            CurrentStation = StationList.First();

            StationList.Remove(removeStation);
            
            return StationList;
        }

        public XDocument CreateNewXML(ObservableCollection<RadioStation> MainRadiolist)
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

            return Xmldoc;
          
      
        }

        public ObservableCollection<RadioStation> CleanMainList(ObservableCollection<RadioStation> StationList)
        {
            var query = StationList.GroupBy(x => x.Url.ToUpper()).Select(y => y.First()).ToList();

            StationList = new ObservableCollection<RadioStation>(query);

            foreach (var item in StationList)
            {
                FillEmptyString(item);
            }

            MessageBox.Show("Main List cleaned");

            return StationList;

        }

        public void SaveXML(XDocument doc)
        {
            doc.Save(@"..\..\..\ViewModelsXML\XML\Main\RadioStations.xml");
        }
    }

}
