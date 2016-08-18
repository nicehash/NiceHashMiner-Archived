#pragma once

#include <string>


struct CudaDevice {
	unsigned int DeviceID;
	std::string DeviceName;
	std::string SMVersionString;
	int SM_major;
	int SM_minor;
	std::string UUID;
	size_t DeviceGlobalMemory;
	unsigned int pciDeviceId;    //!< The combined 16-bit device id and 16-bit vendor id
	unsigned int pciSubSystemId; //!< The 32-bit Sub System Device ID
};

