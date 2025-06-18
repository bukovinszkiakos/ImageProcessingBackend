#pragma once

using namespace System;

namespace ImageProcessingNative {
    public ref class Processor sealed
    {
    public:
        // Applies a multithreaded Gaussian blur to the input image using OpenCV.
        static array<Byte>^ ApplyGaussianBlur(array<Byte>^ imageBytes, bool outputAsPng);
    };
}
