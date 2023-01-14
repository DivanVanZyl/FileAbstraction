# FileAbstraction

For shortened file IO syntax. 

This is an easy way of writing/reading files. This might be useful for state management, or quick prototyping.

For example, you can save files to the current directory with the syntax: `someObject.ToFile();`

For example: `256.ToFile();` This will write the number 256 to a file.

Then, you can display the file with `FileAbstract.DisplayFile();` (It will figure out how to find the file automatically)

You can also specify specific paths, but the FileAbstraction library will try to find your file based on the name,
even if it is not found. For example:
```
"Some file content".ToFile( "Some user directory", "myFile.txt"));
FileAbstraction.DisplayFile("myFile.txt");
```

You can serialise objects to/from files, for example:

```
var c = new Computer();
c.ToFile();
var savedC = FileAbstraction.ReadBinFile<Computer>();
Console.WriteLine(savedC.Name);
```
