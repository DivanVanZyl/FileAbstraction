﻿using FileAbstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstractionTests
{
    public class BinaryFilesTests
    {
        [Fact]
        public void WriteObjectAsBinary()
        {
            try
            {
                256.ToFile();
            }
            catch
            {
                Assert.Fail("Writing object as binary to a file threw an exception.");
            }
        }

        [Fact]
        public void ReadFileAsByteArray()
        {
            string text = "This is a test.";
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            bytes.ToFile();
            var savedBytes = FileAbstract.ReadBinFile();
            Assert.Equal(Encoding.UTF8.GetString( bytes), Encoding.UTF8.GetString(savedBytes));
        }

        [Fact]
        public void ReadFileAsObjectDefault()
        {
            var c = new Computer();
            c.ToFile();
            var savedC = FileAbstract.ReadBinFile<Computer>();
            Assert.NotNull(savedC);
            Assert.Equal(c.Name, savedC.Name);
        }

        [Fact]
        public void ReadFileAsObject()
        {
            var p = new Person("John Doe");
            p.ToFile();
            var savedC = FileAbstract.ReadBinFile<Person>();
            Assert.NotNull(savedC);
            Assert.Equal(p.Name, savedC.Name);
        }

        [Fact]
        public void ListOfObjects()
        {
            var list = new List<string>
{
    "This",
    "is",
    "a",
    "collection"
};

            list.ToFile();
            var savedList = FileAbstract.ReadBinFile<List<string>>();
            Assert.NotNull(savedList);
            Assert.Equal(list, savedList);
        }
    }

    internal class Computer
    {
        public Computer()
        {
            Name = Environment.MachineName;
            _description = RuntimeInformation.ProcessArchitecture;
        }
        public string Name { get; }
        private Architecture _description;
    }

    internal class Person
    {
        public string Name { get; }
        public Person(string name)
        {
            Name = name;
        }
    }
}
