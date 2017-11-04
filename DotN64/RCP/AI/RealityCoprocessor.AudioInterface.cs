﻿using System.Collections.Generic;

namespace DotN64.RCP
{
    public partial class RealityCoprocessor
    {
        public class AudioInterface : Interface
        {
            #region Fields
            private readonly IReadOnlyList<MappingEntry> memoryMaps;
            #endregion

            #region Properties
            protected override IReadOnlyList<MappingEntry> MemoryMaps => memoryMaps;

            private uint dramAddress;
            /// <summary>
            /// Starting RDRAM address (8B-aligned).
            /// </summary>
            public uint DRAMAddress
            {
                get => dramAddress;
                set => dramAddress = value & ((1 << 24) - 1);
            }

            private uint transferLength;
            public uint TransferLength
            {
                get => transferLength;
                set => transferLength = (uint)(value & ((1 << 18) - 1) & ~((1 << 3) - 1)); // "v2.0".
            }
            #endregion

            #region Constructors
            public AudioInterface()
            {
                memoryMaps = new[]
                {
                    new MappingEntry(0x04500000, 0x04500003) // AI DRAM address.
                    {
                        Write = (o, v) => DRAMAddress = v
                    },
                    new MappingEntry(0x04500004, 0x04500007) // AI length.
                    {
                        Write = (o, v) => TransferLength = v
                    }
                };
            }
            #endregion
        }
    }
}