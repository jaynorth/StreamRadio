using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Windows;
using System.IO;
using System.Xml;

namespace ViewModelsXML.Tools
{
    public  class ValidateXML
    {

        public ValidateXML()
        {
            schema = new XmlSchemaSet();
            string xsdPath = @"..\..\..\ViewModelsXML\XML\Main\RadioStations.xsd";
            schema.Add("", xsdPath);
        }

        public XmlSchemaSet schema { get; set; }

        public bool validate(string xmlPath, bool showSucceedMessage)
        {

            bool Validated = true;
            
           
            try
            {
                string Filename = Path.GetFileName(xmlPath);
                XDocument doc = XDocument.Load(xmlPath);
                doc.Validate(schema, (s, e) =>
                {

                    //MessageBox.Show(Filename + " is not valid: " + e.Message);
                    MessageBox.Show(Filename + ": XSD validation failed\n" +e.Message);

                    Validated = false;
                });

                if (Validated == false)
                {
                  //  MessageBox.Show(Filename + ": XSD validation failed" );
                }
                else if (showSucceedMessage)
                {
                    MessageBox.Show(Filename + ": XSD validation Succeeded");
                }
            }
            catch (Exception e)
            {
                
                string fileName = Path.GetFileName(xmlPath);
                MessageBox.Show("Exception with "+fileName+" : " + e.Message +"\nValidation Failed!");
                Validated = false;
            }

                return Validated;

        }

        public bool validate(XDocument doc, bool showSucceedMessage)
        {

            bool Validated = true;


            try
            {
          
                doc.Validate(schema, (s, e) =>
                {

                    //MessageBox.Show(Filename + " is not valid: " + e.Message);

                    Validated = false;
                });

                if (Validated == false)
                {
                    MessageBox.Show("XSD validation failed");
                }
                else if (showSucceedMessage)
                {
                    MessageBox.Show( "XSD validation Succeeded");
                }
            }
            catch (Exception e)
            {

                
                MessageBox.Show("Exception: "  + e.Message + "\nValidation Failed!");
                Validated = false;
            }

            return Validated;

        }

    }
}
