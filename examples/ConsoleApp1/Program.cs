using System;
using System.Text.Json;
using DutchCoronaCheckUtils;

namespace ConsoleAppDecode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 200;

            DecodeExample_VWS_CC_1();
            Console.WriteLine(new string('-', 100));
            DecodeExample_VWS_CC_2();
            Console.WriteLine(new string('-', 100));
            DecodeReal("S");
            Console.WriteLine(new string('-', 100));
            DecodeReal("C");
        }

        private static void DecodeExample_VWS_CC_1()
        {
            Console.WriteLine("DecodeExample_VWS_CC_1");
            var qrCodeData = "NL2:2QFE0Z9H%H7%3H$R6-9 176RO-F-DWDF+H63L8E 4IP2W.YVV:/BVT%DBBCMKGKN0L0V17$$B*G* 3ZK3P/HFK1B-29-H0.66Z1D9$ON67TTKD2J88HYZG58NP6S3+*CM DPSA%%TGFH1X5EYV28F69RTUXC6.ME5A:4YE:4TE2PNE%FI-%E9O+XGR%ST6RET%CFNF*A*B$XO/1SFUP%JDRQ%SHJTKP7SMCHV12ZYT:WV-8OB7B:D*FWQHZTMHOUD83568KXI/-L43QES+C*XQX8 E6YL9Z8T188R5Y4PXRDNJRP%-K:3EK8IRJ$WYW$VUX0S3IFYZ$PYJEH+D$S%PPGB-C/VB8R+$FCTL/278YG2AAJHVX9MJNR%5PF*YO.4JDU6UBI95/C9YQ5RW%E NJ696WUY4*7UZ/-FU14D9K4MML6H9FC$569+D3AEN+BCS6%7TYCRRN4QN%BF6MWSFIN+5O4Y3CWK%NP.YSM3IT:-V-:Q62OXFD1V$+DE*VL%B6VUO-6I.VX6LNE45H$C0MP72FXZK.N+OSDU9HT/1NSBGJDNJJ3+3BZTPK67O0MUJRS.9WYGUAS9--+HMHC8H231YT :CN6Y+XMAOCKL96XFW1BJQX/EUBQ8GA7%FCH23$XG5V+EOLKM.E:/O5 2LM$GJZT1QUGRTMP6AHR8IWF3BZL6D /BS.3/LHTJG6QESY5HM0BWQ1A-61HYTU.Y6B0*YD37A03AKE:R+N.JZV5T7JFOICQ97KS19GP256M*U/%80%O1BCEI0D+U6G-SDGS.U..2CR*D.C.*UD5CXYDMPT5PD:I8HN2%ZTWVJKEA6PM27P8S YRXMBUE ZXN20+94TQ6DIOEA.LST-TQ*2V2 HUK6XI$T$H71FHZ/E39ZTOEIN42X9PHL-AS8PWA5FXUIKHMM+3$OJV2O/-VLKR%AMSSGV*3G:GFHC*H$.EI2NF LU1RMIINAXU%AV7B+.Y% 8UMDT+D8PJYSZD-5.-.3U5$QGF-DC4CX6Z+4W4Z8POIE*R97EDBWK+28HIU.4A GC5QATF66/*6IB+/7C1$9Q6UNC4R1E$C.H PZ94MV4UK/U4QL%R/G+E39C*3CRQG7WHJZPLT00WZ3X%6I86.*B9G% K 9NV$*2...AFFED%AQQ CUDG00RVJ4%EJYM--BD48S:4.K*:L0Q1%*BUJ1WJ3%N-52+ZBURQWB2X%L: QQ/M5*0%X8QSK7.WT3+RPHG-RMH1KIA/TZ/92-O*CPHPLOY9KXKM%7F5MJ3REJ9MO9X9A4.7T*ZFCTUF-UXNTLDNR-:.:HT..N:8S.3";

            var decodedData = DutchCoronaCheckBase45Utils.Decode(qrCodeData[4..]);
            var encodedString = DutchCoronaCheckBase45Utils.Encode(decodedData);

            var equal = encodedString == qrCodeData[4..];
            Console.WriteLine(equal);

            Decode(qrCodeData);
        }

        private static void DecodeExample_VWS_CC_2()
        {
            Console.WriteLine("DecodeExample_VWS_CC_2");
            var qrCodeData = "NL2:B4V.W9D:LWJ5W2S6A$XQ9N* Y252O4%%  ZNK**$840VPY8T7$J0GR$8L2%VMO/20/3C.C.L XO:FN%IWW.TI+G3KW2RA+ $6T1 BQAGU6HJ35D.2YPIT*6Y3C733IOBZIKEWP4L/$9TX6QUVQFFZWJ+RY/JV6N3%NX%Y4XX43J182O/.AELM1%E-D*Q+8*O1CG*9/5ENUJ0HXT*PJXJ*XE-6QFMM7*B$IFEY04:-PN14PX3% Q5-JQF9$YJFVBSUD*P/AXHJRNUIA:SCX*SBIQ*BHZG$PJ+LG-S*:0.GZ8M4HO.XLM$BKZG7H/BVRUW$7WH$B3$L-T58KK$20EDRZW1B*VJ1Q5VC:X/.5*OQJ/EA92-8J*-QL6J+3NX:C5%%XZ4LLIS31KKPA9:1FP++KT:.QFRZ%M5R$I2*DM36M%BW/3.LS9MX6YE8KU4S-.Q%W2ZCI7CQ79E/X342+5T3ODK8X.-F02J-GMF18KCE.5NDV2V8I/5L0GVNPQRF+T3A*$%HI3-$R3+*RO/X8N.RG7LBFJP5SO9QAD:KYRP978DTHFL39368JXWSO2CKLQTYDZ45CF0FE9J3$$6+ZX RSNRQ6+HV%DE$V:O/Q8FYO+.NZFL6R8R8UE.0:A*Q9$8HYB+WZ26UM%.4R4 25AA7XQW.NYAJCO6+-C QZEPKLYS6G0Z/YGHPR*+YKEDO*3LE:KP HT3NCJPRNRG9K0Y84*C-7N2-BQJXY/+D-VF IIQTJ6-AV83%1Y8JMXN1I6/JSHS+HEG+VU+8UX:LL*Y%B*$G$D4H9HZVMHKWT2-87UTR+EZIWJ*IOTQ70.V%CLHVY  2-HFW4BA6-+FWIW6C:WDS /FU2I9G$LXL$B/MY*WQYMN*R00IQZJJ- 6QFX*$%-9ZGS3%G";

            var decodedData = DutchCoronaCheckBase45Utils.Decode(qrCodeData[4..]);
            var encodedString = DutchCoronaCheckBase45Utils.Encode(decodedData);

            var equal = encodedString == qrCodeData[4..];
            Console.WriteLine(equal);

            Decode(qrCodeData);
        }            

        private static void DecodeReal(string x)
        {
            foreach (var data in new[] { $"QR_{x}_NLP2", $"QR_{x}_NLP", $"QR_{x}_NL" })
            {
                try
                {
                    var qrCodeData = Environment.GetEnvironmentVariable(data);
                    Console.WriteLine(qrCodeData);

                    Decode(qrCodeData);
                }
                catch
                {
                    //
                }
            }
        }

        private static void Decode(string qrCodeData)
        {
            var base45Decoded = DutchCoronaCheckBase45Utils.Decode(qrCodeData[4..]);

            var topLevelStructure = DutchCoronaCheckASN1Utils.Read(base45Decoded);

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