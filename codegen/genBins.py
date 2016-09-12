import os
from fnmatch import fnmatch

c_sharp_Class_START = """
namespace NiceHashMiner.Utils {
    public partial class MinersDownloadManager : BaseLazySingleton<MinersDownloadManager> {
    #region CODE_GEN STUFF // listFiles.py
"""

containsFilesC_SHARP_Code = "private static string[] ALL_FILES_BINS = {"

root = "."

outFile = "BINS_CODEGEN.cs"
outFile_shared = "BINS_CODEGEN_SHARED.cs"
outFile_amd = "BINS_CODEGEN_AMD.cs"
outFile_nvidia = "BINS_CODEGEN_NVIDIA.cs"
inFile = "genBins.py"

for path, subdirs, files in os.walk(root):
    for name in files:
        #print os.path.join(path, name)
        if (".py" not in name) and (".cs" not in name):
            file = '@"%s",' % os.path.join(path, name)[1:]
            containsFilesC_SHARP_Code =  "%s%s%s" % (containsFilesC_SHARP_Code, os.linesep, file)

containsFilesC_SHARP_Code = containsFilesC_SHARP_Code + os.linesep + "};"

#print containsFilesC_SHARP_Code


c_sharp_Class_END = """
#endregion //CODE_GEN STUFF // listFiles.py
}
}
"""

print "%s%s%s" % (c_sharp_Class_START, containsFilesC_SHARP_Code, c_sharp_Class_END)