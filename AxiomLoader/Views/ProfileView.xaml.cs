using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;

namespace AxiomLoader.Views
{
    public partial class ProfileView : UserControl
    {
        public ProfileView()
        {
            InitializeComponent();
            LoadProfileData();
        }

        private void LoadProfileData()
        {
            // Generate a semi-realistic HWID based on machine info
            string machineName = Environment.MachineName;
            string processorCount = Environment.ProcessorCount.ToString();
            string rawId = machineName + processorCount + "AXIOM-SALT";
            
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(rawId));
                string hwid = BitConverter.ToString(hash).Replace("-", "").Substring(0, 16);
                HwidText.Text = hwid;
            }
        }
    }
}
