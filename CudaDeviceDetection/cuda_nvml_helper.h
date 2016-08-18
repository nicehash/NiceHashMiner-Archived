#pragma once

#include <cuda.h>
#include <cuda_runtime.h>
#include <nvml.h>

#define CUDA_SAFE_CALL(call)								\
do {														\
	cudaError_t err = call;									\
	if (cudaSuccess != err) {								\
		const char * errorString = cudaGetErrorString(err);	\
		fprintf(stderr,										\
			"CUDA error in func '%s' at line %i : %s.\n",	\
			__FUNCTION__, __LINE__, errorString);			\
		throw std::runtime_error(errorString);				\
				}											\
} while (0)

#define NVML_SAFE_CALL(call)								\
do {														\
	nvmlReturn_t err = call;								\
	if (NVML_SUCCESS != err) {								\
		const char * errorString = nvmlErrorString(err);	\
		fprintf(stderr,										\
			"NVML error in func '%s' at line %i : %s.\n",	\
			__FUNCTION__, __LINE__, errorString);			\
		throw std::runtime_error(errorString);				\
					}										\
} while (0)

