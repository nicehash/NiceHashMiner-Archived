import os
from fnmatch import fnmatch

containsFilesC_SHARP_Code = "private static string[] ALL_FILES_BINS = {"

root = "."

for path, subdirs, files in os.walk(root):
    for name in files:
        #print os.path.join(path, name)
        file = '@"%s",' % os.path.join(path, name)[1:]
        containsFilesC_SHARP_Code =  "%s%s%s" % (containsFilesC_SHARP_Code, os.linesep, file)

containsFilesC_SHARP_Code = containsFilesC_SHARP_Code + os.linesep + "};"

print containsFilesC_SHARP_Code