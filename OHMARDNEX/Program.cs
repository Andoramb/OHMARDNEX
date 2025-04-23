using OHMARDNEX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Reflection;
using System.Security.Principal;
using System.Diagnostics;
using System.IO;

namespace OHMARDNEX
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // The application runs with the form, which will handle the startup registration
            Application.Run(new Form1());
        }
        
        /// <summary>
        /// Adds the application to Windows startup
        /// </summary>
        public static void SetStartup(bool enable)
        {
            try
            {
                // Get the application executable path
                string appPath = Assembly.GetExecutingAssembly().Location;
                string appName = Path.GetFileNameWithoutExtension(appPath);
                
                if (enable)
                {
                    // Create a task scheduler using Process.Start to launch schtasks.exe
                    string taskName = "OHMARDNEX_Startup";
                    string workingDir = Path.GetDirectoryName(appPath);
                    
                    // Delete any existing task first
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "schtasks.exe",
                        Arguments = $"/delete /tn \"{taskName}\" /f",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    })?.WaitForExit();
                    
                    // Create a new task - runs at logon with highest privileges
                    Process process = Process.Start(new ProcessStartInfo
                    {
                        FileName = "schtasks.exe",
                        Arguments = $"/create /tn \"{taskName}\" /tr \"\\\"{appPath}\\\"\" /sc onlogon /rl highest /f",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    });
                    process?.WaitForExit();
                    
                    // Also remove from registry startup if it exists
                    RemoveFromRegistryStartup();
                }
                else
                {
                    // Delete the task if it exists
                    string taskName = "OHMARDNEX_Startup";
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "schtasks.exe",
                        Arguments = $"/delete /tn \"{taskName}\" /f",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    })?.WaitForExit();
                    
                    // Also remove from registry startup
                    RemoveFromRegistryStartup();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error setting startup task: " + ex.Message, "OHMARDNEX", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // Fall back to registry method if task scheduler fails
                SetStartupRegistry(enable);
            }
        }
        
        /// <summary>
        /// Checks if the application is set to run at startup
        /// </summary>
        public static bool IsStartupEnabled()
        {
            try
            {
                // Check if task exists
                string taskName = "OHMARDNEX_Startup";
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "schtasks.exe",
                    Arguments = $"/query /tn \"{taskName}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                });
                
                string output = process.StandardOutput.ReadToEnd();
                process?.WaitForExit();
                
                if (output.Contains(taskName))
                {
                    return true;
                }
                
                // Fall back to checking registry
                return IsStartupEnabledInRegistry();
            }
            catch
            {
                // Fall back to registry check
                return IsStartupEnabledInRegistry();
            }
        }
        
        /// <summary>
        /// Check if registered in the registry startup
        /// </summary>
        private static bool IsStartupEnabledInRegistry()
        {
            try
            {
                string appName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false))
                {
                    return key?.GetValue(appName) != null;
                }
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// Adds the application to Windows startup registry (legacy method)
        /// </summary>
        private static void SetStartupRegistry(bool enable)
        {
            try
            {
                // Get the application executable path
                string appPath = Assembly.GetExecutingAssembly().Location;
                string appName = Path.GetFileNameWithoutExtension(appPath);
                
                // Create a registry key for the current user's startup programs
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (enable)
                    {
                        // Add the application to startup registry
                        key.SetValue(appName, appPath);
                    }
                    else
                    {
                        // Remove the application from startup registry if it exists
                        if (key.GetValue(appName) != null)
                        {
                            key.DeleteValue(appName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error setting registry startup: " + ex.Message, "OHMARDNEX", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Removes the application from Windows startup registry
        /// </summary>
        private static void RemoveFromRegistryStartup()
        {
            try
            {
                string appName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (key?.GetValue(appName) != null)
                    {
                        key.DeleteValue(appName);
                    }
                }
            }
            catch
            {
                // Just silently fail, this is just cleanup
            }
        }
    }
}
