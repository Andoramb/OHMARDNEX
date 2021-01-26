# OHMARDNEX
### Open Hardware Monitor with Arduino and Nextion

Welcome to my little side project:

We're going to make a simple hardware and sensor monitoring tool that looks great as a built-in piece of hardware or as a gadget on your desktop!

### Working principle:
Read the data from your Laptop/PC -> send it to an Arduino that simply forwards it to a Nextion display!

### Good to know before starting:
1. The core app is based on Open Hardware Monitor - it needs admin rights to read some temps and info
2. The Arduino acts only as a serial data forwarder to the display
   - it can be connected via USB
   - directly to the motherboard usb pins
3. It works with the 2 famous displays: Itead's [Nextion](https://nextion.tech/enhanced-series-introduction/) and it's original Chinese equivalent of TJC
   - currently the user interface is created for the following 2 models: NX4832K035_011, TJC4832K035_011

**Naming convention:**
 - NX - Nextion<br>
 - 4832 - 480 x 320 resolution<br>
 - K - ENHANCED family<br>
 - 035 - 3,5inch screen size<br>

### Page layout / Features:
 - Status1:
	 - CPU temp/load
	 - GPU temp/load
	 - RAM usage
	 - GPU RAM usage
 - Status2:
	 - HDD1 name/used space
	 - HDD2 name/used space
	 - GPU fan usage/RPM
	 - CPU Power consumption
 - Graph:
	 - CPU & GPU temp - long-term representation
	 - Settings:
     - Show/Hide either the CPU or GPU temp
		 - Colorize CPU temp based on it's values
		 - Change GPU color to a custom color
  - Info
	  - Minimal info about the motherboard, CPU, GPU, HDD1, HDD2, RAM
  - Settings
  	- Manually change the display's brightness
    - Change when the display goes to sleep 
    - Restart the display
    - Show time and date
	
### Other features:
  - The executable requires admin priviledges, as the Open Hardware Monitor library needs to read the sensor data (don't worry about that) :)
  - The display turns off after 1min, if it doesn't receive data
  - To manually turn off the display, just press "Lock" on the main page
  - Only 2 storage units are handled, it should be enough
  - Send custom commands to the display (ex. sleep=1)
  
### How to get started:
1. Flash the Arduino with SimpleSerialReader.ino
2. Flash the precompiled *.tft file for your display
3. Print the display bracket
4. Run the executable
   - Select the correct COM port
   - Select baud rate (usually 115200)
   - Choose update rate - works fine with 2000msec
   
### Want to contribuite?
Check out the resources and the project folder! Here are some tips for future improvements:
- [x] Feature to minimize the app to the taskbar
- [x] Feature to select which data has to be sent to the display
- [x] Send custom commands to the display
- [ ] Add autostart with Windows! - I just started C#, the app can be improved in many ways
- [ ] Create different themes for the user interface - the display can have many themes, so it matches your surroundings!
- [ ] Extend the functionality - other external libraries can be included - such as fan control or RGB light control!
