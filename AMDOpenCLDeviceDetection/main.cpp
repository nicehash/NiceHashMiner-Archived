#include "AMDOpenCLDeviceDetection.h"

int main() {
	AMDOpenCLDeviceDetection AMDOpenCLDeviceDetection;
	if (AMDOpenCLDeviceDetection.QueryDevices()) {
		//AMDOpenCLDeviceDetection.PrintDevicesJson();
		AMDOpenCLDeviceDetection.PrintDevicesJsonDirty();
	}
	return 0;
}

