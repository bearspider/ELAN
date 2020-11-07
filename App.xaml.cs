using EQAudioTriggers.Models;
using Syncfusion.SfSkinManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EQAudioTriggers
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzQ0Njg4QDMxMzgyZTMzMmUzMGFkdkZ4UjRWZWUxcW12U0dRZFIwRXFHQ2VkSjhyQk9pZUJaWUdtS0xxcGc9");
        }
    }
}
