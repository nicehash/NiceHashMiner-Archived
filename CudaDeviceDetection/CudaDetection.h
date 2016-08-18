#pragma once

#include <vector>
#include <string>

#include "cuda_nvml_helper.h"
#include "CudaDevice.h"

class CudaDetection
{
public:
	CudaDetection();
	~CudaDetection();

	bool QueryDevices();
	void PrintDevicesJson();

private:

	void print(CudaDevice &dev);
	void json_print(CudaDevice &dev);

	std::vector<std::string> _errorMsgs;
	std::vector<CudaDevice> _cudaDevices;
};

