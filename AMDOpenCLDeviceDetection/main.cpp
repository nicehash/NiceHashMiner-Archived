#include "AMDOpenCLDeviceDetection.h"

int main() {
	AMDOpenCLDeviceDetection AMDOpenCLDeviceDetection;
	if (AMDOpenCLDeviceDetection.QueryDevices()) {
		AMDOpenCLDeviceDetection.PrintDevicesJson();
	}
	return 0;
}

