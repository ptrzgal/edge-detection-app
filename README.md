# Edge Detection App

## Project Description
Edge Detection App is an application that detects edges in bitmap images using the Sobel operator. The user can load an image, choose the processing method (C++ or Assembly), and analyze the execution time of the algorithm.

## Technologies
- **C# (.NET 6.0, Windows Forms)** – User interface, conversion of 32bpp images to an 8-bit grayscale format, and integration with native libraries.
- **C++ (CppDLL)** and **Assembly (AsmDLL)** – Implementation of the Sobel algorithm on 8-bit data.

## Architecture
The application consists of three main modules:
1. **Graphical User Interface (C#)** – Allows users to load an image, select a processing method, and save the output.
2. **Image Conversion Module** – Converts the input image (32bpp) to an 8-bit grayscale format before processing and converts the result back to 32bpp.
3. **DLL Libraries (C++ and Assembly)** – Implementation of the Sobel algorithm in two versions, enabling performance analysis of both methods.

## Sobel Algorithm
The Sobel algorithm is used to detect edges by calculating the brightness gradient in the image.

1. **Image Conversion** – The input image is converted to an 8-bit grayscale format.
2. **Convolution Masks** – Two matrices are used:
   - Gx – Detects horizontal changes
   - Gy – Detects vertical changes
3. **Convolution Operation** – For each pixel (except for the border ones), calculations are performed on a 3×3 neighborhood, where pixel values are multiplied by the corresponding mask values.
4. **Gradient Calculation** – The resulting gradient is calculated as the sum of the absolute values of the horizontal and vertical components:
   \[ magnitude = |sumX| + |sumY| \]
5. **Assigning Values to the Output Image** – For border pixels, a value of 0 (black color) is set.

## Installation and Execution
### System Requirements
- Visual Studio 2022
- .NET 6.0
- Windows 10/11 (x64)

### Compilation and Execution
1. Clone the repository:
   ```sh
   git clone https://github.com/your-repo/EdgeDetectionApp.git
   cd EdgeDetectionApp
   ```
2. Open the project in Visual Studio 2022.
3. Configure the build in x64 mode.
4. Build the project (Build -> Build Solution).
5. Run the application (Start).

## Step-by-Step Application Workflow

1. **Launch the Application** – Start the Edge Detection App.  
   <p align="center">
     <img src="https://github.com/user-attachments/assets/808ae71d-996b-4df2-83ad-9b148491913d" width="600">
   </p>

2. **Load an Image** – Click the "Load Image" button to select a bitmap file.  
   <p align="center">
     <img src="https://github.com/user-attachments/assets/cd62ad04-5253-4747-915f-5e166b43235c" width="600">
   </p>

3. **Choose Processing Method** – Select between the C++ or Assembly implementation for edge detection.  
   <p align="center">
     <img src="https://github.com/user-attachments/assets/446362aa-ff91-4507-8010-8e3c74f11b3b" width="600">
   </p>

4. **Start Processing** – Click the "Start" button to execute the Sobel algorithm.  
   <p align="center">
     <img src="https://github.com/user-attachments/assets/8d41941d-7370-442d-9c3b-f5b1864b61c9" width="600">
   </p>

5. **Save the Output** – Optionally, save the edge-detected image for further use.  
   <p align="center">
     <img src="https://github.com/user-attachments/assets/14ad0836-29f6-4741-b6c5-1c43631ef9ee" width="600">
   </p>

## Results
- **Before:**
<p align="center">
  <img src="https://github.com/user-attachments/assets/8ac320b8-2e46-4da5-8949-1b3f80089e56" width="600">
</p>

- **After:**
<p align="center">
  <img src="https://github.com/user-attachments/assets/b06d0bc2-82b7-4447-ab67-b651a5fb0421" width="600">
</p>

## Author
**[ptrzgal](https://github.com/ptrzgal)** 
