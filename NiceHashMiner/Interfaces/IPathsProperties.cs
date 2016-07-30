using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Interfaces {
    public interface IPathsProperties {
        string FilePath { get; set; }
        string FilePathOld { get; set; }
    }
}
