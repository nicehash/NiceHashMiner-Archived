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
            nvidiaSpecificSettings1.Visible = false;
        }

        ComputeDevice _selectedComputeDevice;
        public ComputeDevice SelectedComputeDevice {
            get { return _selectedComputeDevice; }
            set {
                if (value != null) {
                    _selectedComputeDevice = value;

                    SetSelectedDeviceFields();
                }
            }
        }

        public void InitLocale(ToolTip toolTip) {
            cpuSpecificSettings1.InitLocale(toolTip);
            amdSpecificSettings1.InitLocale(toolTip);
        }

        private void SetSelectedDeviceFields() {
            if(_selectedComputeDevice == null) return;
            // TODO translation
            labelSelectedDeviceName.Text = "Name: " + _selectedComputeDevice.Name;
            labelSelectedDeviceGroup.Text = "Group: " +  _selectedComputeDevice.Group;

            if (_selectedComputeDevice == null || _selectedComputeDevice.DeviceBenchmarkConfig == null) return;


            fieldUsePassword.EntryText = _selectedComputeDevice.DeviceBenchmarkConfig.UsePassword;
            richTextBoxExtraLaunchParameters.Text = _selectedComputeDevice.DeviceBenchmarkConfig.ExtraLaunchParameters;
            
            
            // enable group specific settings
            cpuSpecificSettings1.Visible = _selectedComputeDevice.DeviceGroupType == DeviceGroupType.CPU;
            amdSpecificSettings1.Visible = _selectedComputeDevice.DeviceGroupType == DeviceGroupType.AMD_OpenCL;
            // yea this is no good
            nvidiaSpecificSettings1.Visible =
                   _selectedComputeDevice.DeviceGroupType == DeviceGroupType.NVIDIA_2_1
                || _selectedComputeDevice.DeviceGroupType == DeviceGroupType.NVIDIA_3_x
                || _selectedComputeDevice.DeviceGroupType == DeviceGroupType.NVIDIA_5_x
                || _selectedComputeDevice.DeviceGroupType == DeviceGroupType.NVIDIA_6_x;
        }
    }
}
