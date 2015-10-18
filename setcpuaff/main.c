#include <Windows.h>
#include <stdio.h>
#include <stdlib.h>

int main(int argc, char* argv[])
{
	DWORD pid;
	DWORD_PTR mask;
	HANDLE hProc;

	if (argc < 3)
	{
		printf("Usage: %s [PID] [AFFINITY]\n", argv[0]);
		return 0;
	}

	pid = atoi(argv[1]);
	mask = atoll(argv[2]);

	printf("PID=%u, MASK=%llu\n", pid, mask);

	hProc = OpenProcess(PROCESS_SET_INFORMATION, FALSE, pid);
	if (hProc == NULL)
	{
		printf("Error opening process, code=%u\n", GetLastError());
		return 0;
	}

	if (!SetProcessAffinityMask(hProc, mask))
	{
		printf("Error setting affinity, code=%u\n", GetLastError());
		return 0;
	}

	CloseHandle(hProc);

	printf("Affinity adjusted.\n");

	return 0;
}