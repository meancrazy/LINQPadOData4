using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.OData.Edm;

namespace OData4.Builder
{
    /// <summary>
    /// Utility class.
    /// </summary>    
    public static class Utils
    {
        /// <summary>
        /// Serializes the xml element to a string.
        /// </summary>
        /// <param name="xml">The xml element to serialize.</param>
        /// <returns>The string representation of the xml.</returns>
        internal static string SerializeToString(XElement xml)
        {
            // because comment nodes can contain special characters that are hard to embed in VisualBasic, remove them here
            xml.DescendantNodes().OfType<XComment>().Remove();

            var stringBuilder = new StringBuilder();
            using (var writer = XmlWriter.Create(
                stringBuilder,
                new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    NewLineHandling = NewLineHandling.Replace,
                    Indent = true
                }))
            {
                xml.WriteTo(writer);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Changes the text to use camel case, which lower case for the first character.
        /// </summary>
        /// <param name="text">Text to convert.</param>
        /// <returns>The converted text in camel case</returns>
        internal static string CamelCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (text.Length == 1)
            {
                return text[0].ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            }

            return text[0].ToString(CultureInfo.InvariantCulture).ToLowerInvariant() + text.Substring(1);
        }

        /// <summary>
        /// Gets the clr type name from the give type reference.
        /// </summary>
        /// <param name="edmTypeReference">The type reference in question.</param>
        /// <param name="useDataServiceCollection">true to use the DataServicCollection type for entity collections and the ObservableCollection type for non-entity collections,
        /// false to use Collection for collections.</param>
        /// <param name="clientTemplate">ODataClientTemplate instance that call this method.</param>
        /// <param name="context">CodeGenerationContext instance in the clientTemplate.</param>
        /// <param name="addNullableTemplate">This flag indicates whether to return the type name in nullable format</param>
        /// <param name="needGlobalPrefix">The flag indicates whether the namespace need to be added by global prefix</param>
        /// <param name="isOperationParameter">This flag indicates whether the edmTypeReference is for an operation parameter</param>
        /// <returns>The clr type name of the type reference.</returns>
        internal static string GetClrTypeName(IEdmTypeReference edmTypeReference, bool useDataServiceCollection, ODataClientTemplate clientTemplate, CodeGenerationContext context, bool addNullableTemplate = true, bool needGlobalPrefix = true, bool isOperationParameter = false)
        {
            string clrTypeName;
            var edmType = edmTypeReference.Definition;
            var edmPrimitiveType = edmType as IEdmPrimitiveType;
            if (edmPrimitiveType != null)
            {
                clrTypeName = GetClrTypeName(edmPrimitiveType, clientTemplate);
                if (edmTypeReference.IsNullable && !clientTemplate.ClrReferenceTypes.Contains(edmPrimitiveType.PrimitiveKind) && addNullableTemplate)
                {
                    clrTypeName = string.Format(clientTemplate.SystemNullableStructureTemplate, clrTypeName);
                }
            }
            else
            {
                var edmComplexType = edmType as IEdmComplexType;
                if (edmComplexType != null)
                {
                    clrTypeName = context.GetPrefixedFullName(edmComplexType,
                        context.EnableNamingAlias ? clientTemplate.GetFixedName(Customization.CustomizeNaming(edmComplexType.Name)) : clientTemplate.GetFixedName(edmComplexType.Name), clientTemplate);
                }
                else
                {
                    var edmEnumType = edmType as IEdmEnumType;
                    if (edmEnumType != null)
                    {
                        clrTypeName = context.GetPrefixedFullName(edmEnumType,
                            context.EnableNamingAlias ? clientTemplate.GetFixedName(Customization.CustomizeNaming(edmEnumType.Name)) : clientTemplate.GetFixedName(edmEnumType.Name), clientTemplate, needGlobalPrefix);
                        if (edmTypeReference.IsNullable && addNullableTemplate)
                        {
                            clrTypeName = string.Format(clientTemplate.SystemNullableStructureTemplate, clrTypeName);
                        }
                    }
                    else
                    {
                        var edmEntityType = edmType as IEdmEntityType;
                        if (edmEntityType != null)
                        {
                            clrTypeName = context.GetPrefixedFullName(edmEntityType,
                                context.EnableNamingAlias ? clientTemplate.GetFixedName(Customization.CustomizeNaming(edmEntityType.Name)) : clientTemplate.GetFixedName(edmEntityType.Name), clientTemplate);
                        }
                        else
                        {
                            var edmCollectionType = (IEdmCollectionType)edmType;
                            var elementTypeReference = edmCollectionType.ElementType;
                            var primitiveElementType = elementTypeReference.Definition as IEdmPrimitiveType;
                            if (primitiveElementType != null)
                            {
                                clrTypeName = GetClrTypeName(primitiveElementType, clientTemplate);
                            }
                            else
                            {
                                var schemaElement = (IEdmSchemaElement)elementTypeReference.Definition;
                                clrTypeName = context.GetPrefixedFullName(schemaElement,
                                    context.EnableNamingAlias ? clientTemplate.GetFixedName(Customization.CustomizeNaming(schemaElement.Name)) : clientTemplate.GetFixedName(schemaElement.Name), clientTemplate);
                            }

                            var collectionTypeName = isOperationParameter
                                ? clientTemplate.CollectionOfTStructureTemplate
                                : (useDataServiceCollection
                                    ? (elementTypeReference.TypeKind() == EdmTypeKind.Entity
                                        ? clientTemplate.DataServiceCollectionStructureTemplate
                                        : clientTemplate.ObservableCollectionStructureTemplate)
                                    : clientTemplate.ObjectModelCollectionStructureTemplate);

                            clrTypeName = string.Format(collectionTypeName, clrTypeName);
                        }
                    }
                }
            }

            return clrTypeName;
        }

