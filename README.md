# Game Editor

## Overview
This academic project was developed using **C#** and the **MonoGame framework** to simulate the core functionalities of a custom game editor. The goal was to explore how editors manage assets, interface elements, shaders, and procedural content generation — all within a Windows Forms environment. The editor provides a structured interface for managing game resources, applying visual effects, and interacting with custom UI components. It serves as a hands-on introduction to engine tooling and editor-side logic.

## Objective
The project focused on implementing key systems commonly found in game development tools, with an emphasis on modular design, real-time responsiveness, and editor-side control. Each subsystem was built to simulate how professional game editors manage resources, interface logic, and rendering workflows.
### **Asset Management**
- Drag-and-drop interface for organizing textures, models, and sound files within categorized panels.
- Support for asset previewing using thumbnail rendering and metadata inspection.
- Centralized asset controller to track file paths, types, and usage references across the editor.
- Designed to reduce duplication and streamline resource access during runtime and export.
  
### **Menu Functionalities** 
- Creation of a custom editor tab using Windows Forms in Visual Studio 2022.
- Integration of UI elements such as radio buttons, text fields, checkboxes, and action buttons.
- Event-driven architecture using delegates and listeners to bind UI controls to editor logic.
- Dynamic layout updates based on user interaction and context-sensitive options.

### **Shader Creation** 
- Exploration of shader theory including vertex transformation, fragment coloring, and GPU-side execution.
- Implementation of custom shaders written in HLSL or GLSL (depending on MonoGame backend).
- Support for uniform variables and real-time parameter tweaking via editor sliders and input fields.
- Shader compilation pipeline integrated with the asset manager for live reloading and debugging.

### **Procedural Terrain Generation** 
- Use of noise functions (e.g., Perlin, Simplex) to generate heightmaps and terrain meshes.
- Adjustable parameters for resolution, amplitude, frequency, and seed values to control terrain shape.
- Mesh generation logic using indexed vertices and triangle strips for efficient rendering.
- Real-time preview window with regeneration triggers and material assignment options.

## Installation

1. **Download the Project**  
   - Click the green **Code** button on the repository page and select **Download ZIP**, then extract it to a folder of your choice.

2. **Open the Solution**  
   - Launch **Visual Studio 2022** and open the `.sln` file included in the project.  
   - Make sure you have the required components installed:
     - .NET Desktop Development  
     - MonoGame SDK  
     - Windows Forms dependencies  

3. **Build the Project**  
   - Set the configuration to **x64**.  
   - Build the solution using **Ctrl + Shift + B**.

4. **Run the Editor**  
   - Start the application using **Start Debugging** or **Start Without Debugging** inside Visual Studio.

### Important  
This project must be built and run in **x64 mode** to ensure all dependencies load correctly.

