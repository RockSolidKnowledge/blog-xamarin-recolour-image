using System;
using System.Collections.Generic;
using System.Text;

namespace RecolorImageExample
{
    public interface IFileSystem
    {
	    string GetTempImageFilePathForCurrentPixelDensity();
    }
}
