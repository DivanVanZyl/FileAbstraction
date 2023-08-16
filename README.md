![Nuget](https://img.shields.io/nuget/v/Util.File.Abstraction?style=flat-square)
# FileAbstraction

For shortened file IO syntax. 

This is an easy way of writing/reading files. This might be useful for state management, or quick prototyping.

For example, you can save files to the current directory with the syntax: `someObject.ToFile();`

![EarlNotBadGIF](https://github.com/DivanVanZyl/FileAbstraction/assets/5897077/6688498d-412f-4b79-a4cf-d18247dac653)

For example: `256.ToFile();` This will write the number 256 to a file!

![ShooktShockedGIF](https://github.com/DivanVanZyl/FileAbstraction/assets/5897077/887e2a35-3110-4d36-8a21-1ee979ead0cb)

Then, you can display the file with `FileAbstract.DisplayFile();`

You can also specify specific paths:
```
"Some file content".ToFile( "Some user directory", "myFile.txt"));
FileAbstraction.DisplayFile("Some user directory", "myFile.txt");
```

You can serialise objects to/from files, for example:

```
var c = new Computer();
c.ToFile();
var savedC = FileAbstraction.ReadBinFile<Computer>();
Console.WriteLine(savedC.Name);
```
