#pragma once

#include <string>

struct OpenCLDevice {
	unsigned int DeviceID;
	std::string _CL_DEVICE_NAME;
	std::string _CL_DEVICE_TYPE;
	unsigned long long _CL_DEVICE_GLOBAL_MEM_SIZE;
	std::string _CL_DEVICE_VENDOR;
	std::string _CL_DEVICE_VERSION;
	std::string _CL_DRIVER_VERSION;
};

