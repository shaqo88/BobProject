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
    /// <summary>
    /// ConfigurationData - Class that represent the configuration of the softwatre.
    /// the data stored in the registry.
    /// </summary>
    public class ConfigurationData
    {
        #region enum

        public enum Regkeys { Choice, ChoiceNull, Element, Error, Sequence, SchemaPath, Search };

        #endregion

        #region private members

        //path of configuraion in the registry
        private const string pathColorConf  = "Bob\\Colors";
        private const string pathSchemaConf = "Bob\\Configuration";
        //singleton instance class
        private static ConfigurationData instance;

        #endregion

        #region Properties

        public ObservableDictionary<string, Color> TypesColor { get; private set; }

        public String SchemaPath { get; set; }                

        public bool IsErrorLoadingColors { get; private set; }

        public bool IsErrorLoadingSchema { get; private set; }

        public static ConfigurationData Instance
        {
            get
            {
                if (instance == default(ConfigurationData))
                    instance = new ConfigurationData();
                return instance;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Singleton Design - private C'tor.
        /// </summary>
        private ConfigurationData()
        {
            TypesColor = new ObservableDictionary<string, Color>();
            ReadConfigurationFromRegistry();
        }

        #endregion

        #region public method
        
        /// <summary>
        /// Save configuration in registry
        /// </summary>
        public void SaveConfig()
        {
            //save all data
            GetSetRegisterValue(Regkeys.Choice, ((Color)TypesColor[Regkeys.Choice.ToString()]).ToString(), false);
            GetSetRegisterValue(Regkeys.ChoiceNull, ((Color)TypesColor[Regkeys.ChoiceNull.ToString()]).ToString(), false);
            GetSetRegisterValue(Regkeys.Element, ((Color)TypesColor[Regkeys.Element.ToString()]).ToString(), false);
            GetSetRegisterValue(Regkeys.Error, ((Color)TypesColor[Regkeys.Error.ToString()]).ToString(), false);
            GetSetRegisterValue(Regkeys.Sequence, ((Color)TypesColor[Regkeys.Sequence.ToString()]).ToString(), false);
            GetSetRegisterValue(Regkeys.Search, ((Color)TypesColor[Regkeys.Search.ToString()]).ToString(), false);
            GetSetRegisterValue(Regkeys.SchemaPath, SchemaPath, false);
        }

        /// <summary>
        /// get item color from data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Color GetColorConfiguration(Regkeys item)
        {
            return ((Color)TypesColor[item.ToString()]);
        }

        /// <summary>
        /// get item value
        /// </summary>
        /// <param name="item">item - key</param>
        /// <param name="value">updated value</param>
        /// <returns>return if success to update</returns> 
        public bool SetColorConfiguration(Regkeys item, string value)
        {
            if (item != Regkeys.SchemaPath)
            {                
                try
                {
                    //convert string to color
                    Color color = (Color)ColorConverter.ConvertFromString(value);
                    TypesColor[item.ToString()] = color;                    
                }
                catch (Exception)
                {
                    return false;
                }
                
            }
            else
                SchemaPath = value;
            return true;
        }


        #endregion

        #region private method

        /// <summary>
        /// read configuraion from registry.
        /// read color legend and schema path.
        /// </summary>
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

            //update legend list            
            TypesColor["ChoiceNull"] = colorChoiceNull;
            TypesColor["Choice"] = colorChoice;
            TypesColor["Element"] = colorElement;
            TypesColor["Sequence"] = colorSequence;
            TypesColor["Error"] = colorError;
            TypesColor["Search"] = colorSearch;

        }

        /// <summary>
        /// Read and Update data from registry
        /// </summary>
        /// <param name="key">the key for read/update </param>
        /// <param name="value">value for set in the key</param>
        /// <param name="isGet">get or set mode</param>
        /// <returns>in get mode - return the value in key
        /// otherwise, return null</returns>
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
                if (regKey != null) //check if open succeeded 
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
                //open key failed
                string s = e.Message;
                return null;
            }
        }

        #endregion


    }
}
