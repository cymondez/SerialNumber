# SerialNumber - 建立流水號的抽號機套件

## 安裝
```
Install-Package SerialNumber
```

## 如何使用

本套件可以直接使用物件定義流水號格式，亦可使用套鑑定義的格式化字串

### 使用*NumberDisk*物件定義

```csharp
using SerialNumber;
using SerialNumber.CommonNumerDisks;

//建立流水號 從 Test-α-A-00 至 Test-γ-Z-99
Dispenser dispenser = new Dispenser(
                new ConstantNumberDisk('T'),
                new ConstantNumberDisk('e'),
                new ConstantNumberDisk('s'),
                new ConstantNumberDisk('t'),
                new ConstantNumberDisk('-'),
                new CustomNumberDisk("Greek", "αβγ".ToCharArray()),
                new ConstantNumberDisk('-') ,
                new AlphaNumberDisk(),
                new ConstantNumberDisk('-') ,
                new DigitalNumberDisk(),
                new DigitalNumberDisk()
            );

foreach(var sn in dispenser){
    Console.WriteLine(sn);
}
```

### 使用格式化字串定義

```csharp
using SerialNumber;

// \a ，代表大寫英文字母 ，範圍值 ： A-Z 
// \d ，代表十進位數值，範圍值： 0-9
// \c:'αβγ' ，代表自定義，範圍值為 單引號'' 內的每個字元 ，此例為 α-γ
// 其餘皆為常值 不會跳號
//建立流水號 從 Test-α-A-00 至 Test-γ-Z-99
var dispenser = FormatParser.Parse(@"Test-\c:'αβγ'-\a-\d\d");

foreach(var sn in dispenser){
    Console.WriteLine(sn);
}
```

### 設定流水號的起始值

使用基本方式設定起始值

```csharp
using SerialNumber;
using SerialNumber.CommonNumerDisks;
//只有SetableNumberDisk 的衍伸類別能設定初始值
//起始值為 Test-β-Z-90
Dispenser dispenser = new Dispenser(
            new ConstantNumberDisk('T'),
            new ConstantNumberDisk('e'),
            new ConstantNumberDisk('s'),
            new ConstantNumberDisk('t'),
            new ConstantNumberDisk('-'),
            new CustomNumberDisk("Greek", "αβγ".ToCharArray()){Skip = 1},// β
            new ConstantNumberDisk('-') ,
            new AlphaNumberDisk(){Skip = 25}, // Z
            new ConstantNumberDisk('-') ,
            new DigitalNumberDisk(){Skip=9}, // 9
            new DigitalNumberDisk()
            );
```

使用格式化字串時設定

```csharp
using SerialNumber;

// 在 變動數值後面加{} ，內部數值為起始的index值(從0起編)
//起始值為 Test-β-Z-90
var dispenser = FormatParser.Parse(@"Test-\c:'αβγ'{1}-\a{25}-\d{9}\d");

```
使用擴充方法設定

```csharp
using SerialNumer

var dispenser = FormatParser.Parse(@"Test-\c:'αβγ'-\a-\d\d");
//起始值為 Test-β-Z-90
dispenser.SetStartNumber("Test-β-Z-90");//如果設定字串的格式與流水號不合，將擲出FormatException
```