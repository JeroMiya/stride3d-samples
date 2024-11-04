Material properties:
  * Albeto and Emission should be set to sRGB
  * Roughness and Metalic to non-color
  * Set to Nearest

How to set an environment texture in Blender:
  * Click the World tab on the right pane
  * Click the yellow dot on the Color bit
  * Click Environment Texture
  * Select an hdr texture
  * Note: https://polyhaven.com/hdris for CC0 hdr textures. Download in hdr format

Setting up Material in Blender:
  * Import Albedo, Metalic, Roughness, and Emission textures into the shader
  * Set All to Nearest instead of Linear
  * Albedo and Emission set to sRGB
  * Metallic and Roughness set to Non-Color
  * Hook up Albedo to Base color
  * Hook up Metallic to Metallic
  * Hook up Roughness to Roughness
  * Hook up Emission to Emission Color
  * Set Emission Strength to 20 or so
	
	
Enable bloom in Blender:
  * Click Compositor tab on the top
  * Ensure "Use nodes" is checked
  * Shift-A to add a node, search for "Glare"
  * Add Glare node between Render Layers and Composite nodes
  * Hold ctrl-shift keys and select the Glare node to enable preview
  * Select "Bloom" in the dropdown of the Glare mode to switch to preview
  * In the preview pane, click the dropdown menu, in compositor section select "Always" to preview in realtime

Other Blender Render Settings:
  * Enable Ray Tracing
