﻿using Swifter.Readers;
using Swifter.Writers;
using System;

namespace Swifter.RW
{
    internal sealed class AssemblyInterface<T> : IValueInterface<T> where T : System.Reflection.Assembly
    {
        public T ReadValue(IValueReader valueReader)
        {
            if (valueReader is IValueReader<T> tReader)
            {
                return tReader.ReadValue();
            }

            if (valueReader is IValueReader<System.Reflection.Assembly> assemblyReader)
            {
                return (T)assemblyReader.ReadValue();
            }

            var value = valueReader.DirectRead();

            if (value == null)
            {
                return null;
            }

            if (value is T tValue)
            {
                return tValue;
            }

            if (value is string sValue)
            {
                return (T)System.Reflection.Assembly.Load(sValue);
            }

            throw new NotSupportedException($"Cannot Read a 'Assembly' by '{value}'.");
        }

        public void WriteValue(IValueWriter valueWriter, T value)
        {
            if (value == null)
            {
                valueWriter.DirectWrite(null);

                return;
            }

            if (valueWriter is IValueWriter<T> tWriter)
            {
                tWriter.WriteValue(value);

                return;
            }

            if (valueWriter is IValueWriter<System.Reflection.Assembly> assemblyWriter)
            {
                assemblyWriter.WriteValue(value);

                return;
            }

            valueWriter.WriteString(value.FullName);
        }
    }
}