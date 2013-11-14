// Guids.cs
// MUST match guids.h
using System;

namespace NMSD.VSE_FormatDocumentOnSave
{
    static class GuidList
    {
        public const string guidVSPackage2PkgString = "2f4fac85-be4e-4d7a-8c74-93cc4389b427";
        public const string guidVSPackage2CmdSetString = "388f392f-e15a-4592-a9de-7845c48eea23";

        public static readonly Guid guidVSPackage2CmdSet = new Guid(guidVSPackage2CmdSetString);
    };
}