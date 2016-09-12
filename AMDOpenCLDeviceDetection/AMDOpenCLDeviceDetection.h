#pragma once

#define __CL_ENABLE_EXCEPTIONS
#define CL_USE_DEPRECATED_OPENCL_2_0_APIS

#include "cl_ext.hpp"

#include <time.h>
#include <functional>
#include <map>
#include <vector>

#include "OpenCLDevice.h"

class AMDOpenCLDeviceDetection {
public:
	AMDOpenCLDeviceDetection();
	~AMDOpenCLDeviceDetection();
		
	bool QueryDevices();
	void PrintDevicesJson();

private:

	static std::vector<cl::Device> getDevices(std::vector<cl::Platform> const& _platforms, unsigned _platformId);
	static std::vector<cl::Platform> getPlatforms();

	std::map<std::string, std::vector<OpenCLDevice>> _devicesPerPlatform;
	std::map<std::string, int> _platformNumbers;

	static std::string StringnNullTerminatorFix(const std::string& str);
};
