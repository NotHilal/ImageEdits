# Problème scientifique - Image

A C#/.NET Framework console application for manual, from-scratch BMP image processing, built as a university ("Problème scientifique") project. It reads raw `.bmp` bytes and implements pixel-level transformations, filters, and fractal renderers without relying on high-level image libraries.

## Features

**Image I/O & basic transforms**
- Load/save raw `.bmp` files from a `Fichiers/` folder (`MyImage`)
- Grayscale conversion, contrast adjustment, selective color recoloring
- Mirror, resize (enlarge/shrink by integer coefficient), rotation by arbitrary angle, and fast 180° rotation (clockwise/counter-clockwise)

**Filters & convolution**
- Edge detection, blur, sharpen ("repoussage", "netteté")
- Generic convolution matrix application
- Histogram generation

**Encoding**
- Simple image encoder/decoder between two images

**Fractals**
- Mandelbrot-style fractal rendering with color maps (`ColorMap`, `Complexe`)
- Buddhabrot renderer, including a multithreaded variant (`Buddhabrot`, `BuddhabrotMultiThreading`)

**Misc utilities**
- Base conversion and custom text/binary encoding helpers (`Methods`)
- Pixel-level RGB ↔ HSV conversion (`Pixel`)

## Project structure

| File | Purpose |
|---|---|
| `Program.cs` | Entry point / scratch area used to exercise the various features |
| `MyImage.cs` | Core image class: load, save, and all pixel-based transforms and filters |
| `Pixel.cs` | Pixel representation and RGB/HSV conversion |
| `ColorMap.cs` / `Complexe.cs` | Color mapping and complex-number math for fractal rendering |
| `Buddhabrot.cs` / `BuddhabrotMultiThreading.cs` | Buddhabrot fractal generation (single-threaded and multithreaded) |
| `Methods.cs` | Shared helper functions (base conversion, string/binary encoding, distance, etc.) |
| `ThreadingClass.cs` | Small threading demo/utility |

## Requirements

- .NET Framework 4.7.2
- Windows (uses `System.Drawing`)
- Visual Studio 2017+ (or `msbuild`) to build the solution

## Getting started

1. Open `Problème scientifique - Image.sln` in Visual Studio.
2. Place source `.bmp` images in a `Fichiers/` folder next to the executable (created automatically on first save if missing).
3. Build and run — `Program.cs` contains example calls (many commented out) demonstrating how to use each feature.

## Notes

This is a learning project focused on implementing image processing and fractal algorithms manually (no external imaging libraries), so code is organized around exploration rather than a fixed CLI/UI.
