﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

namespace N64Emu.PI
{
    public partial class PeripheralInterface
    {
        #region Fields
        private readonly IReadOnlyList<MappingEntry> memoryMaps;
        #endregion

        #region Properties
        public StatusRegister Status { get; } = new StatusRegister();

        public byte[] BootROM { get; set; }

        public byte[] RAM { get; } = new byte[64];

        public Domain[] Domains { get; } = new[]
        {
            new Domain(),
            new Domain()
        };
        #endregion

        #region Constructors
        public PeripheralInterface()
        {
            memoryMaps = new[]
            {
                new MappingEntry(0x1FC00000, 0x1FC007BF) // PIF Boot ROM.
                {
                    Read = o => (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(BootROM, (int)o))
                },
                new MappingEntry(0x1FC007C0, 0x1FC007FF) // PIF (JoyChannel) RAM.
                {
                    Read = o => BitConverter.ToUInt32(RAM, (int)o),
                    Write = (o, v) => Array.Copy(BitConverter.GetBytes(v), 0, RAM, (int)o, sizeof(uint))
                },
                new MappingEntry(0x04600010, 0x04600013) // PI status.
                {
                    Read = o => (uint)Status.Bits.Data,
                    Write = (o, v) =>
                    {
                        var bits = new BitVector32((int)v);

                        if (bits[StatusRegister.ResetControllerMask])
                            ResetController();

                        if (bits[StatusRegister.ClearInterruptMask])
                            ClearInterrupt();
                    }
                },
                new MappingEntry(0x04600014, 0x04600017) // PI dom1 latency.
                {
                    Write = (o, v) => Domains[0].Latency = (byte)v
                },
                new MappingEntry(0x04600018, 0x0460001B) // PI dom1 pulse width.
                {
                    Write = (o, v) => Domains[0].PulseWidth = (byte)v
                },
                new MappingEntry(0x0460001C, 0x0460001F) // PI dom1 page size.
                {
                    Write = (o, v) => Domains[0].PageSize = (byte)v
                },
                new MappingEntry(0x04600020, 0x04600023) // PI dom1 release.
                {
                    Write = (o, v) => Domains[0].Release = (byte)v
                }
            };
        }
        #endregion

        #region Methods
        private void ClearInterrupt() { /* TODO: Implement. */ }

        private void ResetController() { /* TODO: Implement. */ }

        public uint ReadWord(ulong address) => memoryMaps.First(e => e.Contains(address)).ReadWord(address);

        public void WriteWord(ulong address, uint value) => memoryMaps.First(e => e.Contains(address)).WriteWord(address, value);
        #endregion
    }
}
