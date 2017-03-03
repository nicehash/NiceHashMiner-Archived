#include "AMDOpenCLDeviceDetection.h"

int main(int argc, char* argv[]) {
	AMDOpenCLDeviceDetection AMDOpenCLDeviceDetection;
	if (AMDOpenCLDeviceDetection.QueryDevices()) {
		if (argc < 2) {
			AMDOpenCLDeviceDetection.PrintDevicesJsonDirty();
		}
		else {
			AMDOpenCLDeviceDetection.PrintDevicesJson();
		}
	}
	return 0;
}

