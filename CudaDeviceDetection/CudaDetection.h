#pragma once

#include <map>
#include <vector>
#include <string>

#include "cuda_nvml_helper.h"
#include "CudaDevice.h"

#include <cstdint>

class CudaDetection
{
public:
	CudaDetection();
	~CudaDetection();

	bool QueryDevices();
	void PrintDevicesJson();
	void PrintDevicesJson_d();

private:

	//void print(CudaDevice &dev);
	void json_print(CudaDevice &dev);

	//void print_d(CudaDevice &dev);
	void json_print_d(CudaDevice &dev);

	std::vector<std::string> _errorMsgs;
	std::vector<CudaDevice> _cudaDevices;

	static std::map<uint16_t, std::string> _VENDOR_NAMES;
	static uint16_t getVendorId(nvmlPciInfo_t &nvmlPciInfo);
	static std::string getVendorString(nvmlPciInfo_t &nvmlPciInfo);
};

