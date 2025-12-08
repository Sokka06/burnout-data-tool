# todo
- figure out what file defs are used for.
- figure out how and where per vehicle data stored. Are they accessed by an offset or by using a hash?
- figure out how data is accessed per vehicle and from the default data section. how to get the right hash.
- 

# Info

## VDB
VDB files store values that are used during runtime.
Takedown/Revenge era games seem to share the same VDB structure.

### Header
The header stores the Type (usually 2), 

Offset +0: Type (4 bytes)
Offset +4: Number of default values (4 bytes)
Offset +0x8: unknown value. maybe unused at least in BO4 Proto (4 bytes). Seems to be not accessed during normal gameplay.
Offset +0xC: Number of loaded files (4 bytes)
Offset +0x10: Read to get offset to file definitions array
Offset +0x14: Start of default values array

### Default Values
Default value entries are 8 bytes each and can store a few different data types (4 bytes): bool, float, int, vector3 (as a pointer to the data) or a callback (also an address pointer. maybe not used?).
The value after the raw value is an int (4 bytes) hash value that is used to access the value during runtime.
The hash is calculated from three strings using hashname function: Name, Path and Filename.
For example: "Steering Reaction Factor Neutral", "Slams and Shunts", "../Data/Export/ValueDB/Physics.cfg" = 0xC9ED4725 = -907196635: int (925353388), float (1E-05), bool (True)

Int values such as 29776, 30224, 29744, etc seem to point to addresses in the Data section that begins after the default values.
They are either Vector3's or Vector4's, always taking up 4 * 4 bytes.

### Unknown/Per vehicle values
This section contains more values, perhaps per vehicle as well as Vector3 values used by the pointers in the default section.
Possible that these values aren't in sequence since they're mostly accessed by addresses.

It's possible that not all vehicles have invidual values. Below works for most traffic cars, but not drivable cars. 
Maybe they use the default 600f values set in Construct__24CB4VehiclePhysicsAttribs.
"Driving Mass (Kg)" "Physics/Vehicle" "../Data/Export/ValueDB/VehiclePhysics/TRAFUSCAR02.cfg" = 0x060E3F6B = 101597035: int (1148846080), float (1000), bool (True)

### File Definitions
The file definitions are used to...
They are accessed with a hashed filename, for example: "../Data/Export/ValueDB/AI/defaults.cfg" = 0x7AD6F668 -> 0x68F6D67A
Notice that the hash needs to be reversed if the endian of the file is small, e.g. on PS2 and Xbox versions.

It is possible that not all files are found in the filedefs section.
Possibly only used during debugging to change values while the game is running.


## VList
VList files store data about the vehicles used in each game.
All of the games differ slightly in structure.


## ProgressData

## TrackList

## ProfileData

# Resources
Big thanks to:
- Burnout Wiki
- The Hidden Palace
- https://raw.githubusercontent.com/EdnessP/scripts/main/burnout/GtHash.py for Hash function


# Decompile notes

## interesting functions

Prepare__7CB4Game = many data files are queued to load into memory here.

InitialiseDefaultValues__Q27GtComms20CGtCommsDatabaseGamei = load some values from VDB file.

SetCarID__10CB4RaceCarUl = getting the car ID
GetData__20CB4VehicleDataStream = getting car data
Load__20CB4VehicleDataStreamUl = loading car data
CreateVehiclePathFromID__14CB4VehicleListUlPci
FixUp__14CB4VehicleData = fixing up vehicle data after loading

Register__32CB4DrivableVehiclePhysicsAttribsP17CB4VehiclePhysics = loading drivable vehicle physics attributes

## PCSX2

Finding a default value by hash in PCSX2 debugger:
- Set breakpoint to 002DA784 (jr ra) inside GetDefaultValueByHash function.
- It will pause when a matching hash is found in the VDB.
- Notice how a1 and a3 registers will have the same value (e.g. 8E73CE2D).
- Then if you go to the address that v0 now points to in the memory window, the next value will be the same sequence but reversed (2DCE738E).

This is the hash value, and it can be confirmed by searching for the hex value in the VDB file.

The v0 will point to a CGtCommsDatabaseDefaultValue, where the first 4 bytes will be the mData (UGtCommsDataElement), the actual raw value.
The next 4 bytes will be mnNameHash, the same hash value as seen in the a1 and a3 registers.

The function will then return this CGtCommsDatabaseDefaultValue.

## Example values
DatabaseEntry
mData = 1142292480 = 600f (default value set in Construct__24CB4VehiclePhysicsAttribs before the actual value is fetched)
mMaximum = 0 (not yet set?)
mMinimum = 0 (not yet set?)
mpcName = "Driving Mass (Kg)"
mpcPath = "Physics/Vehicle"
mpcFileName = "../Data/Export/ValueDB/VehiclePhysics/TRAFUSCAR02.cfg"
mnNameHash = 0x060E3F6B = 101597035: int (1148846080), float (1000), bool (True)