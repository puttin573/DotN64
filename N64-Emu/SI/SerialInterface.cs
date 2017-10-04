﻿using System.Collections.Generic;
using System.Linq;

namespace N64Emu.SI
{
    public partial class SerialInterface
    {
        #region Fields
        private readonly IReadOnlyList<MappingEntry> memoryMaps;
        #endregion

        #region Properties
        public StatusRegister Status { get; } = new StatusRegister();
        #endregion

        #region Constructors
        public SerialInterface()
        {
            memoryMaps = new[]
            {
                new MappingEntry(0x04800018, 0x0480001B) // SI status.
                {
                    Read = o => (uint)Status.Bits.Data,
                    Write = (o, v) => Status.Interrupt = false
                }
            };
        }
        #endregion

        #region Methods
        public uint ReadWord(ulong address) => memoryMaps.First(e => e.Contains(address)).ReadWord(address);

        public void WriteWord(ulong address, uint value) => memoryMaps.First(e => e.Contains(address)).WriteWord(address, value);
        #endregion
    }
}
