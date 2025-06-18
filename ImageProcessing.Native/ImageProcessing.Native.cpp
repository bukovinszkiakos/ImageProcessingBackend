#define IServiceProvider WIN32_IServiceProvider
#include "pch.h"
#include "ImageProcessing.Native.h"

#include <opencv2/opencv.hpp>
#include <msclr/marshal_cppstd.h>
#include <vector>
#include <thread>

using namespace System;
using namespace System::IO;
using namespace msclr::interop;

namespace ImageProcessingNative {

    //  Multithreaded GaussianBlur helper
    static cv::Mat ParallelGaussianBlur(const cv::Mat& inputImage, int blurSize = 15)
    {
        int numThreads = std::thread::hardware_concurrency();
        if (numThreads <= 0) numThreads = 4;

        int height = inputImage.rows;
        int segmentHeight = height / numThreads; 

        std::vector<cv::Mat> segments(numThreads); 
        std::vector<cv::Mat> blurredSegments(numThreads);
        std::vector<std::thread> workers;

        for (int i = 0; i < numThreads; ++i)
        {
            int startY = i * segmentHeight; 
            int endY = (i == numThreads - 1) ? height : (i + 1) * segmentHeight; 
            cv::Rect roi(0, startY, inputImage.cols, endY - startY); 
            segments[i] = inputImage(roi).clone(); 

            workers.emplace_back([&, i, blurSize]() {
                cv::GaussianBlur(segments[i], blurredSegments[i], cv::Size(blurSize, blurSize), 0); 
                });
        }

        for (auto& t : workers) 
        {
            if (t.joinable()) t.join();
        }

        cv::Mat finalImage;
        cv::vconcat(blurredSegments, finalImage);
        return finalImage;
    }

    //  Main processing method called from C#
    array<Byte>^ Processor::ApplyGaussianBlur(array<Byte>^ imageBytes, bool outputAsPng) 
    {
        // Convert managed byte[] to std::vector<uchar>
        pin_ptr<Byte> pinnedArray = &imageBytes[0]; 
        std::vector<uchar> inputVector(
            reinterpret_cast<unsigned char*>(pinnedArray),
            reinterpret_cast<unsigned char*>(pinnedArray) + imageBytes->Length); 

        // Decode the image using OpenCV
        cv::Mat inputImage = cv::imdecode(inputVector, cv::IMREAD_COLOR);

        // Throw if OpenCV was unable to decode the image (e.g. invalid or unsupported format)
        if (inputImage.empty()) {
            throw gcnew System::Exception("Input image could not be decoded.");

        }
        // Throw if the image is too small to be meaningfully split across threads
        if (inputImage.rows < std::thread::hardware_concurrency()) {
            throw gcnew System::Exception("Image is too small to be processed with multiple threads.");
        }

        //  Apply multithreaded Gaussian blur
        cv::Mat blurredImage = ParallelGaussianBlur(inputImage);

        // Encode the blurred image to PNG or JPG
        std::vector<uchar> outputVector;
        std::string format = outputAsPng ? ".png" : ".jpg";
        cv::imencode(format, blurredImage, outputVector);

        // Convert result back to managed byte[]
        array<Byte>^ result = gcnew array<Byte>((int)outputVector.size()); 
        System::Runtime::InteropServices::Marshal::Copy((IntPtr)outputVector.data(), result, 0, result->Length); 

        return result;
    }
}
