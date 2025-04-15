
This asset increments the build version of your project every time you build.
And for Android projects, it also increments the bundle code.

To use, just install the asset and build.

I have a YouTube video that explains how to use it: https://youtu.be/tdmzO644A-s

The demo scene exists to test with. Load the demo scene and build.
Then you can adjust things as you wish.

FYI:

The build version has 3 parts: (1)Major, (2) Minor, and (3) Build number.

This code updates the Build Number with a time stamp in the format of: YYMMDDHHMMTHHMMSSZ
Where:
	YY = Year
	MM = Month
	DD = Day
	HH = Hour
	MM = Minute
	SS = Second
	Z = Zulu time (UTC)

There is 1 line in the script that if you uncomment it, it will also increment the Minor version.
