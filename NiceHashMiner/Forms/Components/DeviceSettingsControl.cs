using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Forms.Components {
    public partial class DeviceSettingsControl : UserControl {
        public DeviceSettingsControl() {
            InitializeComponent();

            amdSpecificSettings1.Visible = false;
            cpuSpecificSettings1.Visible = false;
        }

        DeviceGroupSettings _settings;
        ComputeDevice _selectedComputeDevice;
        public ComputeDevice SelectedComputeDevice {
            get { return _selectedComputeDevice; }
            set {
                if (value != null) {
                    _selectedComputeDevice = value;
                    _settings = ComputeDeviceGroupManager.Instance.GetDeviceGroupSettings(_selectedComputeDevice.Vendor);

                    SetSelectedDeviceFields();
                }
            }
        }

        private void SetSelectedDeviceFields() {
            if(_selectedComputeDevice == null) return;
            // TODO translation
            labelSelectedDeviceName.Text = "Name: " + _selectedComputeDevice.Name;
            labelSelectedDeviceGroup.Text = "Group: " +  _selectedComputeDevice.Vendor;

            if (_settings == null) return;

            fieldAPIBindPort.EntryText = _settings.APIBindPort.ToString();
            fieldUsePassword.EntryText = _settings.UsePassword;
            fieldMinimumProfit.EntryText = _settings.MinimumProfit.ToString();

            richTextBoxExtraLaunchParameters.Text = _settings.ExtraLaunchParameters;
            
            
            // enable group specific settings

            cpuSpecificSettings1.Visible = _selectedComputeDevice.DeviceGroupType == DeviceGroupType.CPU;
            amdSpecificSettings1.Visible = _selectedComputeDevice.DeviceGroupType == DeviceGroupType.AMD_OpenCL;
        }
    }
}
