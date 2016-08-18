#include "OpenCLDeviceDetection.h"

int main() {
	OpenCLDeviceDetection openCLDeviceDetection;
	if (openCLDeviceDetection.QueryDevices()) {
		openCLDeviceDetection.PrintDevicesJson();
	}
	return 0;
}

