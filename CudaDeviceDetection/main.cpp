#include "CudaDetection.h"

// TODO maybe add nvml.dll check
//#include <Windows.h>

int main() {
	CudaDetection detection;
	if (detection.QueryDevices()) {
		detection.PrintDevicesJson();
	}
	return 0;
}
