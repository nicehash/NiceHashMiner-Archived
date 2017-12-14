#include "CudaDetection.h"

// TODO maybe add nvml.dll check
//#include <Windows.h>

int main(int argc, char* argv[]) {
	CudaDetection detection;
	if (detection.QueryDevices()) {
		if (argc < 2) {
			detection.PrintDevicesJson_d();
		}
		else {
			detection.PrintDevicesJson();
		}
	}
	return 0;
}
