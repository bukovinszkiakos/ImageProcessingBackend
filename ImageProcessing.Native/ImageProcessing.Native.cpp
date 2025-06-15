#include "pch.h"

#include "ImageProcessing.Native.h"

using namespace System;
using namespace System::IO;

namespace ImageProcessingNative {

    array<Byte>^ Processor::MockBlur(array<Byte>^ imageBytes, bool outputAsPng)
    {
         // Temporary dummy implementation:
        // Simply returns the original image without modification.
       // This will later be replaced by OpenCV GaussianBlur logic.
        return imageBytes;
    }

}