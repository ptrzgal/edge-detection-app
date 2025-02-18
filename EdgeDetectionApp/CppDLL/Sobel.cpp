#include "Sobel.h"
#include "pch.h"

#include <windows.h>
#include <vector>
#include <cmath>
#include <cstring>

extern "C" __declspec(dllexport)
void SobelFilter32bpp(unsigned char* data, int width, int height, int stride)
{
    // Output buffer the same size as original data
    std::vector<unsigned char> output(height * stride);

    // Sobel masks definitions (Gx and Gy)
    int Gx[3][3] = {
        { -1,  0,  1 },
        { -2,  0,  2 },
        { -1,  0,  1 }
    };
    int Gy[3][3] = {
        { -1, -2, -1 },
        {  0,  0,  0 },
        {  1,  2,  1 }
    };

    // We process pixels ignoring the edges to avoid going out of range
    for (int y = 1; y < height - 1; y++)
    {
        for (int x = 1; x < width - 1; x++)
        {
            double sumX = 0.0;
            double sumY = 0.0;

            // For each pixel in a 3x3 window around (x, y)
            for (int ky = -1; ky <= 1; ky++)
            {
                for (int kx = -1; kx <= 1; kx++)
                {
                    int px = x + kx;
                    int py = y + ky;

                    // We calculate the offset for 32bpp: 4 bytes per pixel
                    int idx = py * stride + px * 4;

                    // We read channels (BGRA)
                    unsigned char b = data[idx + 0];
                    unsigned char g = data[idx + 1];
                    unsigned char r = data[idx + 2];

                    // Convert to grayscale
                    double gray = 0.299 * r + 0.587 * g + 0.114 * b;

                    sumX += gray * Gx[ky + 1][kx + 1];
                    sumY += gray * Gy[ky + 1][kx + 1];
                }
            }

            // We calculate the gradient magnitude
            double magnitude = std::sqrt(sumX * sumX + sumY * sumY);
            if (magnitude > 255) magnitude = 255; // We prevent exceeding the 8-bit range

            unsigned char edgeVal = static_cast<unsigned char>(magnitude);

            // We write the result to the output buffer
            int outIdx = y * stride + x * 4;
            output[outIdx + 0] = edgeVal; // Blue
            output[outIdx + 1] = edgeVal; // Green
            output[outIdx + 2] = edgeVal; // Red

            // We copy the original alpha channel
            output[outIdx + 3] = data[outIdx + 3];
        }
    }

    // Edge pixel support - we copy them without changes from the original
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (y == 0 || y == height - 1 || x == 0 || x == width - 1)
            {
                int idx = y * stride + x * 4;
                output[idx + 0] = data[idx + 0];
                output[idx + 1] = data[idx + 1];
                output[idx + 2] = data[idx + 2];
                output[idx + 3] = data[idx + 3];
            }
        }
    }

    // We copy the result from the buffer to the original image memory
    memcpy(data, output.data(), height * stride);
}
