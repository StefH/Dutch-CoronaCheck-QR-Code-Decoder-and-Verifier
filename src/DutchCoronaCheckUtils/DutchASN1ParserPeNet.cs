using System.Numerics;
using System.Text;
using DutchCoronaCheckUtils.Models;
using PeNet.Asn1;

namespace DutchCoronaCheckUtils
{
    public static class DutchASN1ParserPeNet
    {
        public static BigInteger ReadAsBigInteger(this Asn1Node node, bool isUnsigned = false, bool isBigEndian = true)
        {
            return new BigInteger(((Asn1Integer)node).Value, isUnsigned, isBigEndian);
        }

        public static byte[] ReadAsOctetString(this Asn1Node node)
        {
            return ((Asn1OctetString)node).Data;
        }

        public static string ReadAsPrintableString(this Asn1Node node)
        {
            return ((Asn1PrintableString)node).Value;
        }

        public static TopLevelStructure Parse(byte[] base45Decoded)
        {
            var document = ((Asn1Sequence)Asn1Node.ReadNode(base45Decoded)).Nodes;

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
                IsSpecimen = GetStringData(ReadAsBigInteger(ADisclosed[1])),
                IsPaperProof = GetStringData(ReadAsBigInteger(ADisclosed[2])),
                ValidFrom = GetStringData(ReadAsBigInteger(ADisclosed[3])),
                ValidForHours = GetStringData(ReadAsBigInteger(ADisclosed[4])),
                FirstNameInitial = GetStringData(ReadAsBigInteger(ADisclosed[5])),
                LastNameInitial = GetStringData(ReadAsBigInteger(ADisclosed[6])),
                BirthDay = GetStringData(ReadAsBigInteger(ADisclosed[7])),
                BirthMonth = GetStringData(ReadAsBigInteger(ADisclosed[8]))
            };

            return topLevelStructure;
        }

        private static Metadata? ParseMetadata(BigInteger metadataInteger)
        {
            var data = GetData(metadataInteger);
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

        private static string? GetStringData(BigInteger value)
        {
            var data = GetData(value);
            if (data is null)
            {
                return null;
            }

            // Decode as UTF-8 string
            return Encoding.UTF8.GetString(data.Value.ToByteArray(false, true));
        }

        private static BigInteger? GetData(BigInteger value)
        {
            if (value.IsEven)
            {
                // Least significant bit acting as a null flag
                return null;
            }

            // Right-shift to get the data
            return value >> 1;
        }
    }
}