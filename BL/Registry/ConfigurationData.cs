using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using BL.UtilityClasses;
using Microsoft.Win32;


namespace BL.RegistryConfig
{
    public class ConfigurationData
    {
        public ObservableDictionary<string, Color> TypesColor { get; private set; }
        public String SchemaPath { get; set; }
        private const string pathColorConf = "Bob\\Colors";
        private const string pathSchemaConf = "Bob\\Configuration";
        public enum Regkeys { Choice, ChoiceNull, Element, Error, Sequence, SchemaPath, Search };
        public bool IsErrorLoadingColors { get; private set; }
        public bool IsErrorLoadingSchema { get; private set; }
        private static ConfigurationData instance;

        public static ConfigurationData Instance
        {
            get
            {
                if (instance == default(ConfigurationData))
                    instance = new ConfigurationData();
                return instance;
            }
        }


        private ConfigurationData()
        {
            TypesColor = new ObservableDictionary<string, Color>();
            ReadConfigurationFromRegistry();
        }

        public void SaveConfig()
        {
            GetSetRegisterValue(Regkeys.Choice, ((Color)TypesColor[Regkeys.Choice.ToString()]).ToString(), false);
            GetSetRegisterValue(Regkeys.ChoiceNull, ((Color)TypesColor[Regkeys.ChoiceNull.ToString()]).ToString(), false);
            GetSetRegisterValue(Regkeys.Element, ((Color)TypesColor[Regkeys.Element.ToString()]).ToString(), false);
            GetSetRegisterValue(Regkeys.Error, ((Color)TypesColor[Regkeys.Error.ToString()]).ToString(), false);
            GetSetRegisterValue(Regkeys.Sequence, ((Color)TypesColor[Regkeys.Sequence.ToString()]).ToString(), false);
            GetSetRegisterValue(Regkeys.Search, ((Color)TypesColor[Regkeys.Search.ToString()]).ToString(), false);
            GetSetRegisterValue(Regkeys.SchemaPath, SchemaPath, false);
        }

        public Color GetColorConfiguration(Regkeys item)
        {
            return ((Color)TypesColor[item.ToString()]);
        }

        #region private method

        private void ReadConfigurationFromRegistry()
        {
            //get values for registry
            string choice = GetSetRegisterValue(Regkeys.Choice);
            string choiceNull = GetSetRegisterValue(Regkeys.ChoiceNull);
            string element = GetSetRegisterValue(Regkeys.Element);
            string error = GetSetRegisterValue(Regkeys.Error);
            string sequence = GetSetRegisterValue(Regkeys.Sequence);
            string search = GetSetRegisterValue(Regkeys.Search);
            SchemaPath = GetSetRegisterValue(Regkeys.SchemaPath);

            //check if error occured while reading from the registry
            if ((choice == null) || (choiceNull == null) || (search == null) ||
                (element == null) || (error == null) || (sequence == null))
                IsErrorLoadingColors = true;
            else
                IsErrorLoadingColors = false;

            if (SchemaPath == null)
                IsErrorLoadingSchema = true;
            else
                IsErrorLoadingSchema = false;

            //convert string to color
            Color colorChoice = (Color)ColorConverter.ConvertFromString(choice);
            Color colorChoiceNull = (Color)ColorConverter.ConvertFromString(choiceNull);
            Color colorElement = (Color)ColorConverter.ConvertFromString(element);
            Color colorError = (Color)ColorConverter.ConvertFromString(error);
            Color colorSequence = (Color)ColorConverter.ConvertFromString(sequence);
            Color colorSearch = (Color)ColorConverter.ConvertFromString(search);

            //update list            
            TypesColor["ChoiceNull"] = colorChoiceNull;
            TypesColor["Choice"] = colorChoice;
            TypesColor["Element"] = colorElement;
            TypesColor["Sequence"] = colorSequence;
            TypesColor["Error"] = colorError;
            TypesColor["Search"] = colorSearch;

        }


        private string GetSetRegisterValue(Regkeys key, string value = null, bool isGet = true)
        {
            string subkey = "";
            string path = "";
            switch (key)
            {
                case Regkeys.Choice:
                    subkey = Regkeys.Choice.ToString();
                    path = pathColorConf;
                    break;
                case Regkeys.ChoiceNull:
                    subkey = Regkeys.ChoiceNull.ToString();
                    path = pathColorConf;
                    break;
                case Regkeys.Element:
                    subkey = Regkeys.Element.ToString();
                    path = pathColorConf;
                    break;
                case Regkeys.Error:
                    subkey = Regkeys.Error.ToString();
                    path = pathColorConf;
                    break;
                case Regkeys.Sequence:
                    subkey = Regkeys.Sequence.ToString();
                    path = pathColorConf;
                    break;
                case Regkeys.Search:
                    subkey = Regkeys.Search.ToString();
                    path = pathColorConf;
                    break;
                case Regkeys.SchemaPath:
                    subkey = Regkeys.SchemaPath.ToString();
                    path = pathSchemaConf;
                    break;
            }

            try
            {
                // The name of the key must include a valid root. 
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(path, !isGet);
                if (regKey != null)
                {
                    if (isGet)
                        return (string)regKey.GetValue(subkey);
                    else
                    {
                        regKey.SetValue(subkey, value, RegistryValueKind.String);
                        return null;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                string s = e.Message;
                return null;
            }
        }

        #endregion




    }
}
