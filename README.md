This application allows users to add, update and delete device entry as well as experience the simulation of monitoring IoT devices.

Installation.

• Visual Studio 2022

• Install NewtonSoft.JSON in Visual Studio using NuGet Package Manager. Run in Package Manager Console (Install-Package Newtonsoft.Json)

Steps.

• Copy or download the project folder.

• Make sure the following files exist:
  - 'IoT_Device_Management.exe'/ full project with '.sln'.
  - 'devices.json' for mock data.
• If run from Visual Studio:
  - open '.sln' file.
  - press F5 to build and run.
    
Notes.

• Make sure 'devices.json' is in the correct folder ( 'bin\Debug\net9.0-windows').
-----------------------------------------------------------------------------------------------
How to operate.

Add device.

1.Go to the top of the interface.

2.Enter device name on the blank box and choose device type from the drop down.

  Example: “Garage Sensor” for device name
	
          “Motion Sensor” for device type
					
3.Click Add Device button.

4.The device will display on the list below.

5.Click Save Devices button to save the data to the ‘devices.json’.

6.Log for add device will display on the Action Logs on the bottom of the interface.


<img width="793" height="533" alt="image" src="https://github.com/user-attachments/assets/c3d28e29-bfb6-4332-a1aa-af9b7b4bdcda" />

Update device:

1.Double click on the device name or device type on the list.

2.After edit, click outside the cell or press “Enter”.

3.Click Save Devices button to commit the changes.

4.Log for update device will appear on the Action Logs on the bottom of the interface.

<img width="790" height="540" alt="image" src="https://github.com/user-attachments/assets/193b73c3-88e2-4835-9612-501585a1f7d5" />

Delete device:

1.Select which device to delete.

2.Click Delete button on the right side.

3.Click Save Devices button.

4.Log for delete device will display on the bottom of the interface.

<img width="795" height="802" alt="image" src="https://github.com/user-attachments/assets/0368007d-8c24-4882-a89d-5295cce5d66c" />

Toggle Status:

1.Status of the device will change every 120 mins as set in the code.

2.It will choose random device to toggle the status from ‘Online’ to ‘Offline’ or vice versa.

3.If the status change to ‘Offline’, error message of disconnected device will appear on the log.

<img width="792" height="805" alt="image" src="https://github.com/user-attachments/assets/6bd0867f-21b4-43bb-af5e-7a497aa71aaa" />

View Logs:

1.Go to the bottom of the interface.

2.Action Logs panel display all the actions performed including add, update, delete device and toggle status.

<img width="792" height="803" alt="image" src="https://github.com/user-attachments/assets/1dff51b7-78cc-4682-ac31-a876a807e5e3" />




