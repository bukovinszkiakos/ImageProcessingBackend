#define IServiceProvider WIN32_IServiceProvider 
#include "pch.h"
#include "ImageProcessing.Native.h"

#include <opencv2/opencv.hpp>
#include <msclr/marshal_cppstd.h>

using namespace System;
using namespace System::IO;
using namespace msclr::interop;

namespace ImageProcessingNative {

    array<Byte>^ Processor::MockBlur(array<Byte>^ imageBytes, bool outputAsPng)
    {
        // Convert managed byte[] to unmanaged std::vector<uchar>
        pin_ptr<Byte> pinnedArray = &imageBytes[0];
        std::vector<uchar> inputVector(
            reinterpret_cast<unsigned char*>(pinnedArray),
            reinterpret_cast<unsigned char*>(pinnedArray) + imageBytes->Length);
        // Decode image using OpenCV (read as color image)
        cv::Mat inputImage = cv::imdecode(inputVector, cv::IMREAD_COLOR);
        if (inputImage.empty())
        {
            // Return empty managed array if decoding fails
            return gcnew array<Byte>(0);
        }

        // Apply Gaussian blur (adjust kernel size if needed)
        cv::Mat blurredImage;
        cv::GaussianBlur(inputImage, blurredImage, cv::Size(15, 15), 0);

        // Encode blurred image to PNG or JPG
        std::vector<uchar> outputVector;
        std::vector<int> params;

        // Format must start with a dot (e.g. ".png" or ".jpg")
        std::string format = outputAsPng ? ".png" : ".jpg";

        // Compress the image to memory
        cv::imencode(format, blurredImage, outputVector, params);

        // Copy unmanaged vector<uchar> back to managed byte[]
        array<Byte>^ result = gcnew array<Byte>((int)outputVector.size());
        System::Runtime::InteropServices::Marshal::Copy((IntPtr)outputVector.data(), result, 0, result->Length);

        return result;
    }

}