        /// <summary>
        /// Gets the value expression to initualize the property with.
        /// </summary>
        /// <param name="property">The property in question.</param>
        /// <param name="useDataServiceCollection">true to use the DataServicCollection type for entity collections and the ObservableCollection type for non-entity collections,
        /// false to use Collection for collections.</param>
        /// <param name="clientTemplate">ODataClientTemplate instance that call this method.</param>
        /// <param name="context">CodeGenerationContext instance in the clientTemplate.</param>
        /// <returns>The value expression to initualize the property with.</returns>
        internal static string GetPropertyInitializationValue(IEdmProperty property, bool useDataServiceCollection, ODataClientTemplate clientTemplate, CodeGenerationContext context)
        {
            var edmTypeReference = property.Type;
            var edmCollectionTypeReference = edmTypeReference as IEdmCollectionTypeReference;
            if (edmCollectionTypeReference == null)
            {
                var structuredProperty = property as IEdmStructuralProperty;
                if (structuredProperty != null)
                {
                    if (!string.IsNullOrEmpty(structuredProperty.DefaultValueString))
                    {
                        var valueClrType = GetClrTypeName(edmTypeReference, useDataServiceCollection, clientTemplate, context);
                        var defaultValue = structuredProperty.DefaultValueString;
                        var isCSharpTemplate = clientTemplate is ODataClientCSharpTemplate;
                        if (edmTypeReference.Definition.TypeKind == EdmTypeKind.Enum)
                        {
                            var enumValues = defaultValue.Split(',');
                            var fullenumTypeName = GetClrTypeName(edmTypeReference, useDataServiceCollection, clientTemplate, context);
                            var enumTypeName = GetClrTypeName(edmTypeReference, useDataServiceCollection, clientTemplate, context, false, false);
                            var customizedEnumValues = new List<string>();
                            foreach (var enumValue in enumValues)
                            {
                                var currentEnumValue = enumValue.Trim();
                                var indexFirst = currentEnumValue.IndexOf('\'') + 1;
                                var indexLast = currentEnumValue.LastIndexOf('\'');
                                if (indexFirst > 0 && indexLast > indexFirst)
                                {
                                    currentEnumValue = currentEnumValue.Substring(indexFirst, indexLast - indexFirst);
                                }

                                var customizedEnumValue = context.EnableNamingAlias ? Customization.CustomizeNaming(currentEnumValue) : currentEnumValue;
                                if (isCSharpTemplate)
                                {
                                    currentEnumValue = "(" + fullenumTypeName + ")" + clientTemplate.EnumTypeName + ".Parse(" + clientTemplate.SystemTypeTypeName + ".GetType(\"" + enumTypeName + "\"), \"" + customizedEnumValue + "\")";
                                }
                                else
                                {
                                    currentEnumValue = clientTemplate.EnumTypeName + ".Parse(" + clientTemplate.SystemTypeTypeName + ".GetType(\"" + enumTypeName + "\"), \"" + currentEnumValue + "\")";
                                }
                                customizedEnumValues.Add(currentEnumValue);
                            }
                            if (isCSharpTemplate)
                            {
                                return string.Join(" | ", customizedEnumValues);
                            }
                            return string.Join(" Or ", customizedEnumValues);
                        }

                        if (valueClrType.Equals(clientTemplate.StringTypeName))
                        {
                            defaultValue = "\"" + defaultValue + "\"";
                        }
                        else if (valueClrType.Equals(clientTemplate.BinaryTypeName))
                        {
                            defaultValue = "System.Text.Encoding.UTF8.GetBytes(\"" + defaultValue + "\")";
                        }
                        else if (valueClrType.Equals(clientTemplate.SingleTypeName))
                        {
                            if (isCSharpTemplate)
                            {
                                defaultValue = defaultValue.EndsWith("f", StringComparison.OrdinalIgnoreCase) ? defaultValue : defaultValue + "f";
                            }
                            else
                            {
                                defaultValue = defaultValue.EndsWith("f", StringComparison.OrdinalIgnoreCase) ? defaultValue : defaultValue + "F";
                            }
                        }
                        else if (valueClrType.Equals(clientTemplate.DecimalTypeName))
                        {
                            if (isCSharpTemplate)
                            {
                                // decimal in C# must be initialized with 'm' at the end, like Decimal dec = 3.00m
                                defaultValue = defaultValue.EndsWith("m", StringComparison.OrdinalIgnoreCase) ? defaultValue : defaultValue + "m";
                            }
                            else
                            {
                                // decimal in VB must be initialized with 'D' at the end, like Decimal dec = 3.00D
                                defaultValue = defaultValue.ToLower().Replace("m", "D");
                                defaultValue = defaultValue.EndsWith("D", StringComparison.OrdinalIgnoreCase) ? defaultValue : defaultValue + "D";
                            }
                        }
                        else if (valueClrType.Equals(clientTemplate.GuidTypeName)
                                 | valueClrType.Equals(clientTemplate.DateTimeOffsetTypeName)
                                 | valueClrType.Equals(clientTemplate.DateTypeName)
                                 | valueClrType.Equals(clientTemplate.TimeOfDayTypeName))
                        {
                            defaultValue = valueClrType + ".Parse(\"" + defaultValue + "\")";
                        }
                        else if (valueClrType.Equals(clientTemplate.DurationTypeName))
                        {
                            defaultValue = clientTemplate.XmlConvertClassName + ".ToTimeSpan(\"" + defaultValue + "\")";
                        }
                        else if (valueClrType.Contains("Microsoft.Spatial"))
                        {
                            defaultValue = string.Format(clientTemplate.GeoTypeInitializePattern, valueClrType, defaultValue);
                        }

                        return defaultValue;
                    }
                    // doesn't have a default value 
                    return null;
                }
                // only structured property has default value
                return null;
            }
            string constructorParameters;
            if (edmCollectionTypeReference.ElementType().IsEntity() && useDataServiceCollection)
            {
                constructorParameters = clientTemplate.DataServiceCollectionConstructorParameters;
            }
            else
            {
                constructorParameters = "()";
            }

            var clrTypeName = GetClrTypeName(edmTypeReference, useDataServiceCollection, clientTemplate, context);
            return clientTemplate.NewModifier + clrTypeName + constructorParameters;
        }

