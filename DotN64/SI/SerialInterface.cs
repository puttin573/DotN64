﻿using System.Collections.Generic;

namespace DotN64.SI
{
    public partial class SerialInterface : Interface
    {
        #region Fields
        private readonly IReadOnlyList<MappingEntry> memoryMaps;
        #endregion

        #region Properties
        protected override IReadOnlyList<MappingEntry> MemoryMaps => memoryMaps;

        public StatusRegister Status { get; set; }
        #endregion

        #region Constructors
        public SerialInterface()
        {
            memoryMaps = new[]
            {
                new MappingEntry(0x04800018, 0x0480001B) // SI status.
                {
                    Read = o => (uint)Status,
                    Write = (o, v) => Status &= ~StatusRegister.Interrupt
                }
            };
        }
        #endregion
    }
}
