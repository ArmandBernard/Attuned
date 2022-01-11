using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Attuned
{
    public static class RegistryWF
    {
        private static string ApplicationName
        {
            get
            {
                return Assembly.GetEntryAssembly().GetName().Name;
            }
        }

        /// <summary>
        /// The root directory or folder to store this program's keys in
        /// </summary>
        public static string RegistryRoot
        {
            get { return @"SOFTWARE\Armand\" + ApplicationName; }
        }

        /// <summary>
        /// Set Defaults
        /// </summary>
        public static void SetUp()
        {
            CreateKey();
        }

        #region Values

        /// <summary>
        /// Get all values under a key
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllValues(string key = null)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();

            // default key to registry root
            key = key ?? RegistryRoot;

            try
            {
                using (RegistryKey regkey = Registry.CurrentUser.OpenSubKey(key))
                {
                    if (regkey != null)
                    {
                        foreach (string name in regkey.GetValueNames())
                        {
                            values.Add(name, regkey.GetValue(name).ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistryError(ex, "Could not read registry values under key '" + key + ".");
            }

            return values;
        }



        /// <summary>
        /// Get a value under a registry key.
        /// Returns null if the key/value is not found/empty.
        /// </summary>
        /// <param name="valueName"></param>
        /// <returns></returns>
        public static string GetValue(string valueName, string key = null)
        {
            // default key to registry root
            key = key ?? RegistryRoot;

            object get;
            string result = null;
            try
            {
                using (RegistryKey regkey = Registry.CurrentUser.OpenSubKey(key))
                {
                    if (regkey != null)
                    {
                        get = regkey.GetValue(valueName);
                        if (get != null) { result = get.ToString(); }
                    }
                }
            }
            catch (Exception ex)
            {
                RegistryError(ex, 
                    "Could not read registry value '" + valueName + " under key '" + key + ".");
            }
            return result;
        }

        /// <summary>
        /// Set a value if it is not set
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="valueValue"></param>
        /// <param name="key"></param>
        public static void SetValueDefault(string valueName, string valueValue, string key = null)
        {
            if (GetValue(valueName, key) == null)
            {
                SetValue(valueName, valueValue, key);
            }
        }

        /// <summary>
        /// Set a value under a key
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="valueValue"></param>
        public static void SetValue(string valueName, string valueValue, string key = null)
        {
            // default key to registry root
            key = key ?? RegistryRoot;

            try
            {
                // get the key
                using (RegistryKey regkey = Registry.CurrentUser.OpenSubKey(key, true))
                {
                    // if the key doesn't exist
                    if (regkey == null)
                    {
                        // create it
                        CreateKey(key);
                        // if the key now exists, try again
                        if (CheckKeyExists(key))
                        {
                            SetValue(valueName, valueValue, key);
                            return;
                        }
                        else
                        {
                            throw new UnauthorizedAccessException("Could not create key");
                        }
                    }
                    // set the key's value
                    regkey.SetValue(valueName, valueValue, RegistryValueKind.String);
                }
            }
            catch (Exception ex)
            {
                RegistryError(ex,
                    "Could not write registry value '" + valueName + " under key '" + key + ".");
            }
        }

        /// <summary>
        /// Delete a registry value under a key
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="key"></param>
        public static void DeleteValue(string valueName, string key = null)
        {
            // default key to registry root
            key = key ?? RegistryRoot;

            try
            {
                // get the key
                using (RegistryKey regkey = Registry.CurrentUser.OpenSubKey(key, true))
                {
                    // if the key doesn't exist
                    if (regkey != null)
                    {
                        // set the key's value
                        regkey.DeleteValue(valueName, false);
                    }
                }
            }
            catch (Exception ex)
            {
                RegistryError(ex,
                    "Could not delete registry value '" + valueName +
                    " under key '" + key + ".");
            }
        }

        #endregion Values

        #region Keys

        /// <summary>
        /// Check if a registry value exists
        /// </summary>
        /// <param name="registryValue"></param>
        /// <returns></returns>
        public static bool CheckKeyExists(string key = null)
        {
            // default key to registry root
            key = key ?? RegistryRoot;

            try
            {
                using (RegistryKey regkey = Registry.CurrentUser.OpenSubKey(key))
                {
                    // if the key exists
                    if (regkey != null)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                // if failed, exit gracefully
                MessageBox.Show(
                    "Could not check registry key '" + key + ".\n" +
                    ex.Message + "\n" +
                    "The application will now close.",
                    Assembly.GetEntryAssembly().GetName().Name + ": Registry Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                Environment.Exit(2);
            }
            return false;
        }



        /// <summary>
        /// Create a registry key. Defaults to the key for the application application in the
        /// registry
        /// </summary>
        private static void CreateKey(string key = null)
        {
            // default key to registry root
            key = key ?? RegistryRoot;

            // Try to create the registry key
            try
            {
                Registry.CurrentUser.CreateSubKey(key);
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                // if failed, exit gracefully
                MessageBox.Show(
                    "Could not create registry key '" + key + ".\n" +
                    ex.Message + "\n" +
                    "The application will now close.",
                    ApplicationName + ": Registry Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                Environment.Exit(3);
                
            }
        }

        #endregion Keys

        private static void RegistryError(Exception ex, string message)
        {
            Logger.Write(ex);
            Logger.Write(message);
            // if failed, exit gracefully
            MessageBox.Show(
                message + "\nThe application will now close.",
                ApplicationName + ": Registry Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
                );
            Environment.Exit(1);
        }
    }
}
