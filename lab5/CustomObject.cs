using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class CustomObject
    {
        int intValue;
        string stringValue;
        float floatValue;
        char charValue;
        List<int> intArray;
        List<float> floatArray;

        public string Type { get; set; }

        public object Value
        {
            get
            {
                switch (Type)
                {
                    case "int":
                        return intValue;
                    case "string":
                        return stringValue;
                    case "char":
                        return charValue;
                    case "float":
                        return floatValue;
                    case "int array":
                        return intArray;
                    case "float array":
                        return floatArray;
                    default:
                        return null;
                }

            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator CustomObject(int i)
        {
            return new CustomObject { intValue = i, floatValue= i, Type = "int" };
        }
        public static implicit operator CustomObject(string s)
        {
            return new CustomObject { stringValue = s, Type = "string" };
        }
        public static implicit operator CustomObject(float f)
        {
            return new CustomObject { floatValue = f, Type = "float" };
        }
        public static implicit operator CustomObject(char c)
        {
            return new CustomObject { charValue = c, Type = "char" };
        }
        public static implicit operator CustomObject(List<int> li)
        {
            return new CustomObject { intArray = li, Type = "int array" };
        }
        public static implicit operator CustomObject(List<float> lf)
        {
            return new CustomObject { floatArray = lf, Type = "float array" };
        }

        public static implicit operator int(CustomObject item)
        {
            return item.intValue;
        }
        public static implicit operator string(CustomObject item)
        {
            return item.stringValue;
        }
        public static implicit operator float(CustomObject item)
        {
            return item.floatValue;
        }
        public static implicit operator char(CustomObject item)
        {
            return item.charValue;
        }
        public static implicit operator List<int>(CustomObject item)
        {
            return item.intArray;
        }
        public static implicit operator List<float>(CustomObject item)
        {
            return item.floatArray;
        }
        public static CustomObject operator +(CustomObject item1, CustomObject item2)
        {
            switch (item1.Type)
            {
                case "int":
                    if (item2.Type == "float")
                    {
                        return item1.intValue + item2.floatValue;
                    }
                    return item1.intValue + item2.intValue;
                    break;
                case "float":
                    if (item2.Type == "int")
                    {
                        return item1.floatValue + item2.intValue;
                    }
                    return item1.floatValue + item2.floatValue;
                    break;
                case "string":
                    return item1.stringValue + item2.stringValue;
                    break;
                case "char":
                    return item1.charValue + item2.charValue;
                    break;
            }
            return null;
        }
        public static CustomObject operator -(CustomObject item1, CustomObject item2)
        {
            switch (item1.Type)
            {
                case "int":
                    if (item2.Type == "float")
                    {
                        return item1.intValue - item2.floatValue;
                    }
                    return item1.intValue - item2.intValue;
                    break;
                case "float":
                    if (item2.Type == "int")
                    {
                        return item1.floatValue - item2.intValue;
                    }
                    return item1.floatValue - item2.floatValue;
                    break;
                case "char":
                    return item1.charValue - item2.charValue;
                    break;
            }
            return null;
        }
        public static CustomObject operator *(CustomObject item1, CustomObject item2)
        {
            switch (item1.Type)
            {
                case "int":
                    if (item2.Type == "float")
                    {
                        return item1.intValue * item2.floatValue;
                    }
                    return item1.intValue * item2.intValue;
                    break;
                case "float":
                    if (item2.Type == "int")
                    {
                        return item1.floatValue * item2.intValue;
                    }
                    return item1.floatValue * item2.floatValue;
                    break;
            }
            return null;
        }
        public static CustomObject operator /(CustomObject item1, CustomObject item2)
        {
            switch (item1.Type)
            {
                case "int":
                    if (item2.Type == "float")
                    {
                        return item1.intValue / item2.floatValue;
                    }
                    return item1.intValue / item2.intValue;
                    break;
                case "float":
                    if (item2.Type == "int")
                    {
                        return item1.floatValue / item2.intValue;
                    }
                    return item1.floatValue / item2.floatValue;
                    break;
            }
            return null;
        }
        public static CustomObject operator %(CustomObject item1, CustomObject item2)
        {
            switch (item1.Type)
            {
                case "int":
                    return item1.intValue % item2.intValue;
                    break;
            }
            return null;
        }
        public static bool operator >(CustomObject item1, CustomObject item2)
        {
            switch (item1.Type)
            {
                case "int":
                    if (item2.Type == "float")
                    {
                        return item1.intValue > item2.floatValue;
                    }
                    if (item2.Type == "char")
                    {
                        return item1.intValue > item2.charValue;
                    }
                    return item1.intValue > item2.intValue;
                    break;
                case "float":
                    if (item2.Type == "int")
                    {
                        return item1.floatValue > item2.intValue;
                    }
                    return item1.floatValue > item2.floatValue;
                    break;
                case "char":
                    return item1.charValue > item2.charValue;
                    break;
            }
            return false;
        }
        public static bool operator <(CustomObject item1, CustomObject item2)
        {
            switch (item1.Type)
            {
                case "int":
                    if (item2.Type == "float")
                    {
                        return item1.intValue < item2.floatValue;
                    }
                    if (item2.Type == "char")
                    {
                        return item1.intValue < item2.charValue;
                    }
                    return item1.intValue < item2.intValue;
                    break;
                case "float":
                    if (item2.Type == "int")
                    {
                        return item1.floatValue < item2.intValue;
                    }
                    return item1.floatValue < item2.floatValue;
                    break;
                case "char":
                    return item1.charValue < item2.charValue;
                    break;
            }
            return false;
        }
        public static bool operator >=(CustomObject item1, CustomObject item2)
        {
            switch (item1.Type)
            {
                case "int":
                    if (item2.Type == "float")
                    {
                        return item1.intValue >= item2.floatValue;
                    }
                    if (item2.Type == "char")
                    {
                        return item1.intValue >= item2.charValue;
                    }
                    return item1.intValue >= item2.intValue;
                    break;
                case "float":
                    if (item2.Type == "int")
                    {
                        return item1.floatValue >= item2.intValue;
                    }
                    return item1.floatValue >= item2.floatValue;
                    break;
                case "char":
                    return item1.charValue >= item2.charValue;
                    break;
            }
            return false;
        }
        public static bool operator <=(CustomObject item1, CustomObject item2)
        {
            switch (item1.Type)
            {
                case "int":
                    if (item2.Type == "float")
                    {
                        return item1.intValue <= item2.floatValue;
                    }
                    if (item2.Type == "char")
                    {
                        return item1.intValue <= item2.charValue;
                    }
                    return item1.intValue <= item2.intValue;
                    break;
                case "float":
                    if (item2.Type == "int")
                    {
                        return item1.floatValue <= item2.intValue;
                    }
                    return item1.floatValue <= item2.floatValue;
                    break;
                case "char":
                    return item1.charValue <= item2.charValue;
                    break;
            }
            return false;
        }
        public static bool operator !=(CustomObject item1, CustomObject item2)
        {
            switch (item1.Type)
            {
                case "int":
                    if (item2.Type == "float")
                    {
                        return item1.intValue != item2.floatValue;
                    }
                    if (item2.Type == "char")
                    {
                        return item1.intValue != item2.charValue;
                    }
                    return item1.intValue != item2.intValue;
                    break;
                case "float":
                    if (item2.Type == "int")
                    {
                        return item1.floatValue != item2.intValue;
                    }
                    return item1.floatValue != item2.floatValue;
                    break;
                case "string":
                    return item1.stringValue != item2.stringValue;
                    break;
                case "char":
                    return item1.charValue != item2.charValue;
                    break;
            }
            return false;
        }
        public static bool operator ==(CustomObject item1, CustomObject item2)
        {
            switch (item1.Type)
            {
                case "int":
                    if (item2.Type == "float")
                    {
                        return item1.intValue == item2.floatValue;
                    }
                    if (item2.Type == "char")
                    {
                        return item1.intValue == item2.charValue;
                    }
                    return item1.intValue == item2.intValue;
                    break;
                case "float":
                    if (item2.Type == "int")
                    {
                        return item1.floatValue == item2.intValue;
                    }
                    return item1.floatValue == item2.floatValue;
                    break;
                case "string":
                    return item1.stringValue == item2.stringValue;
                    break;
                case "char":
                    return item1.charValue == item2.charValue;
                    break;
            }
            return false;
        }
        public static CustomObject operator ++(CustomObject item)
        {
            switch (item.Type)
            {
                case "int":
                    return item.intValue += 1;
                    break;
            }
            return null;
        }
        public static CustomObject operator --(CustomObject item)
        {
            switch (item.Type)
            {
                case "int":
                    return item.intValue -= 1;
                    break;
            }
            return null;
        }

        public CustomObject this[int index]
        {
            get
            {
                switch(Type)
                {
                    case "int array":
                        return intArray[index];
                        break;
                    case "float array":
                        return floatArray[index];
                        break;
                    case "string":
                        return stringValue[index];
                        break;
                    default:
                        return null;
                }
            }
            set
            {
                switch (Type)
                {
                    case "int array":
                        intArray[index] = value;
                        break;
                    case "float array":
                        floatArray[index] = value;
                        break;
                    case "string":
                        stringValue.Remove(index).Insert(index, value);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
