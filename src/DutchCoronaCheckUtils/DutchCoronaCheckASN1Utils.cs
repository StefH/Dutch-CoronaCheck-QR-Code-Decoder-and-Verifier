using System;
using System.IO;
using System.Numerics;
using System.Text;
using DutchCoronaCheckUtils.Extensions;
using DutchCoronaCheckUtils.Models;
using PeNet.Asn1;

namespace DutchCoronaCheckUtils
{
    public static class DutchCoronaCheckASN1Utils
    {
        private const byte CredentialVersion = 2;
        private const string IssuerPkIdV1 = "VWS-CC-1";
        private const string IssuerPkIdV2 = "VWS-CC-2";

        private static BigInteger ReadAsBigInteger(this Asn1Node node)
        {
            return ((Asn1Integer)node).Value.ToBigEndianInteger();
        }

        private static byte[] ReadAsOctetString(this Asn1Node node)
        {
            return ((Asn1OctetString)node).Data;
        }

        private static string ReadAsPrintableString(this Asn1Node node)
        {
            return ((Asn1PrintableString)node).Value;
        }

        public static ProofSerializationV2 Read(byte[] base45Decoded)
        {
            var node = (Asn1Sequence)Asn1Node.ReadNode(base45Decoded);

            var document = node.Nodes;

            var proofSerializationV2 = new ProofSerializationV2
            {
                DisclosureTimeSeconds = ReadAsBigInteger(document[0]),
                C = ReadAsBigInteger(document[1]),
                A = ReadAsBigInteger(document[2]),
                EResponse = ReadAsBigInteger(document[3]),
                VResponse = ReadAsBigInteger(document[4]),
                AResponse = ReadAsBigInteger(document[5])
            };

            var ADisclosed = ((Asn1Sequence)document[6]).Nodes;

            var metaData = DecodeMetadata(ReadAsBigInteger(ADisclosed[0]));

            proofSerializationV2.ADisclosed = new SecurityAspect
            {
                Metadata = metaData,
                IsSpecimen = DecodeStringData(ReadAsBigInteger(ADisclosed[1])),
                IsPaperProof = DecodeStringData(ReadAsBigInteger(ADisclosed[2])),
                ValidFrom = DecodeStringData(ReadAsBigInteger(ADisclosed[3])),
                ValidForHours = DecodeStringData(ReadAsBigInteger(ADisclosed[4])),
                FirstNameInitial = DecodeStringData(ReadAsBigInteger(ADisclosed[5])),
                LastNameInitial = DecodeStringData(ReadAsBigInteger(ADisclosed[6])),
                BirthDay = DecodeStringData(ReadAsBigInteger(ADisclosed[7])),
                BirthMonth = DecodeBirthMonth(metaData?.IssuerPkId, ReadAsBigInteger(ADisclosed[8]))
            };

            return proofSerializationV2;
        }

        public static byte[] Write(ProofSerializationV2 structure)
        {
            var issuerPkId = structure.ADisclosed?.Metadata?.IssuerPkId;
            if (string.IsNullOrEmpty(issuerPkId) || issuerPkId != IssuerPkIdV2)
            {
                throw new NotSupportedException($"Only IssuerPkId with value '{IssuerPkIdV2}' is supported.");
            }

            var node = new Asn1Sequence();

            var document = node.Nodes;
            document.Add(new Asn1Integer(structure.DisclosureTimeSeconds.ToBigEndianByteArray()));
            document.Add(new Asn1Integer(structure.C.ToBigEndianByteArray()));
            document.Add(new Asn1Integer(structure.A.ToBigEndianByteArray()));
            document.Add(new Asn1Integer(structure.EResponse.ToBigEndianByteArray()));
            document.Add(new Asn1Integer(structure.VResponse.ToBigEndianByteArray()));
            document.Add(new Asn1Integer(structure.AResponse.ToBigEndianByteArray()));

            var ADisclosed = new Asn1Sequence();

            var metadata = new Asn1Sequence();
            metadata.Nodes.Add(new Asn1OctetString(new[] { structure.ADisclosed?.Metadata?.CredentialVersion ?? CredentialVersion }));
            metadata.Nodes.Add(Asn1PrintableString.ReadFrom(new MemoryStream(Encoding.UTF8.GetBytes(issuerPkId))));

            ADisclosed.Nodes.Add(new Asn1Integer(EncodeData(metadata.GetBytes().ToBigEndianInteger()).ToBigEndianByteArray()));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.IsSpecimen).ToBigEndianByteArray()));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.IsPaperProof).ToBigEndianByteArray()));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.ValidFrom).ToBigEndianByteArray()));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.ValidForHours).ToBigEndianByteArray()));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.FirstNameInitial).ToBigEndianByteArray()));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.LastNameInitial).ToBigEndianByteArray()));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.BirthDay).ToBigEndianByteArray()));
            ADisclosed.Nodes.Add(new Asn1Integer(EncodeStringData(structure.ADisclosed?.BirthMonth).ToBigEndianByteArray()));

            document.Add(ADisclosed);

            return node.GetBytes();
        }

        private static string? DecodeBirthMonth(string? issuerPkId, BigInteger value)
        {
            switch (issuerPkId)
            {
                case IssuerPkIdV1:
                    return value.ToString(); // Best guess ?

                default:
                    return DecodeStringData(value);
            }
        }

        private static CredentialMetadataSerialization? DecodeMetadata(BigInteger metadataInteger)
        {
            var data = DecodeData(metadataInteger);
            if (data is null)
            {
                return null;
            }

            var metadataSequence = ((Asn1Sequence)Asn1Node.ReadNode(data.Value.ToBigEndianByteArray())).Nodes;

            return new CredentialMetadataSerialization
            {
                CredentialVersion = metadataSequence[0].ReadAsOctetString()[0],
                IssuerPkId = metadataSequence[1].ReadAsPrintableString(),
            };
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

        private static string? DecodeStringData(BigInteger value)
        {
            var data = DecodeData(value);
            if (data is null)
            {
                return null;
            }

            // Decode as UTF-8 string
            return Encoding.UTF8.GetString(data.Value.ToBigEndianByteArray());
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

        private static BigInteger EncodeStringData(string? value)
        {
            if (value == null)
            {
                return 0;
            }

            var data = new BigInteger(Encoding.UTF8.GetBytes(value), false, true);

            return EncodeData(data);
        }
    }
}