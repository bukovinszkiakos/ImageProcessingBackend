#pragma once

using namespace System;

namespace ImageProcessingNative {
    public ref class Processor sealed
    {
    public:
        // Performs a dummy blur operation (to be replaced with OpenCV GaussianBlur)
        static array<Byte>^ MockBlur(array<Byte>^ imageBytes, bool outputAsPng);
    };
}
