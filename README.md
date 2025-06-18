# 📷 Image Processing Backend

## 📌 About the Project

This project is a simplified **image processing backend**, built using **C# ASP.NET Core Web API** and a **C++/CLI image processing module**. It accepts images encoded as **base64 strings** and applies a **Gaussian blur** using **OpenCV** in a multithreaded manner. The processed image is returned as a downloadable PNG or JPG stream.

The project was developed as part of a company technical challenge and fulfills all functional and code quality requirements specified in the assignment.

---

## 🚀 Core Features

* ✅ ASP.NET Core REST API with Swagger UI
* ✅ Accepts image input as base64 string
* ✅ Supports output encoding as PNG or JPG
* ✅ Multithreaded Gaussian blur using OpenCV (parallelized across all CPU cores)
* ✅ C++/CLI interop module for native processing
* ✅ Graceful cancellation support via `CancellationToken`
* ✅ Returns processed image as `FileStreamResult`
* ✅ Clean codebase with XML documentation
* ✅ Structured exception handling with clear status codes

---

## 🏗️ Technologies Used

* C# (.NET 8) WebAPI
* C++/CLI (Native Interop Layer)
* OpenCV 4
* Swagger UI
* Visual Studio 2022

---

## 🧰 Prerequisites

Ensure the following tools and libraries are installed:

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [Visual Studio 2022](https://visualstudio.microsoft.com/) with workloads:

  * ☑ Desktop development with C++
  * ☑ .NET desktop development
  * ☑ C++/CLI support
* OpenCV 4 native binaries configured for your platform

---

## ⚙️ Installation & Running

1. **Clone the repository:**

```bash
git clone https://github.com/bukovinszkiakos/ImageProcessingBackend.git
cd ImageProcessingBackend
```

2. **Configure OpenCV paths in the C++/CLI project settings:**

* Include directories
* Library directories
* Linker input (e.g., `opencv_world480.lib`)

3. **Build the solution** in Visual Studio (recommended: Debug | x64)

4. **Run the Web API project**. 

---

## 📥 Usage Example

### Endpoint:

```
POST /imageprocessing/image_processing
```

### Request Body (JSON):

```json
{
  "imageBase64": "<your_base64_image>",
  "outputEncoding": "PNG"
}
```

### Response:

Returns the blurred image as a downloadable stream with MIME type `image/png` or `image/jpeg`.

> You can also test this directly in Swagger UI using base64-encoded input.

---

## 🧠 Gaussian Blur with Parallel Processing

The native image processing logic is implemented in C++/CLI using OpenCV. Processing steps:

1. The image is decoded from raw bytes to `cv::Mat`
2. It is split into horizontal segments
3. Each segment is blurred in parallel using `std::thread`
4. The blurred segments are concatenated into a single final image

---

## 📄 File Structure

* `Controllers/ImageProcessingController.cs` — API routing and response logic
* `Services/ImageProcessingService.cs` — Decoding, validation, cancellation support, native interop
* `ImageProcessing.Native/Processor.cpp` — C++/CLI class for native entry point
* `ImageProcessing.Native/ParallelGaussianBlur.cpp` — Implements multithreaded blur logic

---

## 📌 Highlights

* Modular design following single responsibility principle
* XML documentation on all public APIs
* Multithreaded performance using native `std::thread`
* Clean cancellation handling using `CancellationToken`
* Safe exception propagation from native to managed code
* No hardcoded paths, secrets, or dependencies
* Git history follows clean, atomic commits with descriptive English messages

---

## 🔮 Future Improvements

* [ ] Add Swagger example for optional file upload support
* [ ] Add unit and integration tests for controller and service

---

## 👨‍💻 Developer

**Ákos Bukovinszki**
[GitHub Profile](https://github.com/bukovinszkiakos)

---

## 🪪 License

This project is licensed under the MIT License.

---

<p align="right">(<a href="#top">Back to top</a>)</p>


