# Decoding and verifying the Dutch CoronaCheck QR Code

Based on this [blog - Decoding the Dutch domestic CoronaCheck QR code](https://www.bartwolff.com/Blog/2021/08/21/decoding-the-dutch-domestic-coronacheck-qr-code) I created C# code to decode the Dutch CoronaCheck QR Code.

## Blog
For a full blog related to QR Codes in general and more details on this project, see [mstack.nl : decoding-coronacheck-qrcode](https://mstack.nl/blog/20211013-decoding-coronacheck-qrcode/).

## Overview
![overview](https://github.com/StefH/Dutch-CoronaCheck-QR-Code-Decoder-and-Verifier/blob/main/resources/overview.png)

## Steps

### 1. Get QR Code data

Use a library to decode a QR Code image to data : https://github.com/StefH/QRCode

![overview](https://github.com/StefH/Dutch-CoronaCheck-QR-Code-Decoder-and-Verifier/blob/main/resources/qr.png)

``` c#
var qrCodeData = "NL2:B4V.W9D:LWJ5W2S6A$XQ9N* Y252O4%%  ZNK**$840VPY8T7$J0GR$8L2%VMO/20/3C.C.L XO:FN%IWW.TI+G3KW2RA+ $6T1 BQAGU6HJ35D.2YPIT*6Y3C733IOBZIKEWP4L/$9TX6QUVQFFZWJ+RY/JV6N3%NX%Y4XX43J182O/.AELM1%E-D*Q+8*O1CG*9/5ENUJ0HXT*PJXJ*XE-6QFMM7*B$IFEY04:-PN14PX3% Q5-JQF9$YJFVBSUD*P/AXHJRNUIA:SCX*SBIQ*BHZG$PJ+LG-S*:0.GZ8M4HO.XLM$BKZG7H/BVRUW$7WH$B3$L-T58KK$20EDRZW1B*VJ1Q5VC:X/.5*OQJ/EA92-8J*-QL6J+3NX:C5%%XZ4LLIS31KKPA9:1FP++KT:.QFRZ%M5R$I2*DM36M%BW/3.LS9MX6YE8KU4S-.Q%W2ZCI7CQ79E/X342+5T3ODK8X.-F02J-GMF18KCE.5NDV2V8I/5L0GVNPQRF+T3A*$%HI3-$R3+*RO/X8N.RG7LBFJP5SO9QAD:KYRP978DTHFL39368JXWSO2CKLQTYDZ45CF0FE9J3$$6+ZX RSNRQ6+HV%DE$V:O/Q8FYO+.NZFL6R8R8UE.0:A*Q9$8HYB+WZ26UM%.4R4 25AA7XQW.NYAJCO6+-C QZEPKLYS6G0Z/YGHPR*+YKEDO*3LE:KP HT3NCJPRNRG9K0Y84*C-7N2-BQJXY/+D-VF IIQTJ6-AV83%1Y8JMXN1I6/JSHS+HEG+VU+8UX:LL*Y%B*$G$D4H9HZVMHKWT2-87UTR+EZIWJ*IOTQ70.V%CLHVY  2-HFW4BA6-+FWIW6C:WDS /FU2I9G$LXL$B/MY*WQYMN*R00IQZJJ- 6QFX*$%-9ZGS3%G";
```

### 2. Decode this string using the Dutch Base45 decoder

Make sure to skip the first 4 characters.

``` c#
var base45Decoded = DutchCoronaCheckBase45Utils.Decode(qrCodeData.Substring(4));
```

### 3. Decode the Base45 string using an ASN1 decoder

``` c#
var topLevelStructure = DutchCoronaCheckASN1Utils.Read(base45Decoded);
```

### 4. Print the info as json

Note that you need a specfic BigIntegerJsonConverter to show the BigInteger value correctly.

``` c#
var options = new JsonSerializerOptions
{
    WriteIndented = true
};
options.Converters.Add(new BigIntegerJsonConverter());
var json = JsonSerializer.Serialize(topLevelStructure, options);
```

### 5a. Decoded structure as JSON

``` json
{
  "DisclosureTimeSeconds": 0,
  "DisclosureTimeAsDateTime": null,
  "C": 8552691641371642315207690017304824071043033037718558536544370073022402689101,
  "A": 49268998317399832353736153768064371574626738522454049754099967437769709104310153609419233826877129804699795832362376712815509777453876457088841057013114352195643598128717108189839059529627445516705110559750713315964799043200407446690141492417247324685581653291574436663012628249066333485906592079172004705549,
  "EResponse": 49545515259154895522776867107959247032241047651980572443481917331143789403307139106555023968391340296360761238102083435099286993359857518,
  "VResponse": 7463954613777398261540270129601931803174222687130155761347994847425322491183229539732191198415826129672984792140024846081031144727556269931620272207040926338693607156199863455849443514025962426861121889546981280059686356901102682907001113258526002924172374302439709804314739830145508425336941142618773720272632415685260123356955161489134413604652892038340458427564488213207319660351733296444266493507445217438295664429771376840180237292580120261971321393055748802007069008017175471042800839302970504855484030669136380196262086189703554019361768087619371773861096653352702590133156777381984232268805659708171263123,
  "AResponse": 8995728107811269294519662596375119434427697506197921896277098071616873267360774813635548449786430125954107935654004828703451862806879219403683135630175078760507502448619423267753,
  "ADisclosed": {
    "Metadata": {
      "CredentialVersion": 2,
      "IssuerPkId": "VWS-CC-2"
    },
    "IsSpecimen": "1",
    "IsPaperProof": "1",
    "ValidFrom": "1627466400",
    "ValidFromAsDateTime": "2021-07-28T10:00:00",
    "ValidForHours": "25",
    "FirstNameInitial": "B",
    "LastNameInitial": "B",
    "BirthDay": "31",
    "BirthMonth": "7"
  }
}
```

### 5b. Decoded structure from real QR code (phone)
``` json
{
  "DisclosureTimeSeconds": 1636813015,
  "DisclosureTimeAsDateTime": "2021-11-13T14:16:55",
  "C": ???,
  "A": ???,
  "EResponse": ???,
  "VResponse": ???,
  "AResponse": ???,
  "ADisclosed": {
    "Metadata": {
      "CredentialVersion": 2,
      "IssuerPkId": "VWS-CC-1"
    },
    "IsSpecimen": "0",
    "IsPaperProof": "0",
    "ValidFrom": "1636797600",
    "ValidFromAsDateTime": "2021-11-13T10:00:00",
    "ValidForHours": "24",
    "FirstNameInitial": "???",
    "LastNameInitial": "???",
    "BirthDay": null,
    "BirthMonth": "???"
  }
}
```

##### Differences with the PaperProof version
- The `DisclosureTimeSeconds` defines the time when this code was generated/displayed on the screen.
- The `IssuerPkId` version is different
- The `BirthDay` is null
- The `ValidForHours` is 24 instead of 25
- The `ValidFrom` differs 1 hour