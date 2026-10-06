// Copyright (c) 2026 Roger Brown.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;

namespace RhubarbGeekNz.AppleScript
{
    internal class NSAppleEventDescriptor
    {
        int nesting = 0;
        internal NSAppleEventDescriptor()
        {
        }

        internal static IntPtr stringValue = ObjC.sel_registerName("stringValue");
        internal static IntPtr doubleValue = ObjC.sel_registerName("doubleValue");
        internal static IntPtr dateValue = ObjC.sel_registerName("dateValue");
        internal static IntPtr descriptorType = ObjC.sel_registerName("descriptorType");
        internal static IntPtr data = ObjC.sel_registerName("data");
        internal static IntPtr typeCodeValue = ObjC.sel_registerName("typeCodeValue");
        internal static IntPtr timeIntervalSince1970 = ObjC.sel_registerName("timeIntervalSince1970");
        internal static IntPtr int32Value = ObjC.sel_registerName("int32Value");
        internal static IntPtr numberOfItems = ObjC.sel_registerName("numberOfItems");
        internal static IntPtr descriptorAtIndex = ObjC.sel_registerName("descriptorAtIndex:");
        internal static IntPtr descriptorWithString = ObjC.sel_registerName("descriptorWithString:");
        internal static IntPtr listDescriptor = ObjC.sel_registerName("listDescriptor");
        internal static IntPtr insertDescriptorAtIndex = ObjC.sel_registerName("insertDescriptor:atIndex:");
        internal static IntPtr nullDescriptor = ObjC.sel_registerName("nullDescriptor");
        internal static IntPtr descriptorWithInt32 = ObjC.sel_registerName("descriptorWithInt32:");
        internal static IntPtr descriptorWithBoolean = ObjC.sel_registerName("descriptorWithBoolean:");
        internal static IntPtr descriptorWithDouble = ObjC.sel_registerName("descriptorWithDouble:");
        internal static IntPtr dateWithTimeIntervalSince1970 = ObjC.sel_registerName("dateWithTimeIntervalSince1970:");
        internal static IntPtr descriptorWithDate = ObjC.sel_registerName("descriptorWithDate:");
        internal static IntPtr URLWithString = ObjC.sel_registerName("URLWithString:");
        internal static IntPtr descriptorWithFileURL = ObjC.sel_registerName("descriptorWithFileURL:");
        internal static IntPtr fileURLValue = ObjC.sel_registerName("fileURLValue");
        internal static IntPtr absoluteString = ObjC.sel_registerName("absoluteString");
        internal static IntPtr sendEventWithOptions = ObjC.sel_registerName("sendEventWithOptions:timeout:error:");
        internal static IntPtr recordDescriptor = ObjC.sel_registerName("recordDescriptor");
        internal static IntPtr setDescriptorForKeyword = ObjC.sel_registerName("setDescriptor:forKeyword:");
        internal static IntPtr isRecordDescriptor = ObjC.sel_registerName("isRecordDescriptor");
        internal static IntPtr descriptorForKeyword = ObjC.sel_registerName("descriptorForKeyword:");
        internal static IntPtr keywordForDescriptorAtIndex = ObjC.sel_registerName("keywordForDescriptorAtIndex:");
        internal static IntPtr fileURLWithPath = ObjC.sel_registerName("fileURLWithPath:");
        internal static IntPtr initWithContentsOfURLerror = ObjC.sel_registerName("initWithContentsOfURL:error:");
        internal const Int32 typeAERecord = 0x7265636F; // reco
        internal const Int32 typeApplicationBundleID = 0x62756E64; // bund
        internal const Int32 typeChar = 0x54455854; // TEXT
        internal const Int32 typeType = 0x74797065;  // type
        internal const Int32 typeUTF8Text = 0x75746638; // utf8
        internal const Int32 typeUnicodeText = 0x75747874; // utxt
        internal const Int32 typeUTF16ExternalRepresentation = 0x75743136; // ut16
        internal const Int32 typeNull = 0x6E756C6C; // null
        internal const Int32 typeFalse = 0x066616C73; // fals
        internal const Int32 typeTrue = 0x074727565; // true
        internal const Int32 typeIEEE64BitFloatingPoint = 0x646F7562; // doub
        internal const Int32 typeSInt32 = 0x6C6F6E67; // long
        internal const Int32 typeFileURL = 0x6675726C; // furl
        internal const Int32 typeSInt16 = 0x73686F72; // shor
        internal const Int32 typeLongDateTime = 0x6C647420; // ldt
        internal const Int32 typeAEList = 0x6C697374; // list
        internal const Int32 keyASUserRecordFields = 0x75737266; // usrf
        internal const Int32 keyAEDescType = 0x64737470; // dstp
        internal const Int32 keyAEData = 0x64617461; // data
        internal const Int32 keyAEClassID = 0x636c4944; // clID
        internal const Int32 keyDirectObject = 0x2D2D2D2D; // ----
        internal const Int32 keyASSubroutineName = 0x736E616D; // snam

