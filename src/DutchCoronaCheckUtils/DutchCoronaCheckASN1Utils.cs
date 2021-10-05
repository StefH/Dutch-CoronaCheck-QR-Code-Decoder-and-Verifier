using System;
using System.IO;
using System.Numerics;
using System.Text;
using DutchCoronaCheckUtils.Models;
using PeNet.Asn1;

namespace DutchCoronaCheckUtils
{
    public static class DutchCoronaCheckASN1Utils
    {
        private const byte Version = 2;
        private const string PublicKey = "VWS-CC-2";

        private static BigInteger ReadAsBigInteger(this Asn1Node node, bool isUnsigned = false, bool isBigEndian = true)
        {
            return new BigInteger(((Asn1Integer)node).Value, isUnsigned, isBigEndian);
        }

        private static byte[] ReadAsOctetString(this Asn1Node node)
        {
            return ((Asn1OctetString)node).Data;
        }

        private static string ReadAsPrintableString(this Asn1Node node)
        {
            return ((Asn1PrintableString)node).Value;
        }

        public static byte[] Write(TopLevelStructure structure)
        {
            var node = new Asn1Sequence();

            var document = node.Nodes;
            document.Add(new Asn1Integer(structure.DisclosureTimeSeconds.ToByteArray(false, true)));
            document.Add(new Asn1Integer(structure.C.ToByteArray(false, true)));
            document.Add(new Asn1Integer(structure.A.ToByteArray(false, true)));
            document.Add(new Asn1Integer(structure.EResponse.ToByteArray(false, true)));
            document.Add(new Asn1Integer(structure.VResponse.ToByteArray(false, true)));
            document.Add(new Asn1Integer(structure.AResponse.ToByteArray(false, true)));

            var ADisclosed = new Asn1Sequence();

            var metadata = new Asn1Sequence();
            metadata.Nodes.Add(new Asn1OctetString(new[] { structure.ADisclosed?.Metadata?.Version ?? Version }));
            metadata.Nodes.Add(Asn1PrintableString.ReadFrom(new MemoryStream(Encoding.UTF8.GetBytes(structure.ADisclosed?.Metadata?.PublicKey ?? PublicKey))));

            ADisclosed.Nodes.Add(new Asn1Integer(EncodeData(new BigInteger(metadata.GetBytes(), false, true)).ToByteArray(false, true)));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.IsSpecimen).ToByteArray(false, true)));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.IsPaperProof).ToByteArray(false, true)));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.ValidFrom).ToByteArray(false, true)));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.ValidForHours).ToByteArray(false, true)));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.FirstNameInitial).ToByteArray(false, true)));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.LastNameInitial).ToByteArray(false, true)));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.BirthDay).ToByteArray(false, true)));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.BirthMonth).ToByteArray(false, true)));

            document.Add(ADisclosed);

            return node.GetBytes();
        }

        public static TopLevelStructure Read(byte[] base45Decoded)
        {
            var node = (Asn1Sequence)Asn1Node.ReadNode(base45Decoded);

            var document = node.Nodes;

            var topLevelStructure = new TopLevelStructure
            {
                DisclosureTimeSeconds = ReadAsBigInteger(document[0]),
                C = ReadAsBigInteger(document[1]),
                A = ReadAsBigInteger(document[2]),
                EResponse = ReadAsBigInteger(document[3]),
                VResponse = ReadAsBigInteger(document[4]),
                AResponse = ReadAsBigInteger(document[5])
            };

            var ADisclosed = ((Asn1Sequence)document[6]).Nodes;

            topLevelStructure.ADisclosed = new SecurityAspect
            {
                Metadata = ParseMetadata(ReadAsBigInteger(ADisclosed[0])),
                IsSpecimen = DecodeStringData(ReadAsBigInteger(ADisclosed[1])),
                IsPaperProof = DecodeStringData(ReadAsBigInteger(ADisclosed[2])),
                ValidFrom = DecodeStringData(ReadAsBigInteger(ADisclosed[3])),
                ValidForHours = DecodeStringData(ReadAsBigInteger(ADisclosed[4])),
                FirstNameInitial = DecodeStringData(ReadAsBigInteger(ADisclosed[5])),
                LastNameInitial = DecodeStringData(ReadAsBigInteger(ADisclosed[6])),
                BirthDay = DecodeStringData(ReadAsBigInteger(ADisclosed[7])),
                BirthMonth = DecodeStringData(ReadAsBigInteger(ADisclosed[8]))
            };

            return topLevelStructure;
        }

        private static Metadata? ParseMetadata(BigInteger metadataInteger)
        {
            var data = DecodeData(metadataInteger);
            if (data is null)
            {
                return null;
            }

            var metadataSequence = ((Asn1Sequence)Asn1Node.ReadNode(data.Value.ToByteArray(false, true))).Nodes;

            return new Metadata
            {
                Version = metadataSequence[0].ReadAsOctetString()[0],
                PublicKey = metadataSequence[1].ReadAsPrintableString(),
            };
        }

        private static string? DecodeStringData(BigInteger value)
        {
            var data = DecodeData(value);
            if (data is null)
            {
                return null;
            }

            // Decode as UTF-8 string
            return Encoding.UTF8.GetString(data.Value.ToByteArray(false, true));
        }

        private static BigInteger? DecodeData(BigInteger value)
        {
            if (value.IsEven)
            {
                // Least significant bit acting as a null flag
                return null;
            }

            // Right-shift to get the data
            return value >> 1;
        }

        private static BigInteger EncodeStringData(string? value)
        {
            if (value == null)
            {
                return 0;
            }

            var data = new BigInteger(Encoding.UTF8.GetBytes(value), false, true);

            return EncodeData(data);
        }

        private static BigInteger EncodeData(BigInteger? value)
        {
            if (value is null)
            {
                return 0;
            }

            // Left-shift to set the data
            var data = value.Value << 1;

            // east significant bit acting as data flag
            return data | 1;
        }
    }
}