<img width="2048" height="1024" alt="bdtool-logo-2" src="https://github.com/user-attachments/assets/b97f10ce-690e-486a-9233-fd9abdbf5b19" />
[![Bus VS Compact Cars](https://img.youtube.com/vi/YsoAnhhDEJo/0.jpg)](https://www.youtube.com/watch?v=YsoAnhhDEJo)

# burnout-data-tool
A C# command line tool for reading and writing Burnout 3/Revenge-era data files, such as VDB, VList, and maybe more in the future.

## Usage

### Editing Burnout 3 VDB values
Burnout 3 stores all values in the VDB file, including every vehicle's values.

1. Extract the whole game from the ISO to a folder or just the `VDB.xml` file from the `data` folder.
2. Export the VDB file to YAML in raw format with `bdtool.exe vdb export "path/to/vdb.xml" "path/to/vdb.yaml"`, or to a more readable format with `bdtool.exe vdb export "path/to/vdb.bin" "path/to/vdb.yaml" --format dto --definitions "path/to/bo3_vdb_definitions.yaml"`.
3. Edit the values in the exported YAML file.
4. Import the YAML file back to a binary file with `bdtool.exe vdb import "path/to/vdb.yaml" "path/to/vdb.xml"`, or `bdtool.exe vdb import "path/to/vdb.yaml" "path/to/vdb.xml" --format dto`, if `dto` format was used to export it in step 2.
5. Replace the original VDB file in the data folder with your new VDB file and recreate the ISO using the appropriate software (differs by platform). Some software may also allow you to replace the file in the ISO directly, if the size of the file wasn't changed.
6. Play!

### Editing Burnout 4 VDB values
Modifying BO4 values works the same way, except for per-vehicle values which are stored in their respective BGV files.

1. Extract the whole game from the ISO to a folder or just the BGV file of the vehicle that you want to modify from the `PVEH` folder. [Here's a list of vehicles.](https://docs.google.com/spreadsheets/d/1-S_HopSlF97elHJ04F-Hx4xdIb2YSxNBx6_d3JJDDnQ/edit?gid=2019748069#gid=2019748069)
2. Extract VDB section from the BGV file to a binary file with `bdtool.exe vdata extract "path/to/Car.BGV" "path/to/car_vdb.bin"`.
3. Export the VDB binary file to YAML in raw format with `bdtool.exe vdb export "path/to/car_vdb.bin" "path/to/car_vdb.yaml"`, or to a more readable format with `bdtool.exe vdb export "path/to/car_vdb.bin" "path/to/car_vdb.yaml" --format dto --definitions "path/to/bo4_bgv_vdb_definitions.yaml"`.
4. Edit the values in the exported YAML file.
5. Import the YAML file back to a binary file with `bdtool.exe vdb import "path/to/car_vdb.yaml" "path/to/car_vdb.bin"`, or `bdtool.exe vdb import "path/to/car_vdb.yaml" "path/to/car_vdb.bin" --format dto`, if `dto` format was used to export it in step 3.
6. Insert the VDB binary data back into the BGV file with `bdtool.exe vdata insert "path/to/Car.BGV" "path/to/car_vdb.bin"`
7. Replace the original BGV file in the vehicle's folder with your new BGV file and recreate the ISO using the appropriate software (differs by platform). Some software may also allow you to replace the file in the ISO directly, if the size of the file wasn't changed.
8. Play!

**Note:** Xbox 360 version BGV files need to be uncompressed first before extracting the VDB from it with `bdtool.exe tools zlib uncompress "path/to/Car.BGV" "path/to/Car_uncompressed.BGV"` command.

After the modified VDB is inserted into the uncompressed BGV file, it needs to be compressed again with `bdtool.exe tools zlib compress "path/to/Car_uncompressed.BGV" "path/to/Car.BGV"` command.

## Supported Types
- VDB (Value Database): Read/Write
- VList (Vehicle List): Read/Write
- BGV/BTV (Drivable/Traffic Vehicle Data): Partial Read/Partial write

## NOTES
- This tool is still in development, keep backups of your game files and be aware that there may still be changes to the exported formats!
- Initially the plan was to make this tool for specifically editing VDB values, so support for other Burnout data file types is still limited.

## Resources
Thanks to:
- [Burnout Wiki](https://burnout.wiki/wiki/Main_Page)
- [The Hidden Palace](https://hiddenpalace.org/)
- [EdnessP for the decompiled Hash function](https://raw.githubusercontent.com/EdnessP/scripts/main/burnout/GtHash.py)
