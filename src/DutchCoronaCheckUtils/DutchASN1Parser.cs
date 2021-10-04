using System.Formats.Asn1;
using System.Numerics;
using System.Text;
using DutchCoronaCheckUtils.Models;

namespace DutchCoronaCheckUtils
{
    public static class DutchASN1Parser
    {
        public static TopLevelStructure Parse(byte[] base45Decoded)
        {
            var reader = new AsnReader(base45Decoded, AsnEncodingRules.BER);
            var document = reader.ReadSequence();

            var topLevelStructure = new TopLevelStructure
            {
                DisclosureTimeSeconds = document.ReadInteger(),
                C = document.ReadInteger(),
                A = document.ReadInteger(),
                EResponse = document.ReadInteger(),
                VResponse = document.ReadInteger(),
                AResponse = document.ReadInteger()
            };

            var ADisclosed = document.ReadSequence();

            var metadata = ParseMetadata(ADisclosed.ReadInteger());

            topLevelStructure.ADisclosed = new SecurityAspect
            {
                Metadata = metadata,
                IsSpecimen = GetStringData(ADisclosed.ReadInteger()),
                IsPaperProof = GetStringData(ADisclosed.ReadInteger()),
                ValidFrom = GetStringData(ADisclosed.ReadInteger()),
                ValidForHours = GetStringData(ADisclosed.ReadInteger()),
                FirstNameInitial = GetStringData(ADisclosed.ReadInteger()),
                LastNameInitial = GetStringData(ADisclosed.ReadInteger()),
                BirthDay = GetStringData(ADisclosed.ReadInteger()),
                BirthMonth = GetStringData(ADisclosed.ReadInteger())
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

            var metadataReader = new AsnReader(data.Value.ToByteArray(false, true), AsnEncodingRules.BER);
            var metadataSequence = metadataReader.ReadSequence();

            return new Metadata
            {
                Version = metadataSequence.ReadOctetString()[0],
                PublicKey = metadataSequence.ReadCharacterString(UniversalTagNumber.PrintableString)
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