#include "AMDOpenCLDeviceDetection.h"

#include <iostream>
#include <stdexcept>
#include <utility>

using namespace std;
using namespace cl;

AMDOpenCLDeviceDetection::AMDOpenCLDeviceDetection()
{
}

AMDOpenCLDeviceDetection::~AMDOpenCLDeviceDetection()
{
}

vector<Platform> AMDOpenCLDeviceDetection::getPlatforms() {
	vector<Platform> platforms;
	try {
		Platform::get(&platforms);
	} catch (Error const& err) {
#if defined(CL_PLATFORM_NOT_FOUND_KHR)
		if (err.err() == CL_PLATFORM_NOT_FOUND_KHR)
			cout << "No OpenCL platforms found" << endl;
		else
#endif
			throw err;
	}
	return platforms;
}

vector<Device> AMDOpenCLDeviceDetection::getDevices(vector<Platform> const& _platforms, unsigned _platformId) {
	vector<Device> devices;
	try {
		_platforms[_platformId].getDevices(/*CL_DEVICE_TYPE_CPU| */CL_DEVICE_TYPE_GPU | CL_DEVICE_TYPE_ACCELERATOR, &devices);
	} catch (Error const& err) {
		// if simply no devices found return empty vector
		if (err.err() != CL_DEVICE_NOT_FOUND)
			throw err;
	}
	return devices;
}

string AMDOpenCLDeviceDetection::StringnNullTerminatorFix(const string& str) {
	return string(str.c_str(), strlen(str.c_str()));
}

bool AMDOpenCLDeviceDetection::QueryDevices() {
	try {
		// get platforms
		auto platforms = getPlatforms();
		if (platforms.empty()) {
			cout << "No OpenCL platforms found" << endl;
			return false;
		}
		else {
			for (auto i_pId = 0u; i_pId < platforms.size(); ++i_pId) {
				string platformName = StringnNullTerminatorFix(platforms[i_pId].getInfo<CL_PLATFORM_NAME>());
				_platformNumbers.emplace(make_pair(platformName, i_pId));
				// not the best way but it should work
				bool isAMD = platformName.find("AMD", 0) != string::npos;
				vector<OpenCLDevice> platformDevs;
				auto clDevs = getDevices(platforms, i_pId);
				for (auto i_devId = 0u; i_devId < clDevs.size(); ++i_devId) {
					OpenCLDevice curDevice;
					curDevice.DeviceID = i_devId;
					curDevice._CL_DEVICE_NAME = StringnNullTerminatorFix(clDevs[i_devId].getInfo<CL_DEVICE_NAME>());
					switch (clDevs[i_devId].getInfo<CL_DEVICE_TYPE>()) {
					case CL_DEVICE_TYPE_CPU:
						curDevice._CL_DEVICE_TYPE = "CPU";
						break;
					case CL_DEVICE_TYPE_GPU:
						curDevice._CL_DEVICE_TYPE = "GPU";
						break;
					case CL_DEVICE_TYPE_ACCELERATOR:
						curDevice._CL_DEVICE_TYPE = "ACCELERATOR";
						break;
					default:
						curDevice._CL_DEVICE_TYPE = "DEFAULT";
						break;
					}

					curDevice._CL_DEVICE_GLOBAL_MEM_SIZE = clDevs[i_devId].getInfo<CL_DEVICE_GLOBAL_MEM_SIZE>();
					curDevice._CL_DEVICE_VENDOR = StringnNullTerminatorFix(clDevs[i_devId].getInfo<CL_DEVICE_VENDOR>());
					curDevice._CL_DEVICE_VERSION = StringnNullTerminatorFix(clDevs[i_devId].getInfo<CL_DEVICE_VERSION>());
					curDevice._CL_DRIVER_VERSION = StringnNullTerminatorFix(clDevs[i_devId].getInfo<CL_DRIVER_VERSION>());

					// AMD topology get Bus No
					if (isAMD) {
						cl_device_topology_amd topology = clDevs[i_devId].getInfo<CL_DEVICE_TOPOLOGY_AMD>();
						if (topology.raw.type == CL_DEVICE_TOPOLOGY_TYPE_PCIE_AMD) {
							curDevice.AMD_BUS_ID = (int)topology.pcie.bus;
						}
					}

					platformDevs.push_back(curDevice);
				}
				_devicesPerPlatform.emplace(make_pair(platformName, platformDevs));
			}
		}
	}
	catch (exception &ex) {
		// TODO
		cout << "AMDOpenCLDeviceDetection::QueryDevices() exception: " << ex.what() << endl;
		return false;
	}

	return true;
}

#define COMMA(num) ((--num > 0 ? "," : ""))

// this function is hardcoded and horrable
void AMDOpenCLDeviceDetection::PrintDevicesJson() {
	cout << "{" << endl;

	// platforms array scope
	{
		cout << "\t\"OCLPlatforms\" : {" << endl;
		int platformsComma = _platformNumbers.size();
		for (const auto &plat_name_num : _platformNumbers) {
			cout << "\t\t\"" + plat_name_num.first + "\" : "
				<< plat_name_num.second << COMMA(platformsComma)
				<< endl;
		}
		cout << "\t}," << endl;
	}
	// device per platform array scope
	{
		int devPlatformsComma = _devicesPerPlatform.size();
		cout << "\t\"OCLPlatformDevices\" : {" << endl;
		for (const auto &plat_name_devs : _devicesPerPlatform) {
			cout << "\t\t\"" + plat_name_devs.first + "\" : [" << endl;
			// device print
			int devComma = plat_name_devs.second.size();
			for (const auto &dev : plat_name_devs.second) {
				cout << "\t\t\t{" << endl;
				cout << "\t\t\t\t\"" << "DeviceID" << "\" : " << dev.DeviceID << "," << endl; // num
				cout << "\t\t\t\t\"" << "AMD_BUS_ID" << "\" : " << dev.AMD_BUS_ID << "," << endl; // num
				cout << "\t\t\t\t\"" << "_CL_DEVICE_NAME" << "\" : \"" << dev._CL_DEVICE_NAME << "\"," << endl;
				cout << "\t\t\t\t\"" << "_CL_DEVICE_TYPE" << "\" : \"" << dev._CL_DEVICE_TYPE << "\"," << endl; 
				cout << "\t\t\t\t\"" << "_CL_DEVICE_GLOBAL_MEM_SIZE" << "\" : " << dev._CL_DEVICE_GLOBAL_MEM_SIZE << "," << endl; // num
				cout << "\t\t\t\t\"" << "_CL_DEVICE_VENDOR" << "\" : \"" << dev._CL_DEVICE_VENDOR << "\"," << endl;
				cout << "\t\t\t\t\"" << "_CL_DEVICE_VERSION" << "\" : \"" << dev._CL_DEVICE_VERSION << "\"," << endl;
				cout << "\t\t\t\t\"" << "_CL_DRIVER_VERSION" << "\" : \"" << dev._CL_DRIVER_VERSION << "\"" << endl;
				cout << "\t\t\t}" << COMMA(devComma) << endl;
			}
			cout << "\t\t]" << COMMA(devPlatformsComma) << endl;
		}
		cout << "\t}" << endl;
	}

	cout << "}" << endl;
}