        /// <summary>
        /// Gets the clr type name from the give Edm primitive type.
        /// </summary>
        /// <param name="edmPrimitiveType">The Edm primitive type in question.</param>
        /// <param name="clientTemplate">ODataClientTemplate instance that call this method.</param>
        /// <returns>The clr type name of the Edm primitive type.</returns>
        internal static string GetClrTypeName(IEdmPrimitiveType edmPrimitiveType, ODataClientTemplate clientTemplate)
        {
            var kind = edmPrimitiveType.PrimitiveKind;

            string type;
            switch (kind)
            {
                case EdmPrimitiveTypeKind.Int32:
                    type = clientTemplate.Int32TypeName;
                    break;
                case EdmPrimitiveTypeKind.String:
                    type = clientTemplate.StringTypeName;
                    break;
                case EdmPrimitiveTypeKind.Binary:
                    type = clientTemplate.BinaryTypeName;
                    break;
                case EdmPrimitiveTypeKind.Decimal:
                    type = clientTemplate.DecimalTypeName;
                    break;
                case EdmPrimitiveTypeKind.Int16:
                    type = clientTemplate.Int16TypeName;
                    break;
                case EdmPrimitiveTypeKind.Single:
                    type = clientTemplate.SingleTypeName;
                    break;
                case EdmPrimitiveTypeKind.Boolean:
                    type = clientTemplate.BooleanTypeName;
                    break;
                case EdmPrimitiveTypeKind.Double:
                    type = clientTemplate.DoubleTypeName;
                    break;
                case EdmPrimitiveTypeKind.Guid:
                    type = clientTemplate.GuidTypeName;
                    break;
                case EdmPrimitiveTypeKind.Byte:
                    type = clientTemplate.ByteTypeName;
                    break;
                case EdmPrimitiveTypeKind.Int64:
                    type = clientTemplate.Int64TypeName;
                    break;
                case EdmPrimitiveTypeKind.SByte:
                    type = clientTemplate.SByteTypeName;
                    break;
                case EdmPrimitiveTypeKind.Stream:
                    type = clientTemplate.DataServiceStreamLinkTypeName;
                    break;
                case EdmPrimitiveTypeKind.Geography:
                    type = clientTemplate.GeographyTypeName;
                    break;
                case EdmPrimitiveTypeKind.GeographyPoint:
                    type = clientTemplate.GeographyPointTypeName;
                    break;
                case EdmPrimitiveTypeKind.GeographyLineString:
                    type = clientTemplate.GeographyLineStringTypeName;
                    break;
                case EdmPrimitiveTypeKind.GeographyPolygon:
                    type = clientTemplate.GeographyPolygonTypeName;
                    break;
                case EdmPrimitiveTypeKind.GeographyCollection:
                    type = clientTemplate.GeographyCollectionTypeName;
                    break;
                case EdmPrimitiveTypeKind.GeographyMultiPolygon:
                    type = clientTemplate.GeographyMultiPolygonTypeName;
                    break;
                case EdmPrimitiveTypeKind.GeographyMultiLineString:
                    type = clientTemplate.GeographyMultiLineStringTypeName;
                    break;
                case EdmPrimitiveTypeKind.GeographyMultiPoint:
                    type = clientTemplate.GeographyMultiPointTypeName;
                    break;
                case EdmPrimitiveTypeKind.Geometry:
                    type = clientTemplate.GeometryTypeName;
                    break;
                case EdmPrimitiveTypeKind.GeometryPoint:
                    type = clientTemplate.GeometryPointTypeName;
                    break;
                case EdmPrimitiveTypeKind.GeometryLineString:
                    type = clientTemplate.GeometryLineStringTypeName;
                    break;
                case EdmPrimitiveTypeKind.GeometryPolygon:
                    type = clientTemplate.GeometryPolygonTypeName;
                    break;
                case EdmPrimitiveTypeKind.GeometryCollection:
                    type = clientTemplate.GeometryCollectionTypeName;
                    break;
                case EdmPrimitiveTypeKind.GeometryMultiPolygon:
                    type = clientTemplate.GeometryMultiPolygonTypeName;
                    break;
                case EdmPrimitiveTypeKind.GeometryMultiLineString:
                    type = clientTemplate.GeometryMultiLineStringTypeName;
                    break;
                case EdmPrimitiveTypeKind.GeometryMultiPoint:
                    type = clientTemplate.GeometryMultiPointTypeName;
                    break;
                case EdmPrimitiveTypeKind.DateTimeOffset:
                    type = clientTemplate.DateTimeOffsetTypeName;
                    break;
                case EdmPrimitiveTypeKind.Duration:
                    type = clientTemplate.DurationTypeName;
                    break;
                case EdmPrimitiveTypeKind.Date:
                    type = clientTemplate.DateTypeName;
                    break;
                case EdmPrimitiveTypeKind.TimeOfDay:
                    type = clientTemplate.TimeOfDayTypeName;
                    break;
                default:
                    throw new Exception("Type " + kind + " is unrecognized");
            }

            return type;
        }
    }
}