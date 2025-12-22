using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bdtool.Dto;
using bdtool.Models.B3;
using bdtool.Models.B4;
using bdtool.Models.Common;
using bdtool.Utilities;

namespace bdtool.Converters
{
    public static class VListConverter
    {
        public static Dto.VehicleList ToDto(VList vlist)
        {
            switch (vlist)
            {
                case B3VehicleList b3List:
                    var entriesb3 = new List<Dto.B3.B3VehicleList.Entry>(b3List.VehicleCount);

                    for (int i = 0; i < b3List.VehicleCount; i++)
                    {
                        var entry = new Dto.B3.B3VehicleList.Entry()
                        {
                            Id = GtID.GtIDConvertToString(b3List.VehicleIDs[i]),
                            Driveable = b3List.VehicleIsDriveable[i],
                            Rank = b3List.RaceCarRanks[i],
                            Unk1 = b3List.Unk1[i],
                            Unk2 = b3List.Unk2[i]
                        };

                        entriesb3.Add(entry);
                    }

                    return new Dto.B3.B3VehicleList()
                    {
                        VersionNumber = vlist.VersionNumber,
                        Entries = entriesb3
                    };
                case B4VehicleList b4List:
                    var entriesb4 = new List<Dto.B4.B4VehicleList.Entry>(b4List.VehicleCount);

                    for (int i = 0; i < b4List.VehicleCount; i++)
                    {
                        var entry = new Dto.B4.B4VehicleList.Entry()
                        {
                            Id = GtID.GtIDConvertToString(b4List.VehicleIDs[i]),
                            Driveable = b4List.VehicleIsDriveable[i],
                            Rank = b4List.RaceCarRanks[i],
                            MaxCrashScore = b4List.RaceCarRanks[i],
                            GrudgePoints = b4List.VehicleGrudgePoints[i],
                            Price = b4List.VehiclePrice[i],
                            DefaultColor = b4List.VehicleDefaultColor[i]
                        };

                        entriesb4.Add(entry);
                    }

                    return new Dto.B4.B4VehicleList()
                    {
                        VersionNumber = vlist.VersionNumber,
                        Entries = entriesb4
                    };
                default:
                    throw new NotImplementedException();
            }
        }

        public static VList FromDto(Dto.VehicleList dto)
        {
            switch (dto)
            {
                case Dto.B3.B3VehicleList b3List:
                    var idsb3 = new List<ulong>(b3List.Entries.Count);
                    var isDriveableb3 = new List<bool>(b3List.Entries.Count);
                    var raceCarRanksb3 = new List<int>(b3List.Entries.Count);
                    var unk1b3 = new List<int>(b3List.Entries.Count);
                    var unk2b3 = new List<int>(b3List.Entries.Count);

                    for (int i = 0; i < b3List.Entries.Count; i++)
                    {
                        var entry = b3List.Entries[i];

                        idsb3.Add(GtID.Compress(entry.Id));
                        isDriveableb3.Add(entry.Driveable);
                        raceCarRanksb3.Add(entry.Rank);
                        unk1b3.Add(entry.Unk1);
                        unk2b3.Add(entry.Unk2);
                    }

                    return new B3VehicleList()
                    {
                        VersionNumber = b3List.VersionNumber,
                        VehicleCount = b3List.Entries.Count,
                        VehicleIDs = idsb3,
                        VehicleIsDriveable = isDriveableb3,
                        RaceCarRanks = raceCarRanksb3,
                        Unk1 = unk1b3,
                        Unk2 = unk2b3
                    };
                case Dto.B4.B4VehicleList b4List:
                    var idsb4 = new List<ulong>(b4List.Entries.Count);
                    var isDriveableb4 = new List<bool>(b4List.Entries.Count);
                    var raceCarRanksb4 = new List<int>(b4List.Entries.Count);
                    var maxCrashScores = new List<int>(b4List.Entries.Count);
                    var grudgePoints = new List<int>(b4List.Entries.Count);
                    var prices = new List<int>(b4List.Entries.Count);
                    var defaultColors = new List<sbyte>(b4List.Entries.Count);

                    for (int i = 0; i < b4List.Entries.Count; i++)
                    {
                        var entry = b4List.Entries[i];

                        idsb4.Add(GtID.Compress(entry.Id));
                        isDriveableb4.Add(entry.Driveable);
                        raceCarRanksb4.Add(entry.Rank);
                        maxCrashScores.Add(entry.MaxCrashScore);
                        grudgePoints.Add(entry.GrudgePoints);
                        prices.Add(entry.Price);
                        defaultColors.Add(entry.DefaultColor);
                    }

                    return new B4VehicleList()
                    {
                        VersionNumber = b4List.VersionNumber,
                        VehicleCount = b4List.Entries.Count,
                        VehicleIDs = idsb4,
                        VehicleIsDriveable = isDriveableb4,
                        RaceCarRanks = raceCarRanksb4,
                        VehicleMaxCrashScore = maxCrashScores,
                        VehicleGrudgePoints = grudgePoints,
                        VehiclePrice = prices,
                        VehicleDefaultColor = defaultColors
                    };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