        internal static IntPtr FromString(String str)
        {
            if (str == null) return ObjC.msgSend(NSClass.NSAppleEventDescriptor, nullDescriptor);
            IntPtr strPtr = NSString.FromString(str);
            return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithString, strPtr);
        }

        private static readonly IDictionary<Type, Func<object, IntPtr>> descriptorFromTable = new Dictionary<Type, Func<object, IntPtr>>()
        {
            {typeof(bool),o=>{bool b=(bool)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithBoolean, b);}},
            {typeof(byte),o=>{byte b=(byte)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithInt32, (int)b);}},
            {typeof(SByte),o=>{SByte sb=(SByte)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithInt32, (int)sb);}},
            {typeof(Int16),o=>{Int16 i16=(Int16)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithInt32, (int)i16);}},
            {typeof(UInt16),o=>{UInt16 u16=(UInt16)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithInt32, (int)u16);}},
            {typeof(Int32),o=>{Int32 i32=(Int32)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithInt32, i32);}},
            {typeof(UInt32),o=>{UInt32 u32=(UInt32)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithInt32, (int)u32);}},
            {typeof(Int64),o=>{Int64 i64=(Int64)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithInt32, (int)i64);}},
            {typeof(UInt64),o=>{UInt64 u64=(UInt64)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithInt32, (int)u64);}},
            {typeof(Int128),o=>{Int128 i128=(Int128)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithInt32, (int)i128);}},
            {typeof(UInt128),o=>{UInt128 u128=(UInt128)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithInt32, (int)u128);}},
            {typeof(IntPtr),o=>{IntPtr iPtr=(IntPtr)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithInt32, iPtr.ToInt32());}},
            {typeof(double),o=>{double d=(double)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithDouble, d);;}},
            {typeof(float),o=>{float d=(float)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithDouble, (double)d);;}},
            {typeof(decimal),o=>{decimal d=(decimal)o; return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithDouble, (double)d);;}},
            {typeof(char),o=>{return FromString(new String(new char[]{(char)o}));}},
            {typeof(String),o=>{return FromString((String)o);}}
        };

        internal IntPtr DescriptorFromObject(object obj)
        {
            if (obj == null)
            {
                return ObjC.msgSend(NSClass.NSAppleEventDescriptor, nullDescriptor);
            }

            if (descriptorFromTable.TryGetValue(obj.GetType(), out var func))
            {
                return func(obj);
            }

            if (obj is DateTime dt)
            {
                DateTime epoch = DateTime.UnixEpoch;
                DateTime targetDate = dt.ToUniversalTime();
                TimeSpan difference = targetDate - epoch;
                double diff = difference.TotalSeconds;
                IntPtr date = ObjC.msgSend(NSClass.NSDate, dateWithTimeIntervalSince1970, diff);
                return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithDate, date);
            }

            if (obj is Uri uri)
            {
                IntPtr uriPtr = ObjC.msgSend(NSClass.NSURL, URLWithString, NSString.FromString(uri.ToString()));
                return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithFileURL, uriPtr);
            }

            if (obj is XmlNode xml)
            {
                return FromString(xml.OuterXml);
            }

            if (obj is JsonNode node)
            {
                return FromString(JsonSerializer.Serialize(node));
            }

            if (obj is FileInfo fileInfo)
            {
                IntPtr uriPtr = ObjC.msgSend(NSClass.NSURL, fileURLWithPath, NSString.FromString(fileInfo.FullName));
                return ObjC.msgSend(NSClass.NSAppleEventDescriptor, descriptorWithFileURL, uriPtr);
            }

            if (nesting > 20)
            {
                throw new ParseException();
            }

            nesting++;

            try
            {
                if (obj is PSObject psobj)
                {
                    if (psobj.BaseObject is PSCustomObject pscust)
                    {
                        int n = 1;
                        IntPtr desc = ObjC.msgSend(NSClass.NSAppleEventDescriptor, recordDescriptor);
                        IntPtr list = ObjC.msgSend(NSClass.NSAppleEventDescriptor, listDescriptor);
                        foreach (var de in psobj.Properties)
                        {
                            if (de.MemberType == PSMemberTypes.NoteProperty)
                            {
                                IntPtr key = DescriptorFromObject(de.Name);
                                IntPtr value = DescriptorFromObject(de.Value);
                                ObjC.msgSend(list, insertDescriptorAtIndex, key, n++);
                                ObjC.msgSend(list, insertDescriptorAtIndex, value, n++);
                            }
                        }
                        ObjC.msgSend(desc, setDescriptorForKeyword, list, keyASUserRecordFields);
                        return desc;
                    }
                    else
                    {
                        return DescriptorFromObject(psobj.BaseObject);
                    }
                }

                if (obj is IDictionary d)
                {
                    int n = 1;
                    IntPtr desc = ObjC.msgSend(NSClass.NSAppleEventDescriptor, recordDescriptor);
                    bool isTrueRecord = true;
                    foreach (var k in d.Keys)
                    {
                        if (k is Int32 resType)
                        {
                            if (resType == keyASUserRecordFields)
                            {
                                isTrueRecord = false;
                                break;
                            }
                        }
                        else
                        {
                            isTrueRecord = false;
                            break;
                        }
                    }

                    if (isTrueRecord)
                    {
                        foreach (DictionaryEntry de in d)
                        {
                            Int32 key = (Int32)de.Key;
                            IntPtr value = DescriptorFromObject(de.Value);
                            ObjC.msgSend(desc, setDescriptorForKeyword, key, value);
                        }
                    }
                    else
                    {
                        IntPtr list = ObjC.msgSend(NSClass.NSAppleEventDescriptor, listDescriptor);
                        foreach (DictionaryEntry de in d)
                        {
                            IntPtr key = DescriptorFromObject(de.Key);
                            IntPtr value = DescriptorFromObject(de.Value);
                            ObjC.msgSend(list, insertDescriptorAtIndex, key, n++);
                            ObjC.msgSend(list, insertDescriptorAtIndex, value, n++);
                        }
                        ObjC.msgSend(desc, setDescriptorForKeyword, list, keyASUserRecordFields);
                    }
                    return desc;
                }

                if (obj is IEnumerable e)
                {
                    IntPtr listParams = ObjC.msgSend(NSClass.NSAppleEventDescriptor, listDescriptor);
                    int n = 1;

                    foreach (var o in e)
                    {
                        IntPtr desc = DescriptorFromObject(o);
                        ObjC.msgSend(listParams, insertDescriptorAtIndex, desc, n);
                        n++;
                    }

                    return listParams;
                }

                {
                    int n = 1;
                    IntPtr desc = ObjC.msgSend(NSClass.NSAppleEventDescriptor, recordDescriptor);
                    IntPtr list = ObjC.msgSend(NSClass.NSAppleEventDescriptor, listDescriptor);
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        var gm = prop.GetMethod;

                        if (gm != null)
                        {
                            if (!gm.IsStatic)
                            {
                                IntPtr key = FromString(prop.Name);
                                IntPtr value = DescriptorFromObject(prop.GetValue(obj));
                                ObjC.msgSend(list, insertDescriptorAtIndex, key, n++);
                                ObjC.msgSend(list, insertDescriptorAtIndex, value, n++);
                            }
                        }
                    }
                    ObjC.msgSend(desc, setDescriptorForKeyword, list, keyASUserRecordFields);

                    return desc;
                }
            }
            finally
            {
                nesting--;
            }
        }

        private static readonly IDictionary<int, Func<IntPtr, object>> objectFromDescriptor = new Dictionary<int, Func<IntPtr, object>>()
        {
            {typeIEEE64BitFloatingPoint,obj=>{return ObjC.msgSend_fpret(obj, doubleValue);}},
            {typeSInt32,obj=>{return (Int32)ObjC.msgSend(obj, int32Value);}},
            {typeSInt16,obj=>{return (Int16)ObjC.msgSend(obj, int32Value);}},
            {typeNull,obj=>{return null;}},
            {typeTrue,obj=>{return true;}},
            {typeFalse,obj=>{return false;}},
            {typeChar,obj=>{return NSString.ToString(ObjC.msgSend(obj, stringValue));}},
            {typeUnicodeText,obj=>{return NSString.ToString(ObjC.msgSend(obj, stringValue));}},
            {typeLongDateTime,obj=>{
                IntPtr dateObj = ObjC.msgSend(obj, dateValue);
                double seconds = ObjC.msgSend_fpret(dateObj, timeIntervalSince1970);
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(0);
                return dateTimeOffset.UtcDateTime.AddSeconds(seconds);}},
            {typeAEList,obj=>{
                IntPtr count = ObjC.msgSend(obj, numberOfItems);
                IntPtr index = 0;
                object[] result = new object[count];
                while (index < count)
                {
                    IntPtr item = ObjC.msgSend(obj, descriptorAtIndex, 1 + index);
                    result[index] = ObjectFromDescriptor(item);
                    index++;
                }
                return result;}},
            {typeType,obj=>{
                Int32 typeCode = (Int32)ObjC.msgSend(obj, typeCodeValue);
                if (typeCode == typeNull) // null
                {
                    return null;
                }
                Hashtable table = new Hashtable();
                table.Add("type", typeType);
                table.Add("data", typeCode);
                return table;}},
            {typeFileURL,obj=>{
                IntPtr urlPtr = ObjC.msgSend(obj, fileURLValue);
                IntPtr strPtr = ObjC.msgSend(urlPtr, absoluteString);
                String str = NSString.ToString(strPtr);
                return new Uri(str);}}
        };

        static internal object ObjectFromDescriptor(IntPtr obj)
        {
            if (obj == IntPtr.Zero) return null;

            if (0 != (byte)ObjC.msgSend(obj, isRecordDescriptor))
            {
                Hashtable table = new Hashtable();
                IntPtr list = ObjC.msgSend(obj, descriptorForKeyword, keyASUserRecordFields);

                if (list != IntPtr.Zero)
                {
                    long count = ObjC.msgSend(list, numberOfItems);
                    if (0 == (count & 1))
                    {
                        count >>= 1;
                        int n = 1;
                        while (0 != count--)
                        {
                            IntPtr name = ObjC.msgSend(list, descriptorAtIndex, n++);
                            IntPtr value = ObjC.msgSend(list, descriptorAtIndex, n++);
                            table.Add(ObjectFromDescriptor(name), ObjectFromDescriptor(value));
                        }
                    }
                }
                else
                {
                    long count = ObjC.msgSend(obj, numberOfItems);
                    int n = 1;
                    while (0 != count--)
                    {
                        Int32 name = (Int32)ObjC.msgSend(obj, keywordForDescriptorAtIndex, n);
                        IntPtr value = ObjC.msgSend(obj, descriptorAtIndex, n);
                        table.Add(name, ObjectFromDescriptor(value));
                        n++;
                    }
                }

                return table;
            }

            Int32 type = (Int32)ObjC.msgSend(obj, descriptorType);

            if (objectFromDescriptor.TryGetValue(type, out var func))
            {
                return func(obj);
            }

            Hashtable tableofLastResort = new Hashtable();
            IntPtr ptr = ObjC.msgSend(obj, data);
            object valueOfLastResort;
            if (ptr != IntPtr.Zero)
            {
                valueOfLastResort = NSData.GetBytes(ptr);
            }
            else
            {
                valueOfLastResort = null;
            }
            tableofLastResort.Add(keyAEDescType, type);
            tableofLastResort.Add(keyAEData, valueOfLastResort);
            return tableofLastResort;
        }

        static internal object ObjectFromIntPtr(IntPtr obj)
        {
            if (obj == IntPtr.Zero) return null;

            if (NSObject.IsKindOfClass(obj, NSClass.NSString))
            {
                return NSString.ToString(obj);
            }

            if (NSObject.IsKindOfClass(obj, NSClass.NSNumber))
            {
                object value;
                long numType = CoreFoundation.CFNumberGetType(obj);
                long numLen = CoreFoundation.CFNumberGetByteSize(obj);
                byte[] numBytes = new byte[numLen];

                if (CoreFoundation.CFNumberGetValue(obj, numType, numBytes))
                {
                    switch (numLen)
                    {
                        case 1:
                            value = numBytes[0];
                            break;
                        case 2:
                            value = BitConverter.ToInt16(numBytes, 0);
                            break;
                        case 4:
                            value = BitConverter.ToInt32(numBytes, 0);
                            break;
                        case 8:
                            value = BitConverter.ToInt64(numBytes, 0);
                            break;
                        default:
                            throw new Exception($"number {numType} {numLen} unknown length");
                    }
                }
                else
                {
                    throw new Exception($"number {numType} {numLen} get failed");
                }

                return value;
            }

            if (NSObject.IsKindOfClass(obj, NSClass.NSConcreteValue))
            {
                String objCType = NSValue.ObjCType(obj);
                byte[] bytes = NSValue.GetValue(obj);
                switch (objCType)
                {
                    case "{_NSRange=QQ}":
                        return new Int64[]{
                            BitConverter.ToInt64(bytes,0),
                            BitConverter.ToInt64(bytes,bytes.Length>>1)};
                    default:
                        Hashtable valDict = new Hashtable();
                        valDict[keyAEClassID] = objCType;
                        valDict[keyAEData] = bytes;
                        return valDict;
                }
            }

            if (NSObject.IsKindOfClass(obj, NSClass.NSDictionary))
            {
                Hashtable dict = new Hashtable();
                long count = CoreFoundation.CFDictionaryGetCount(obj);

                if (count > 0)
                {
                    IntPtr[] keys = new IntPtr[count];
                    IntPtr[] values = new IntPtr[count];
                    CoreFoundation.CFDictionaryGetKeysAndValues(obj, keys, values);
                    int i = 0;

                    while (i < count)
                    {
                        IntPtr keyPtr = keys[i];
                        IntPtr valuePtr = values[i];
                        object key = ObjectFromIntPtr(keyPtr);
                        object value;

                        if (valuePtr != IntPtr.Zero)
                        {
                            value = ObjectFromIntPtr(valuePtr);
                        }
                        else
                        {
                            value = null;
                        }

                        dict.Add(key, value);

                        i++;
                    }
                }

                return dict;
            }

            throw new Exception("unknown class " + ObjC.GetObjCClassName(obj));
        }
    }
}
