using System;
using System.Text.Json;
using DutchCoronaCheckUtils;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 200;

            DecodeExample();
            Console.WriteLine(new string('-', 100));
            DecodeReal();
        }

        private static void DecodeExample()
        {
            var qrCodeData = "NL2:B4V.W9D:LWJ5W2S6A$XQ9N* Y252O4%%  ZNK**$840VPY8T7$J0GR$8L2%VMO/20/3C.C.L XO:FN%IWW.TI+G3KW2RA+ $6T1 BQAGU6HJ35D.2YPIT*6Y3C733IOBZIKEWP4L/$9TX6QUVQFFZWJ+RY/JV6N3%NX%Y4XX43J182O/.AELM1%E-D*Q+8*O1CG*9/5ENUJ0HXT*PJXJ*XE-6QFMM7*B$IFEY04:-PN14PX3% Q5-JQF9$YJFVBSUD*P/AXHJRNUIA:SCX*SBIQ*BHZG$PJ+LG-S*:0.GZ8M4HO.XLM$BKZG7H/BVRUW$7WH$B3$L-T58KK$20EDRZW1B*VJ1Q5VC:X/.5*OQJ/EA92-8J*-QL6J+3NX:C5%%XZ4LLIS31KKPA9:1FP++KT:.QFRZ%M5R$I2*DM36M%BW/3.LS9MX6YE8KU4S-.Q%W2ZCI7CQ79E/X342+5T3ODK8X.-F02J-GMF18KCE.5NDV2V8I/5L0GVNPQRF+T3A*$%HI3-$R3+*RO/X8N.RG7LBFJP5SO9QAD:KYRP978DTHFL39368JXWSO2CKLQTYDZ45CF0FE9J3$$6+ZX RSNRQ6+HV%DE$V:O/Q8FYO+.NZFL6R8R8UE.0:A*Q9$8HYB+WZ26UM%.4R4 25AA7XQW.NYAJCO6+-C QZEPKLYS6G0Z/YGHPR*+YKEDO*3LE:KP HT3NCJPRNRG9K0Y84*C-7N2-BQJXY/+D-VF IIQTJ6-AV83%1Y8JMXN1I6/JSHS+HEG+VU+8UX:LL*Y%B*$G$D4H9HZVMHKWT2-87UTR+EZIWJ*IOTQ70.V%CLHVY  2-HFW4BA6-+FWIW6C:WDS /FU2I9G$LXL$B/MY*WQYMN*R00IQZJJ- 6QFX*$%-9ZGS3%G";

            Decode(qrCodeData);
        }

        private static void DecodeReal()
        {
            var qrCodeData = Environment.GetEnvironmentVariable("QR");
            Console.WriteLine(qrCodeData);

            Decode(qrCodeData);
        }

        private static void Decode(string qrCodeData)
        {
            var base45Decoded = DutchBase45Decoder.Decode(qrCodeData.Substring(4));

            var topLevelStructure = DutchASN1Parser.Parse(base45Decoded);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            options.Converters.Add(new BigIntegerJsonConverter());
            var json = JsonSerializer.Serialize(topLevelStructure, options);

            Console.WriteLine(json);
        }
    }
}