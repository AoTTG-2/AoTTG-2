// MIT License

// Copyright (c) 2020 NedMakesGames

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

#ifndef SOBELOUTLINES_INCLUDED
#define SOBELOUTLINES_INCLUDED

// The sobel effect runs by sampling the texture around a point to see
// if there are any large changes. Each sample is multiplied by a convolution
// matrix weight for the x and y components seperately. Each value is then
// added together, and the final sobel value is the length of the resulting float2.
// Higher values mean the algorithm detected more of an edge

// These are points to sample relative to the starting point
static float2 sobelSamplePoints[9] = {
    float2(-1, 1), float2(0, 1), float2(1, 1),
    float2(-1, 0), float2(0, 0), float2(1, 0),
    float2(-1, -1), float2(0, -1), float2(1, -1),
};

// Weights for the x component
static float sobelXMatrix[9] = {
    1, 0, -1,
    2, 0, -2,
    1, 0, -1
};

// Weights for the y component
static float sobelYMatrix[9] = {
    1, 2, 1,
    0, 0, 0,
    -1, -2, -1
};

// This function runs the sobel algorithm over the depth texture
void DepthSobel_float(float2 UV, float Thickness, out float Out) {
    float2 sobel = 0;
    // We can unroll this loop to make it more efficient
    // The compiler is also smart enough to remove the i=4 iteration, which is always zero
    [unroll] for (int i = 0; i < 9; i++) {
        float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + sobelSamplePoints[i] * Thickness);
        sobel += depth * float2(sobelXMatrix[i], sobelYMatrix[i]);
    }
    // Get the final sobel value
    Out = length(sobel);
}

// This function runs the sobel algorithm over the opaque texture
void ColorSobel_float(float2 UV, float Thickness, out float Out) {
    // We have to run the sobel algorithm over the RGB channels separately
    float2 sobelR = 0;
    float2 sobelG = 0;
    float2 sobelB = 0;
    // We can unroll this loop to make it more efficient
    // The compiler is also smart enough to remove the i=4 iteration, which is always zero
    [unroll] for (int i = 0; i < 9; i++) {
        // Sample the scene color texture
        float3 rgb = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV + sobelSamplePoints[i] * Thickness);
        // Create the kernel for this iteration
        float2 kernel = float2(sobelXMatrix[i], sobelYMatrix[i]);
        // Accumulate samples for each color
        sobelR += rgb.r * kernel;
        sobelG += rgb.g * kernel;
        sobelB += rgb.b * kernel;
    }
    // Get the final sobel value
    // Combine the RGB values by taking the one with the largest sobel value
    Out = max(length(sobelR), max(length(sobelG), length(sobelB)));
    // This is an alternate way to combine the three sobel values by taking the average
    // See which one you like better
    //Out = (length(sobelR) + length(sobelG) + length(sobelB)) / 3.0;
}

#endif