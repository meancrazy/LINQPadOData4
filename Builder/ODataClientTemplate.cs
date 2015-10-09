using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;
using Microsoft.OData.Edm.Values;

namespace OData4.Builder
{
    /// <summary>
    /// The template class to generate the OData client code.
    /// </summary>
    public abstract class ODataClientTemplate : TemplateBase
    {
        protected readonly string SingleSuffix = "Single";
        protected const string T4Version = "2.4.0";

        /// <summary>
        /// The code generation context.
        /// </summary>
        protected readonly CodeGenerationContext Context;

        /// <summary>
        /// The Dictionary to store identifier mappings when there are duplicate names between properties and Entity/Complex types
        /// </summary>
        protected Dictionary<string, string> IdentifierMappings = new Dictionary<string, string>(StringComparer.Ordinal);

        /// <summary>
        /// Creates an instance of the ODataClientTemplate.
        /// </summary>
        /// <param name="context">The code generation context.</param>
        public ODataClientTemplate(CodeGenerationContext context)
        {
            Context = context;
        }

        #region Get Language specific keyword names.
        internal abstract string GlobalPrefix { get; }
        internal abstract string SystemTypeTypeName { get; }
        internal abstract string AbstractModifier { get; }
        internal abstract string DataServiceActionQueryTypeName { get; }
        internal abstract string DataServiceActionQuerySingleOfTStructureTemplate { get; }
        internal abstract string DataServiceActionQueryOfTStructureTemplate { get; }
        internal abstract string NotifyPropertyChangedModifier { get; }
        internal abstract string ClassInheritMarker { get; }
        internal abstract string ParameterSeparator { get; }
        internal abstract string KeyParameterSeparator { get; }
        internal abstract string KeyDictionaryItemSeparator { get; }
        internal abstract string SystemNullableStructureTemplate { get; }
        internal abstract string CollectionOfTStructureTemplate { get; }
        internal abstract string DataServiceCollectionStructureTemplate { get; }
        internal abstract string DataServiceQueryStructureTemplate { get; }
        internal abstract string DataServiceQuerySingleStructureTemplate { get; }
        internal abstract string ObservableCollectionStructureTemplate { get; }
        internal abstract string ObjectModelCollectionStructureTemplate { get; }
        internal abstract string DataServiceCollectionConstructorParameters { get; }
        internal abstract string NewModifier { get; }
        internal abstract string GeoTypeInitializePattern { get; }
        internal abstract string Int32TypeName { get; }
        internal abstract string StringTypeName { get; }
        internal abstract string BinaryTypeName { get; }
        internal abstract string DecimalTypeName { get; }
        internal abstract string Int16TypeName { get; }
        internal abstract string SingleTypeName { get; }
        internal abstract string BooleanTypeName { get; }
        internal abstract string DoubleTypeName { get; }
        internal abstract string GuidTypeName { get; }
        internal abstract string ByteTypeName { get; }
        internal abstract string Int64TypeName { get; }
        internal abstract string SByteTypeName { get; }
        internal abstract string DataServiceStreamLinkTypeName { get; }
        internal abstract string GeographyTypeName { get; }
        internal abstract string GeographyPointTypeName { get; }
        internal abstract string GeographyLineStringTypeName { get; }
        internal abstract string GeographyPolygonTypeName { get; }
        internal abstract string GeographyCollectionTypeName { get; }
        internal abstract string GeographyMultiPolygonTypeName { get; }
        internal abstract string GeographyMultiLineStringTypeName { get; }
        internal abstract string GeographyMultiPointTypeName { get; }
        internal abstract string GeometryTypeName { get; }
        internal abstract string GeometryPointTypeName { get; }
        internal abstract string GeometryLineStringTypeName { get; }
        internal abstract string GeometryPolygonTypeName { get; }
        internal abstract string GeometryCollectionTypeName { get; }
        internal abstract string GeometryMultiPolygonTypeName { get; }
        internal abstract string GeometryMultiLineStringTypeName { get; }
        internal abstract string GeometryMultiPointTypeName { get; }
        internal abstract string DateTypeName { get; }
        internal abstract string DateTimeOffsetTypeName { get; }
        internal abstract string DurationTypeName { get; }
        internal abstract string TimeOfDayTypeName { get; }
        internal abstract string XmlConvertClassName { get; }
        internal abstract string EnumTypeName { get; }
        internal abstract HashSet<string> LanguageKeywords { get; }
        internal abstract string FixPattern { get; }
        internal abstract string EnumUnderlyingTypeMarker { get; }
        internal abstract string ConstantExpressionConstructorWithType { get; }
        internal abstract string TypeofFormatter { get; }
        internal abstract string UriOperationParameterConstructor { get; }
        internal abstract string UriEntityOperationParameterConstructor { get; }
        internal abstract string BodyOperationParameterConstructor { get; }
        internal abstract string BaseEntityType { get; }
        internal abstract string OverloadsModifier { get; }
        internal abstract string ODataVersion { get; }
        internal abstract string ParameterDeclarationTemplate { get; }
        internal abstract string DictionaryItemConstructor { get; }
        #endregion Get Language specific keyword names.

        #region Language specific write methods.
        internal abstract void WriteFileHeader();
        internal abstract void WriteNamespaceStart(string fullNamespace);
        internal abstract void WriteClassStartForEntityContainer(string originalContainerName, string containerName, string fixedContainerName);
        internal abstract void WriteMethodStartForEntityContainerConstructor(string containerName, string fixedContainerName);
        internal abstract void WriteKeyAsSegmentUrlConvention();
        internal abstract void WriteInitializeResolveName();
        internal abstract void WriteInitializeResolveType();
        internal abstract void WriteClassEndForEntityContainerConstructor();
        internal abstract void WriteMethodStartForResolveTypeFromName();
        internal abstract void WriteResolveNamespace(string typeName, string fullNamespace, string languageDependentNamespace);
        internal abstract void WriteMethodEndForResolveTypeFromName();
        internal abstract void WriteMethodStartForResolveNameFromType(string containerName, string fullNamespace);
        internal abstract void WriteResolveType(string fullNamespace, string languageDependentNamespace);
        internal abstract void WriteMethodEndForResolveNameFromType(bool modelHasInheritance);
        internal abstract void WriteContextEntitySetProperty(string entitySetName, string entitySetFixedName, string originalEntitySetName, string entitySetElementTypeName, bool inContext = true);
        internal abstract void WriteContextSingletonProperty(string singletonName, string singletonFixedName, string originalSingletonName, string singletonElementTypeName, bool inContext = true);
        internal abstract void WriteContextAddToEntitySetMethod(string entitySetName, string originalEntitySetName, string typeName, string parameterName);
        internal abstract void WriteGeneratedEdmModel(string escapedEdmxString);
        internal abstract void WriteClassEndForEntityContainer();
        internal abstract void WriteSummaryCommentForStructuredType(string typeName);
        internal abstract void WriteKeyPropertiesCommentAndAttribute(IEnumerable<string> keyProperties, string keyString);
        internal abstract void WriteEntityTypeAttribute();
        internal abstract void WriteEntitySetAttribute(string entitySetName);
        internal abstract void WriteEntityHasStreamAttribute();
        internal abstract void WriteClassStartForStructuredType(string abstractModifier, string typeName, string originalTypeName, string baseTypeName);
        internal abstract void WriteSummaryCommentForStaticCreateMethod(string typeName);
        internal abstract void WriteParameterCommentForStaticCreateMethod(string parameterName, string propertyName);
        internal abstract void WriteDeclarationStartForStaticCreateMethod(string typeName, string fixedTypeName);
        internal abstract void WriteParameterForStaticCreateMethod(string parameterTypeName, string parameterName, string parameterSeparater);
        internal abstract void WriteDeclarationEndForStaticCreateMethod(string typeName, string instanceName);
        internal abstract void WriteParameterNullCheckForStaticCreateMethod(string parameterName);
        internal abstract void WritePropertyValueAssignmentForStaticCreateMethod(string instanceName, string propertyName, string parameterName);
        internal abstract void WriteMethodEndForStaticCreateMethod(string instanceName);
        internal abstract void WritePropertyForStructuredType(string propertyType, string originalPropertyName, string propertyName, string fixedPropertyName, string privatePropertyName, string propertyInitializationValue, bool writeOnPropertyChanged);
        internal abstract void WriteINotifyPropertyChangedImplementation();
        internal abstract void WriteClassEndForStructuredType();
        internal abstract void WriteNamespaceEnd();
        internal abstract void WriteEnumFlags();
        internal abstract void WriteSummaryCommentForEnumType(string enumName);
        internal abstract void WriteEnumDeclaration(string enumName, string originalEnumName, string underlyingType);
        internal abstract void WriteMemberForEnumType(string member, string originalMemberName, bool last);
        internal abstract void WriteEnumEnd();
        internal abstract void WritePropertyRootNamespace(string containerName, string fullNamespace);
        internal abstract void WriteFunctionImportReturnCollectionResult(string functionName, string originalFunctionName, string returnTypeName, string parameters, string parameterValues, bool isComposable, bool useEntityReference);
        internal abstract void WriteFunctionImportReturnSingleResult(string functionName, string originalFunctionName, string returnTypeName, string parameters, string parameterValues, bool isComposable, bool isReturnEntity, bool useEntityReference);
        internal abstract void WriteBoundFunctionInEntityTypeReturnCollectionResult(bool hideBaseMethod, string functionName, string originalFunctionName, string returnTypeName, string parameters, string fullNamespace, string parameterValues, bool isComposable, bool useEntityReference);
        internal abstract void WriteBoundFunctionInEntityTypeReturnSingleResult(bool hideBaseMethod, string functionName, string originalFunctionName, string returnTypeName, string parameters, string fullNamespace, string parameterValues, bool isComposable, bool isReturnEntity, bool useEntityReference);
        internal abstract void WriteActionImport(string actionName, string originalActionName, string returnTypeName, string parameters, string parameterValues);
        internal abstract void WriteBoundActionInEntityType(bool hideBaseMethod, string actionName, string originalActionName, string returnTypeName, string parameters, string fullNamespace, string parameterValues);
        internal abstract void WriteConstructorForSingleType(string singleTypeName, string baseTypeName);
        internal abstract void WriteExtensionMethodsStart();
        internal abstract void WriteExtensionMethodsEnd();
        internal abstract void WriteByKeyMethods(string entityTypeName, string returnTypeName, IEnumerable<string> keys, string keyParameters, string keyDictionaryItems);
        internal abstract void WriteCastToMethods(string baseTypeName, string derivedTypeName, string derivedTypeFullName, string returnTypeName);
        internal abstract void WriteBoundFunctionReturnSingleResultAsExtension(string functionName, string originalFunctionName, string boundTypeName, string returnTypeName, string parameters, string fullNamespace, string parameterValues, bool isComposable, bool isReturnEntity, bool useEntityReference);
        internal abstract void WriteBoundFunctionReturnCollectionResultAsExtension(string functionName, string originalFunctionName, string boundTypeName, string returnTypeName, string parameters, string fullNamespace, string parameterValues, bool isComposable, bool useEntityReference);
        internal abstract void WriteBoundActionAsExtension(string actionName, string originalActionName, string boundSourceType, string returnTypeName, string parameters, string fullNamespace, string parameterValues);
        #endregion Language specific write methods.

        internal HashSet<EdmPrimitiveTypeKind> ClrReferenceTypes
        {
            get
            {
                if (_clrReferenceTypes == null)
                {
                    _clrReferenceTypes = new HashSet<EdmPrimitiveTypeKind>
                    {
                        EdmPrimitiveTypeKind.String, EdmPrimitiveTypeKind.Binary, EdmPrimitiveTypeKind.Geography, EdmPrimitiveTypeKind.Stream,
                        EdmPrimitiveTypeKind.GeographyPoint, EdmPrimitiveTypeKind.GeographyLineString, EdmPrimitiveTypeKind.GeographyPolygon,
                        EdmPrimitiveTypeKind.GeographyCollection, EdmPrimitiveTypeKind.GeographyMultiPolygon, EdmPrimitiveTypeKind.GeographyMultiLineString,
                        EdmPrimitiveTypeKind.GeographyMultiPoint, EdmPrimitiveTypeKind.Geometry, EdmPrimitiveTypeKind.GeometryPoint,
                        EdmPrimitiveTypeKind.GeometryLineString, EdmPrimitiveTypeKind.GeometryPolygon, EdmPrimitiveTypeKind.GeometryCollection,
                        EdmPrimitiveTypeKind.GeometryMultiPolygon, EdmPrimitiveTypeKind.GeometryMultiLineString, EdmPrimitiveTypeKind.GeometryMultiPoint
                    };
                }
                return _clrReferenceTypes;
            }
        }
        private HashSet<EdmPrimitiveTypeKind> _clrReferenceTypes;

        /// <summary>
        /// Generates code for the OData client.
        /// </summary>
        /// <returns>The generated code for the OData client.</returns>
        public override string TransformText()
        {
            WriteFileHeader();
            WriteNamespaces();
            return GenerationEnvironment.ToString();
        }

        internal void WriteNamespaces()
        {
            foreach (var fullNamespace in Context.NamespacesInModel)
            {
                WriteNamespace(fullNamespace);
            }
        }

        internal void WriteNamespace(string fullNamespace)
        {
            WriteNamespaceStart(Context.GetPrefixedNamespace(fullNamespace, this, true, false));

            var schemaElements = Context.GetSchemaElements(fullNamespace).ToArray();
            if (schemaElements.OfType<IEdmEntityContainer>().Any())
            {
                var container = schemaElements.OfType<IEdmEntityContainer>().Single();
                WriteEntityContainer(container, fullNamespace);
            }

            var boundOperationsMap = new Dictionary<IEdmStructuredType, List<IEdmOperation>>();
            foreach (var operation in schemaElements.OfType<IEdmOperation>())
            {
                if (operation.IsBound)
                {
                    var edmType = operation.Parameters.First().Type.Definition;
                    var edmStructuredType = edmType as IEdmStructuredType;
                    if (edmStructuredType != null)
                    {
                        List<IEdmOperation> operationList;
                        if (!boundOperationsMap.TryGetValue(edmStructuredType, out operationList))
                        {
                            operationList = new List<IEdmOperation>();
                        }

                        operationList.Add(operation);
                        boundOperationsMap[edmStructuredType] = operationList;
                    }
                }
            }

            var structuredBaseTypeMap = new Dictionary<IEdmStructuredType, List<IEdmStructuredType>>();
            foreach (var type in schemaElements.OfType<IEdmSchemaType>())
            {
                var enumType = type as IEdmEnumType;
                if (enumType != null)
                {
                    WriteEnumType(enumType);
                }
                else
                {
                    var complexType = type as IEdmComplexType;
                    if (complexType != null)
                    {
                        WriteComplexType(complexType, boundOperationsMap);
                    }
                    else
                    {
                        var entityType = type as IEdmEntityType;
                        WriteEntityType(entityType, boundOperationsMap);
                    }

                    var structuredType = type as IEdmStructuredType;
                    if (structuredType.BaseType != null)
                    {
                        List<IEdmStructuredType> derivedTypes;
                        if (!structuredBaseTypeMap.TryGetValue(structuredType.BaseType, out derivedTypes))
                        {
                            structuredBaseTypeMap[structuredType.BaseType] = new List<IEdmStructuredType>();
                        }

                        structuredBaseTypeMap[structuredType.BaseType].Add(structuredType);
                    }
                }
            }

            if (schemaElements.OfType<IEdmEntityType>().Any() ||
                schemaElements.OfType<IEdmOperation>().Any(o => o.IsBound))
            {
                WriteExtensionMethodsStart();
                foreach (var type in schemaElements.OfType<IEdmEntityType>())
                {
                    var entityTypeName = type.Name;
                    entityTypeName = Context.EnableNamingAlias ? Customization.CustomizeNaming(entityTypeName) : entityTypeName;
                    var entityTypeFullName = Context.GetPrefixedFullName(type, GetFixedName(entityTypeName), this);
                    var returnTypeName = Context.GetPrefixedFullName(type, GetFixedName(entityTypeName + SingleSuffix), this);

                    var keyProperties = type.Key();
                    if (keyProperties != null && keyProperties.Any())
                    {
                        var keyParameters = new List<string>();
                        var keyDictionaryItems = new List<string>();
                        var keyNames = new List<string>();
                        foreach (IEdmProperty key in keyProperties)
                        {
                            var typeName = Utils.GetClrTypeName(key.Type, Context.UseDataServiceCollection, this, Context);
                            var keyName = Utils.CamelCase(key.Name);
                            keyNames.Add(keyName);
                            keyParameters.Add(string.Format(ParameterDeclarationTemplate, typeName, GetFixedName(keyName)));
                            keyDictionaryItems.Add(string.Format(DictionaryItemConstructor, "\"" + key.Name + "\"", GetFixedName(keyName)));
                        }

                        var keyParametersString = string.Join(KeyParameterSeparator, keyParameters);
                        var keyDictionaryItemsString = string.Join(KeyDictionaryItemSeparator, keyDictionaryItems);
                        WriteByKeyMethods(entityTypeFullName, returnTypeName, keyNames, keyParametersString, keyDictionaryItemsString);
                    }

                    var current = (IEdmEntityType)type.BaseType;
                    while (current != null)
                    {
                        var baseTypeName = current.Name;
                        baseTypeName = Context.EnableNamingAlias ? Customization.CustomizeNaming(baseTypeName) : baseTypeName;
                        baseTypeName = Context.GetPrefixedFullName(current, GetFixedName(baseTypeName), this);
                        WriteCastToMethods(baseTypeName, entityTypeName, entityTypeFullName, returnTypeName);
                        current = (IEdmEntityType)current.BaseType;
                    }
                }

                var boundOperations = new HashSet<string>(StringComparer.Ordinal);
                foreach (var function in schemaElements.OfType<IEdmFunction>())
                {
                    if (function.IsBound)
                    {
                        var edmTypeReference = function.Parameters.First().Type;
                        var functionName = Context.EnableNamingAlias ? Customization.CustomizeNaming(function.Name) : function.Name;
                        string parameterString, parameterExpressionString, parameterTypes, parameterValues;
                        bool useEntityReference;
                        GetParameterStrings(function.IsBound, false, function.Parameters.ToArray(), out parameterString, out parameterTypes, out parameterExpressionString, out parameterValues, out useEntityReference);
                        var sourceTypeName = GetSourceOrReturnTypeName(edmTypeReference);
                        sourceTypeName = string.Format(edmTypeReference.IsCollection() ? DataServiceQueryStructureTemplate : DataServiceQuerySingleStructureTemplate, sourceTypeName);
                        var returnTypeName = GetSourceOrReturnTypeName(function.ReturnType);
                        var fixedFunctionName = GetFixedName(functionName);
                        var func = string.Format("{0}({1},{2})", fixedFunctionName, sourceTypeName, parameterTypes);

                        if (!boundOperations.Contains(func))
                        {
                            boundOperations.Add(func);

                            if (function.ReturnType.IsCollection())
                            {
                                WriteBoundFunctionReturnCollectionResultAsExtension(fixedFunctionName, function.Name, sourceTypeName, returnTypeName, parameterString, function.Namespace, parameterValues, function.IsComposable, useEntityReference);
                            }
                            else
                            {
                                WriteBoundFunctionReturnSingleResultAsExtension(fixedFunctionName, function.Name, sourceTypeName, returnTypeName, parameterString, function.Namespace, parameterValues, function.IsComposable, function.ReturnType.IsEntity(), useEntityReference);
                            }
                        }

                        IEdmStructuredType structuredType;
                        if (edmTypeReference.IsCollection())
                        {
                            var collectionType = edmTypeReference.Definition as IEdmCollectionType;
                            structuredType = (IEdmStructuredType)collectionType.ElementType.Definition;
                        }
                        else
                        {
                            structuredType = (IEdmStructuredType)edmTypeReference.Definition;
                        }

                        List<IEdmStructuredType> derivedTypes;
                        if (structuredBaseTypeMap.TryGetValue(structuredType, out derivedTypes))
                        {
                            foreach (var type in derivedTypes)
                            {
                                IEdmTypeReference derivedTypeReference = new EdmEntityTypeReference((IEdmEntityType)type, true);
                                var currentParameters = function.Parameters.Select(p => p.Type).ToList();
                                currentParameters[0] = derivedTypeReference;

                                sourceTypeName = string.Format(edmTypeReference.IsCollection() ? DataServiceQueryStructureTemplate : DataServiceQuerySingleStructureTemplate, GetSourceOrReturnTypeName(derivedTypeReference));
                                var currentFunc = string.Format("{0}({1},{2})", fixedFunctionName, sourceTypeName, parameterTypes);
                                if (!boundOperations.Contains(currentFunc))
                                {
                                    boundOperations.Add(currentFunc);

                                    if (function.ReturnType.IsCollection())
                                    {
                                        WriteBoundFunctionReturnCollectionResultAsExtension(fixedFunctionName, function.Name, sourceTypeName, returnTypeName, parameterString, function.Namespace, parameterValues, function.IsComposable, useEntityReference);
                                    }
                                    else
                                    {
                                        WriteBoundFunctionReturnSingleResultAsExtension(fixedFunctionName, function.Name, sourceTypeName, returnTypeName, parameterString, function.Namespace, parameterValues, function.IsComposable, function.ReturnType.IsEntity(), useEntityReference);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var action in schemaElements.OfType<IEdmAction>())
                {
                    if (action.IsBound)
                    {
                        var edmTypeReference = action.Parameters.First().Type;
                        var actionName = Context.EnableNamingAlias ? Customization.CustomizeNaming(action.Name) : action.Name;
                        string parameterString, parameterExpressionString, parameterTypes, parameterValues;
                        bool useEntityReference;
                        GetParameterStrings(action.IsBound, true, action.Parameters.ToArray(), out parameterString, out parameterTypes, out parameterExpressionString, out parameterValues, out useEntityReference);
                        var sourceTypeName = GetSourceOrReturnTypeName(edmTypeReference);
                        sourceTypeName = string.Format(edmTypeReference.IsCollection() ? DataServiceQueryStructureTemplate : DataServiceQuerySingleStructureTemplate, sourceTypeName);
                        string returnTypeName;
                        if (action.ReturnType != null)
                        {
                            returnTypeName = GetSourceOrReturnTypeName(action.ReturnType);
                            if (action.ReturnType.IsCollection())
                            {
                                returnTypeName = string.Format(DataServiceActionQueryOfTStructureTemplate, returnTypeName);
                            }
                            else
                            {
                                returnTypeName = string.Format(DataServiceActionQuerySingleOfTStructureTemplate, returnTypeName);
                            }
                        }
                        else
                        {
                            returnTypeName = DataServiceActionQueryTypeName;
                        }

                        var fixedActionName = GetFixedName(actionName);
                        var ac = string.Format("{0}({1},{2})", fixedActionName, sourceTypeName, parameterTypes);
                        if (!boundOperations.Contains(ac))
                        {
                            boundOperations.Add(ac);
                            WriteBoundActionAsExtension(fixedActionName, action.Name, sourceTypeName, returnTypeName, parameterString, action.Namespace, parameterValues);
                        }

                        IEdmStructuredType structuredType;
                        if (edmTypeReference.IsCollection())
                        {
                            var collectionType = edmTypeReference.Definition as IEdmCollectionType;
                            structuredType = (IEdmStructuredType)collectionType.ElementType.Definition;
                        }
                        else
                        {
                            structuredType = (IEdmStructuredType)edmTypeReference.Definition;
                        }

                        List<IEdmStructuredType> derivedTypes;
                        if (structuredBaseTypeMap.TryGetValue(structuredType, out derivedTypes))
                        {
                            foreach (var type in derivedTypes)
                            {
                                IEdmTypeReference derivedTypeReference = new EdmEntityTypeReference((IEdmEntityType)type, true);
                                var currentParameters = action.Parameters.Select(p => p.Type).ToList();
                                currentParameters[0] = derivedTypeReference;

                                sourceTypeName = string.Format(edmTypeReference.IsCollection() ? DataServiceQueryStructureTemplate : DataServiceQuerySingleStructureTemplate, GetSourceOrReturnTypeName(derivedTypeReference));
                                var currentAc = string.Format("{0}({1},{2})", fixedActionName, sourceTypeName, parameterTypes);
                                if (!boundOperations.Contains(currentAc))
                                {
                                    boundOperations.Add(currentAc);
                                    WriteBoundActionAsExtension(fixedActionName, action.Name, sourceTypeName, returnTypeName, parameterString, action.Namespace, parameterValues);
                                }
                            }
                        }
                    }
                }

                WriteExtensionMethodsEnd();
            }

            WriteNamespaceEnd();
        }

        internal bool HasBoundOperations(IEnumerable<IEdmOperation> operations)
        {
            foreach (var opeartion in operations)
            {
                if (opeartion.IsBound)
                {
                    return true;
                }
            }

            return false;
        }

        internal void WriteEntityContainer(IEdmEntityContainer container, string fullNamespace)
        {
            var camelCaseContainerName = container.Name;
            if (Context.EnableNamingAlias)
            {
                camelCaseContainerName = Customization.CustomizeNaming(camelCaseContainerName);
            }

            WriteClassStartForEntityContainer(container.Name, camelCaseContainerName, GetFixedName(camelCaseContainerName));
            WriteEntityContainerConstructor(container);

            if (Context.NeedResolveNameFromType)
            {
                WritePropertyRootNamespace(GetFixedName(camelCaseContainerName), Context.GetPrefixedNamespace(fullNamespace, this, false, false));
            }

            WriteResolveTypeFromName();
            WriteResolveNameFromType(camelCaseContainerName, fullNamespace);

            foreach (var entitySet in container.EntitySets())
            {
                var entitySetElementType = entitySet.EntityType();
                var entitySetElementTypeName = GetElementTypeName(entitySetElementType, container);

                var camelCaseEntitySetName = entitySet.Name;
                if (Context.EnableNamingAlias)
                {
                    camelCaseEntitySetName = Customization.CustomizeNaming(camelCaseEntitySetName);
                }

                WriteContextEntitySetProperty(camelCaseEntitySetName, GetFixedName(camelCaseEntitySetName), entitySet.Name, GetFixedName(entitySetElementTypeName));
                List<IEdmNavigationSource> edmNavigationSourceList = null;
                if (!Context.ElementTypeToNavigationSourceMap.TryGetValue(entitySet.EntityType(), out edmNavigationSourceList))
                {
                    edmNavigationSourceList = new List<IEdmNavigationSource>();
                    Context.ElementTypeToNavigationSourceMap.Add(entitySet.EntityType(), edmNavigationSourceList);
                }

                edmNavigationSourceList.Add(entitySet);
            }

            foreach (var entitySet in container.EntitySets())
            {
                var entitySetElementType = entitySet.EntityType();

                var entitySetElementTypeName = GetElementTypeName(entitySetElementType, container);

                var uniqueIdentifierService = new UniqueIdentifierService(/*IsLanguageCaseSensitive*/true);
                var parameterName = GetFixedName(uniqueIdentifierService.GetUniqueParameterName(entitySetElementType.Name));

                var camelCaseEntitySetName = entitySet.Name;
                if (Context.EnableNamingAlias)
                {
                    camelCaseEntitySetName = Customization.CustomizeNaming(camelCaseEntitySetName);
                }

                WriteContextAddToEntitySetMethod(camelCaseEntitySetName, entitySet.Name, GetFixedName(entitySetElementTypeName), parameterName);
            }

            foreach (var singleton in container.Singletons())
            {
                var singletonElementType = singleton.EntityType();
                var singletonElementTypeName = GetElementTypeName(singletonElementType, container);
                var camelCaseSingletonName = singleton.Name;
                if (Context.EnableNamingAlias)
                {
                    camelCaseSingletonName = Customization.CustomizeNaming(camelCaseSingletonName);
                }

                WriteContextSingletonProperty(camelCaseSingletonName, GetFixedName(camelCaseSingletonName), singleton.Name, singletonElementTypeName + "Single");

                List<IEdmNavigationSource> edmNavigationSourceList = null;
                if (Context.ElementTypeToNavigationSourceMap.TryGetValue(singleton.EntityType(), out edmNavigationSourceList))
                {
                    edmNavigationSourceList.Add(singleton);
                }
            }

            WriteGeneratedEdmModel(Utils.SerializeToString(Context.Edmx).Replace("\"", "\"\""));

            var hasOperationImport = container.OperationImports().OfType<IEdmOperationImport>().Any();
            foreach (var functionImport in container.OperationImports().OfType<IEdmFunctionImport>())
            {
                string parameterString, parameterTypes, parameterExpressionString, parameterValues;
                bool useEntityReference;
                GetParameterStrings(false, false, functionImport.Function.Parameters.ToArray(), out parameterString, out parameterTypes, out parameterExpressionString, out parameterValues, out useEntityReference);
                var returnTypeName = GetSourceOrReturnTypeName(functionImport.Function.ReturnType);
                var fixedContainerName = GetFixedName(functionImport.Container.Name);
                var isCollectionResult = functionImport.Function.ReturnType.IsCollection();
                var functionImportName = functionImport.Name;
                if (Context.EnableNamingAlias)
                {
                    functionImportName = Customization.CustomizeNaming(functionImportName);
                    fixedContainerName = Customization.CustomizeNaming(fixedContainerName);
                }

                if (functionImport.Function.ReturnType.IsCollection())
                {
                    WriteFunctionImportReturnCollectionResult(GetFixedName(functionImportName), functionImport.Name, returnTypeName, parameterString, parameterValues, functionImport.Function.IsComposable, useEntityReference);
                }
                else
                {
                    WriteFunctionImportReturnSingleResult(GetFixedName(functionImportName), functionImport.Name, returnTypeName, parameterString, parameterValues, functionImport.Function.IsComposable, functionImport.Function.ReturnType.IsEntity(), useEntityReference);
                }
            }

            foreach (var actionImport in container.OperationImports().OfType<IEdmActionImport>())
            {
                string parameterString, parameterTypes, parameterExpressionString, parameterValues;
                bool useEntityReference;
                GetParameterStrings(false, true, actionImport.Action.Parameters.ToArray(), out parameterString, out parameterTypes, out parameterExpressionString, out parameterValues, out useEntityReference);
                string returnTypeName = null;
                var fixedContainerName = GetFixedName(actionImport.Container.Name);

                if (actionImport.Action.ReturnType != null)
                {
                    returnTypeName = GetSourceOrReturnTypeName(actionImport.Action.ReturnType);
                    if (actionImport.Action.ReturnType.IsCollection())
                    {
                        returnTypeName = string.Format(DataServiceActionQueryOfTStructureTemplate, returnTypeName);
                    }
                    else
                    {
                        returnTypeName = string.Format(DataServiceActionQuerySingleOfTStructureTemplate, returnTypeName);
                    }
                }
                else
                {
                    returnTypeName = DataServiceActionQueryTypeName;
                }

                var actionImportName = actionImport.Name;
                if (Context.EnableNamingAlias)
                {
                    actionImportName = Customization.CustomizeNaming(actionImportName);
                    fixedContainerName = Customization.CustomizeNaming(fixedContainerName);
                }

                WriteActionImport(GetFixedName(actionImportName), actionImport.Name, returnTypeName, parameterString, parameterValues);
            }

            WriteClassEndForEntityContainer();
        }

        internal void WriteEntityContainerConstructor(IEdmEntityContainer container)
        {
            var camelCaseContainerName = container.Name;
            if (Context.EnableNamingAlias)
            {
                camelCaseContainerName = Customization.CustomizeNaming(camelCaseContainerName);
            }

            WriteMethodStartForEntityContainerConstructor(camelCaseContainerName, GetFixedName(camelCaseContainerName));

            if (Context.UseKeyAsSegmentUrlConvention(container))
            {
                WriteKeyAsSegmentUrlConvention();
            }

            if (Context.NeedResolveNameFromType)
            {
                WriteInitializeResolveName();
            }

            if (Context.NeedResolveTypeFromName)
            {
                WriteInitializeResolveType();
            }

            WriteClassEndForEntityContainerConstructor();
        }

        internal void WriteResolveTypeFromName()
        {
            if (!Context.NeedResolveTypeFromName)
            {
                return;
            }

            WriteMethodStartForResolveTypeFromName();

            // NOTE: since multiple namespaces can have the same prefix and match the namespace
            // prefix condition, it's important that the prefix check is done is prefix-length
            // order, starting with the longest prefix.
            IEnumerable<KeyValuePair<string, string>> namespaceToPrefixedNamespacePairs = Context.NamespaceMap.OrderByDescending(p => p.Key.Length).ThenBy(p => p.Key);

            var typeName = SystemTypeTypeName + " ";
            foreach (var namespaceToPrefixedNamespacePair in namespaceToPrefixedNamespacePairs)
            {
                WriteResolveNamespace(typeName, namespaceToPrefixedNamespacePair.Key, namespaceToPrefixedNamespacePair.Value);
                typeName = string.Empty;
            }

            WriteMethodEndForResolveTypeFromName();
        }

        internal void WriteResolveNameFromType(string containerName, string fullNamespace)
        {
            if (!Context.NeedResolveNameFromType)
            {
                return;
            }

            WriteMethodStartForResolveNameFromType(GetFixedName(containerName), fullNamespace);

            // NOTE: in this case order also matters, but the length of the CLR
            // namespace is what needs to be considered.
            IEnumerable<KeyValuePair<string, string>> namespaceToPrefixedNamespacePairs = Context.NamespaceMap.OrderByDescending(p => p.Value.Length).ThenBy(p => p.Key);

            foreach (var namespaceToPrefixedNamespacePair in namespaceToPrefixedNamespacePairs)
            {
                WriteResolveType(namespaceToPrefixedNamespacePair.Key, namespaceToPrefixedNamespacePair.Value);
            }

            WriteMethodEndForResolveNameFromType(Context.ModelHasInheritance);
        }

        internal void WritePropertiesForSingleType(IEnumerable<IEdmProperty> properties)
        {
            foreach (var property in properties.Where(i => i.PropertyKind == EdmPropertyKind.Navigation))
            {
                string propertyType;
                var propertyName = Context.EnableNamingAlias ? Customization.CustomizeNaming(property.Name) : property.Name;
                if (property.Type is EdmCollectionTypeReference)
                {
                    propertyType = GetSourceOrReturnTypeName(property.Type);
                    WriteContextEntitySetProperty(propertyName, GetFixedName(propertyName), property.Name, propertyType, false);
                }
                else
                {
                    propertyType = Utils.GetClrTypeName(property.Type, true, this, Context, true);
                    WriteContextSingletonProperty(propertyName, GetFixedName(propertyName), property.Name, propertyType + "Single", false);
                }
            }
        }

        internal void WriteEntityType(IEdmEntityType entityType, Dictionary<IEdmStructuredType, List<IEdmOperation>> boundOperationsMap)
        {
            var entityTypeName = entityType.Name;
            entityTypeName = Context.EnableNamingAlias ? Customization.CustomizeNaming(entityTypeName) : entityTypeName;
            WriteSummaryCommentForStructuredType(entityTypeName + SingleSuffix);
            WriteStructurdTypeDeclaration(entityType,
                ClassInheritMarker + string.Format(DataServiceQuerySingleStructureTemplate, GetFixedName(entityTypeName)),
                SingleSuffix);
            var singleTypeName = (Context.EnableNamingAlias ?
                Customization.CustomizeNaming(entityType.Name) : entityType.Name) + SingleSuffix;
            WriteConstructorForSingleType(GetFixedName(singleTypeName), string.Format(DataServiceQuerySingleStructureTemplate, GetFixedName(entityTypeName)));
            var current = entityType;
            while (current != null)
            {
                WritePropertiesForSingleType(current.DeclaredProperties);
                current = (IEdmEntityType)current.BaseType;
            }

            WriteClassEndForStructuredType();

            WriteSummaryCommentForStructuredType(Context.EnableNamingAlias ? Customization.CustomizeNaming(entityType.Name) : entityType.Name);

            if (entityType.Key().Any())
            {
                var keyProperties = entityType.Key().Select(k => k.Name);
                WriteKeyPropertiesCommentAndAttribute(
                    Context.EnableNamingAlias ? keyProperties.Select(k => Customization.CustomizeNaming(k)) : keyProperties,
                    string.Join("\", \"", keyProperties));
            }
            else
            {
                WriteEntityTypeAttribute();
            }

            if (Context.UseDataServiceCollection)
            {
                List<IEdmNavigationSource> navigationSourceList;
                if (Context.ElementTypeToNavigationSourceMap.TryGetValue(entityType, out navigationSourceList))
                {
                    if (navigationSourceList.Count == 1)
                    {
                        WriteEntitySetAttribute(navigationSourceList[0].Name);
                    }
                }
            }

            if (entityType.HasStream)
            {
                WriteEntityHasStreamAttribute();
            }

            WriteStructurdTypeDeclaration(entityType, BaseEntityType);
            SetPropertyIdentifierMappingsIfNameConflicts(entityType.Name, entityType);
            WriteTypeStaticCreateMethod(entityType.Name, entityType);
            WritePropertiesForStructuredType(entityType.DeclaredProperties);

            if (entityType.BaseType == null && Context.UseDataServiceCollection)
            {
                WriteINotifyPropertyChangedImplementation();
            }

            WriteBoundOperations(entityType, boundOperationsMap);

            WriteClassEndForStructuredType();
        }

        internal void WriteComplexType(IEdmComplexType complexType, Dictionary<IEdmStructuredType, List<IEdmOperation>> boundOperationsMap)
        {
            WriteSummaryCommentForStructuredType(Context.EnableNamingAlias ? Customization.CustomizeNaming(complexType.Name) : complexType.Name);
            WriteStructurdTypeDeclaration(complexType, string.Empty);
            SetPropertyIdentifierMappingsIfNameConflicts(complexType.Name, complexType);
            WriteTypeStaticCreateMethod(complexType.Name, complexType);
            WritePropertiesForStructuredType(complexType.DeclaredProperties);

            if (complexType.BaseType == null && Context.UseDataServiceCollection)
            {
                WriteINotifyPropertyChangedImplementation();
            }

            WriteClassEndForStructuredType();
        }

        internal void WriteBoundOperations(IEdmStructuredType structuredType, Dictionary<IEdmStructuredType, List<IEdmOperation>> boundOperationsMap)
        {
            List<IEdmOperation> operations;
            if (boundOperationsMap.TryGetValue(structuredType, out operations))
            {
                foreach (var function in operations.OfType<IEdmFunction>())
                {
                    string parameterString, parameterExpressionString, parameterTypes, parameterValues;
                    bool useEntityReference;
                    var hideBaseMethod = CheckMethodsInBaseClass(structuredType.BaseType, function, boundOperationsMap);
                    GetParameterStrings(function.IsBound, false, function.Parameters.ToArray(), out parameterString, out parameterTypes, out parameterExpressionString, out parameterValues, out useEntityReference);
                    var returnTypeName = GetSourceOrReturnTypeName(function.ReturnType);
                    var functionName = function.Name;
                    if (Context.EnableNamingAlias)
                    {
                        functionName = Customization.CustomizeNaming(functionName);
                    }

                    if (function.ReturnType.IsCollection())
                    {
                        WriteBoundFunctionInEntityTypeReturnCollectionResult(hideBaseMethod, GetFixedName(functionName), function.Name, returnTypeName, parameterString, function.Namespace, parameterValues, function.IsComposable, useEntityReference);
                    }
                    else
                    {
                        WriteBoundFunctionInEntityTypeReturnSingleResult(hideBaseMethod, GetFixedName(functionName), function.Name, returnTypeName, parameterString, function.Namespace, parameterValues, function.IsComposable, function.ReturnType.IsEntity(), useEntityReference);
                    }
                }

                foreach (var action in operations.OfType<IEdmAction>())
                {
                    string parameterString, parameterExpressionString, parameterTypes, parameterValues;
                    bool useEntityReference;
                    var hideBaseMethod = CheckMethodsInBaseClass(structuredType.BaseType, action, boundOperationsMap);
                    GetParameterStrings(action.IsBound, true, action.Parameters.ToArray(), out parameterString, out parameterTypes, out parameterExpressionString, out parameterValues, out useEntityReference);
                    string returnTypeName;
                    if (action.ReturnType != null)
                    {
                        returnTypeName = GetSourceOrReturnTypeName(action.ReturnType);
                        if (action.ReturnType.IsCollection())
                        {
                            returnTypeName = string.Format(DataServiceActionQueryOfTStructureTemplate, returnTypeName);
                        }
                        else
                        {
                            returnTypeName = string.Format(DataServiceActionQuerySingleOfTStructureTemplate, returnTypeName);
                        }
                    }
                    else
                    {
                        returnTypeName = DataServiceActionQueryTypeName;
                    }

                    var actionName = action.Name;
                    if (Context.EnableNamingAlias)
                    {
                        actionName = Customization.CustomizeNaming(actionName);
                    }

                    WriteBoundActionInEntityType(hideBaseMethod, GetFixedName(actionName), action.Name, returnTypeName, parameterString, action.Namespace, parameterValues);
                }
            }
        }

        internal bool CheckMethodsInBaseClass(IEdmStructuredType structuredType, IEdmOperation operation, Dictionary<IEdmStructuredType, List<IEdmOperation>> boundOperationsMap)
        {
            if (structuredType != null)
            {
                List<IEdmOperation> operations;
                if (boundOperationsMap.TryGetValue(structuredType, out operations))
                {
                    foreach (var op in operations)
                    {
                        var targetParameter = operation.Parameters.ToList();
                        var checkParameter = op.Parameters.ToList();
                        if (operation.Name == op.Name && targetParameter.Count == checkParameter.Count)
                        {
                            var areSame = true;
                            for (var i = 1; i < targetParameter.Count; ++i)
                            {
                                var targetParameterType = targetParameter[i].Type;
                                var checkParameterType = checkParameter[i].Type;
                                if (!targetParameterType.Definition.Equals(checkParameterType.Definition)
                                    || targetParameterType.IsNullable != checkParameterType.IsNullable)
                                {
                                    areSame = false;
                                    break;
                                }
                            }

                            if (areSame)
                            {
                                return true;
                            }
                        }
                    }
                }

                return CheckMethodsInBaseClass(structuredType.BaseType, operation, boundOperationsMap);
            }

            return false;
        }

        internal void WriteEnumType(IEdmEnumType enumType)
        {
            WriteSummaryCommentForEnumType(Context.EnableNamingAlias ? Customization.CustomizeNaming(enumType.Name) : enumType.Name);
            if (enumType.IsFlags)
            {
                WriteEnumFlags();
            }

            var underlyingType = string.Empty;
            if (enumType.UnderlyingType != null && enumType.UnderlyingType.PrimitiveKind != EdmPrimitiveTypeKind.Int32)
            {
                underlyingType = Utils.GetClrTypeName(enumType.UnderlyingType, this);
                underlyingType = EnumUnderlyingTypeMarker + underlyingType;
            }

            WriteEnumDeclaration(Context.EnableNamingAlias ? GetFixedName(Customization.CustomizeNaming(enumType.Name)) : GetFixedName(enumType.Name), enumType.Name, underlyingType);
            WriteMembersForEnumType(enumType.Members);
            WriteEnumEnd();
        }

        internal void WriteStructurdTypeDeclaration(IEdmStructuredType structuredType, string baseEntityType, string typeNameSuffix = null)
        {
            var abstractModifier = structuredType.IsAbstract && typeNameSuffix == null ? AbstractModifier : string.Empty;
            var baseTypeName = baseEntityType;

            if (typeNameSuffix == null)
            {
                if (structuredType.BaseType == null)
                {
                    if (Context.UseDataServiceCollection)
                    {
                        baseTypeName += string.IsNullOrEmpty(baseTypeName) ? ClassInheritMarker : ", ";
                        baseTypeName += NotifyPropertyChangedModifier;
                    }
                }
                else
                {
                    var baseType = (IEdmSchemaElement)structuredType.BaseType;
                    var baseTypeFixedName = Context.EnableNamingAlias ? GetFixedName(Customization.CustomizeNaming(baseType.Name)) : GetFixedName(baseType.Name);
                    baseTypeName = ((IEdmSchemaElement)structuredType).Namespace == baseType.Namespace ? baseTypeFixedName : Context.GetPrefixedFullName(baseType, baseTypeFixedName, this);
                    baseTypeName = ClassInheritMarker + baseTypeName;
                }
            }

            var structuredTypeName = Context.EnableNamingAlias ?
                Customization.CustomizeNaming(((IEdmSchemaElement)structuredType).Name) : ((IEdmSchemaElement)structuredType).Name;
            WriteClassStartForStructuredType(abstractModifier, GetFixedName(structuredTypeName + typeNameSuffix), ((IEdmSchemaElement)structuredType).Name + typeNameSuffix, baseTypeName);
        }

        internal string GetSourceOrReturnTypeName(IEdmTypeReference typeReference)
        {
            var edmCollectionType = typeReference.Definition as IEdmCollectionType;
            var addNullableTemplate = true;
            if (edmCollectionType != null)
            {
                typeReference = edmCollectionType.ElementType;
                addNullableTemplate = false;
            }

            return Utils.GetClrTypeName(typeReference, Context.UseDataServiceCollection, this, Context, addNullableTemplate);
        }

        internal void GetParameterStrings(bool isBound, bool isAction, IEdmOperationParameter[] parameters, out string parameterString, out string parameterTypes, out string parameterExpressionString, out string parameterValues, out bool useEntityReference)
        {
            parameterString = string.Empty;
            parameterExpressionString = string.Empty;
            parameterTypes = string.Empty;
            parameterValues = string.Empty;
            useEntityReference = false;

            var n = parameters.Count();
            for (var i = isBound ? 1 : 0; i < n; ++i)
            {
                var param = parameters[i];
                if (i == (isBound ? 1 : 0))
                {
                    parameterExpressionString += "\r\n                        ";
                }

                var typeName = Utils.GetClrTypeName(param.Type, Context.UseDataServiceCollection, this, Context, true, true, true);
                parameterString += typeName;
                parameterString += (" " + GetFixedName(param.Name));

                parameterString += i == n - 1 ? string.Empty : ", ";
                parameterTypes += string.Format(TypeofFormatter, typeName) + ", ";
                parameterExpressionString += GetParameterExpressionString(param, typeName) + ", ";

                if (i != (isBound ? 1 : 0))
                {
                    parameterValues += ",\r\n                    ";
                }

                if (isAction)
                {
                    parameterValues += string.Format(BodyOperationParameterConstructor, param.Name, GetFixedName(param.Name));
                }
                else if (param.Type.IsEntity() || (param.Type.IsCollection() && param.Type.AsCollection().ElementType().IsEntity()))
                {
                    useEntityReference = true;
                    parameterValues += string.Format(UriEntityOperationParameterConstructor, param.Name, GetFixedName(param.Name), "useEntityReference");
                }
                else
                {
                    parameterValues += string.Format(UriOperationParameterConstructor, param.Name, GetFixedName(param.Name));
                }
            }
        }

        internal string GetParameterExpressionString(IEdmOperationParameter param, string typeName)
        {
            string clrTypeName;
            var edmType = param.Type.Definition;
            var edmPrimitiveType = edmType as IEdmPrimitiveType;
            if (edmPrimitiveType != null)
            {
                clrTypeName = Utils.GetClrTypeName(edmPrimitiveType, this);
                if (param.Type.IsNullable && !ClrReferenceTypes.Contains(edmPrimitiveType.PrimitiveKind))
                {
                    clrTypeName += "?";
                }

                return string.Format(ConstantExpressionConstructorWithType, GetFixedName(param.Name), clrTypeName);
            }

            return string.Format(ConstantExpressionConstructorWithType, GetFixedName(param.Name), typeName);
        }

        // This is to solve duplicate names between property and type
        internal void SetPropertyIdentifierMappingsIfNameConflicts(string typeName, IEdmStructuredType structuredType)
        {
            if (Context.EnableNamingAlias)
            {
                typeName = Customization.CustomizeNaming(typeName);
            }

            // PropertyName in VB is case-insensitive.
            var isLanguageCaseSensitive = true;

            // In VB, it is allowed that a type has a property whose name is same with the type's name
            var allowPropertyNameSameWithTypeName = false;

            Func<string, string> customizePropertyName = name => { return Context.EnableNamingAlias ? Customization.CustomizeNaming(name) : name; };

            var propertyGroups = structuredType.Properties()
                .GroupBy(p => isLanguageCaseSensitive ? customizePropertyName(p.Name) : customizePropertyName(p.Name).ToUpperInvariant());

            // If the group contains more than one property, or the property in the group has the same name with the type (only for C#), we need to rename the property
            var propertyToBeRenamedGroups = propertyGroups.Where(g => g.Count() > 1 || !allowPropertyNameSameWithTypeName && g.Key == typeName);

            var knownIdentifiers = propertyGroups.Select(g => customizePropertyName(g.First().Name)).ToList();
            if (!allowPropertyNameSameWithTypeName && !knownIdentifiers.Contains(typeName))
            {
                knownIdentifiers.Add(typeName);
            }
            var uniqueIdentifierService =
                new UniqueIdentifierService(knownIdentifiers, isLanguageCaseSensitive);

            IdentifierMappings.Clear();
            foreach (var g in propertyToBeRenamedGroups)
            {
                var hasPropertyNameSameWithCustomizedPropertyName = false;
                var itemCount = g.Count();
                for (var i = 0; i < itemCount; i++)
                {
                    var property = g.ElementAt(i);
                    var customizedPropertyName = customizePropertyName(property.Name);

                    if (Context.EnableNamingAlias && customizedPropertyName == property.Name)
                    {
                        hasPropertyNameSameWithCustomizedPropertyName = true;
                    }

                    if (isLanguageCaseSensitive)
                    {
                        // If a property name is same as its customized property name, then we don't rename it.
                        // Or we don't rename the last property in the group
                        if (customizedPropertyName != typeName
                            && (customizedPropertyName == property.Name
                                || (!hasPropertyNameSameWithCustomizedPropertyName && i == itemCount - 1)))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        // When EnableNamingAlias = true, If a property name is same as its customized property name, then we don't rename it.
                        // Or we don't rename the last property in the group.
                        if ((Context.EnableNamingAlias && customizedPropertyName == property.Name)
                            || (!hasPropertyNameSameWithCustomizedPropertyName && i == itemCount - 1))
                        {
                            continue;
                        }
                    }
                    var renamedPropertyName = uniqueIdentifierService.GetUniqueIdentifier(customizedPropertyName);
                    IdentifierMappings.Add(property.Name, renamedPropertyName);
                }
            }
        }

        internal void WriteTypeStaticCreateMethod(string typeName, IEdmStructuredType structuredType)
        {
            Debug.Assert(structuredType != null, "structuredType != null");
            if (structuredType.IsAbstract)
            {
                return;
            }

            Func<IEdmProperty, bool> hasDefault = p => p.PropertyKind == EdmPropertyKind.Structural && ((IEdmStructuralProperty)p).DefaultValueString != null;

            if (Context.EnableNamingAlias)
            {
                typeName = Customization.CustomizeNaming(typeName);
            }

            var parameters = structuredType.Properties()
                .Where(p => !p.Type.IsNullable && !p.Type.IsCollection() && !hasDefault(p));
            if (!parameters.Any())
            {
                return;
            }

            WriteSummaryCommentForStaticCreateMethod(typeName);

            var uniqueIdentifierService = new UniqueIdentifierService( /*IsLanguageCaseSensitive*/true);
            var instanceName = GetFixedName(uniqueIdentifierService.GetUniqueParameterName(typeName));
            var propertyToParameterNamePairs = parameters
                .Select(p =>
                    new KeyValuePair<IEdmProperty, string>(p,
                        uniqueIdentifierService.GetUniqueParameterName(
                            IdentifierMappings.ContainsKey(p.Name) ? IdentifierMappings[p.Name] : p.Name)))
                .ToArray();

            foreach (var propertyToParameterNamePair in propertyToParameterNamePairs)
            {
                var propertyName = propertyToParameterNamePair.Key.Name;
                propertyName = IdentifierMappings.ContainsKey(propertyName) ?
                    IdentifierMappings[propertyName] : (Context.EnableNamingAlias ? Customization.CustomizeNaming(propertyName) : propertyName);
                WriteParameterCommentForStaticCreateMethod(propertyToParameterNamePair.Value, propertyName);
            }

            propertyToParameterNamePairs = propertyToParameterNamePairs
                .Select(p => p = new KeyValuePair<IEdmProperty, string>(p.Key, GetFixedName(p.Value)))
                .ToArray();

            WriteDeclarationStartForStaticCreateMethod(typeName, GetFixedName(typeName));
            WriteStaticCreateMethodParameters(propertyToParameterNamePairs);
            WriteDeclarationEndForStaticCreateMethod(GetFixedName(typeName), instanceName);

            foreach (var propertyToParameterNamePair in propertyToParameterNamePairs)
            {
                var property = propertyToParameterNamePair.Key;
                var parameterName = propertyToParameterNamePair.Value;

                Debug.Assert(!property.Type.IsCollection(), "!property.Type.IsCollection()");
                Debug.Assert(!property.Type.IsNullable, "!property.Type.IsNullable");

                // The static create method only sets non-nullable properties. We should add the null check if the type of the property is not a clr ValueType.
                // For now we add the null check if the property type is non-primitive. We should add the null check for non-ValueType primitives in the future.
                if (!property.Type.IsPrimitive() && !property.Type.IsEnum())
                {
                    WriteParameterNullCheckForStaticCreateMethod(parameterName);
                }

                var uniqIdentifier = IdentifierMappings.ContainsKey(property.Name) ?
                    IdentifierMappings[property.Name] : (Context.EnableNamingAlias ? Customization.CustomizeNaming(property.Name) : property.Name);
                WritePropertyValueAssignmentForStaticCreateMethod(instanceName,
                    GetFixedName(uniqIdentifier),
                    parameterName);
            }

            WriteMethodEndForStaticCreateMethod(instanceName);
        }

        internal void WriteStaticCreateMethodParameters(KeyValuePair<IEdmProperty, string>[] propertyToParameterPairs)
        {
            if (propertyToParameterPairs.Length == 0)
            {
                return;
            }

            // If the number of parameters are greater than 5, we put them in separate lines.
            var parameterSeparator = propertyToParameterPairs.Length > 5 ? ParameterSeparator : ", ";
            for (var idx = 0; idx < propertyToParameterPairs.Length; idx++)
            {
                var propertyToParameterPair = propertyToParameterPairs[idx];

                var parameterType = Utils.GetClrTypeName(propertyToParameterPair.Key.Type, Context.UseDataServiceCollection, this, Context);
                var parameterName = propertyToParameterPair.Value;
                if (idx == propertyToParameterPairs.Length - 1)
                {
                    // No separator after the last parameter.
                    parameterSeparator = string.Empty;
                }

                WriteParameterForStaticCreateMethod(parameterType, GetFixedName(parameterName), parameterSeparator);
            }
        }

        internal void WritePropertiesForStructuredType(IEnumerable<IEdmProperty> properties)
        {
            var useDataServiceCollection = Context.UseDataServiceCollection;

            var propertyInfos = properties.Select(property =>
            {
                var propertyName = IdentifierMappings.ContainsKey(property.Name) ?
                    IdentifierMappings[property.Name] : (Context.EnableNamingAlias ? Customization.CustomizeNaming(property.Name) : property.Name);

                return new
                {
                    PropertyType = Utils.GetClrTypeName(property.Type, useDataServiceCollection, this, Context),
                    PropertyVanillaName = property.Name,
                    PropertyName = propertyName,
                    FixedPropertyName = GetFixedName(propertyName),
                    PrivatePropertyName = "_" + propertyName,
                    PropertyInitializationValue = Utils.GetPropertyInitializationValue(property, useDataServiceCollection, this, Context)
                };
            }).ToList();

            // Private name should not confict with field name
            var uniqueIdentifierService = new UniqueIdentifierService(propertyInfos.Select(_ => _.FixedPropertyName), true);

            foreach (var propertyInfo in propertyInfos)
            {
                var privatePropertyName = uniqueIdentifierService.GetUniqueIdentifier("_" + propertyInfo.PropertyName);

                WritePropertyForStructuredType(
                    propertyInfo.PropertyType,
                    propertyInfo.PropertyVanillaName,
                    propertyInfo.PropertyName,
                    propertyInfo.FixedPropertyName,
                    privatePropertyName,
                    propertyInfo.PropertyInitializationValue,
                    useDataServiceCollection);
            }
        }

        internal void WriteMembersForEnumType(IEnumerable<IEdmEnumMember> members)
        {
            var n = members.Count();
            for (var idx = 0; idx < n; ++idx)
            {
                var member = members.ElementAt(idx);
                var value = string.Empty;
                if (member.Value != null)
                {
                    var integerValue = member.Value as IEdmIntegerValue;
                    if (integerValue != null)
                    {
                        value = " = " + integerValue.Value.ToString(CultureInfo.InvariantCulture);
                    }
                }

                var memberName = Context.EnableNamingAlias ? Customization.CustomizeNaming(member.Name) : member.Name;
                WriteMemberForEnumType(GetFixedName(memberName) + value, member.Name, idx == n - 1);
            }
        }

        internal string GetFixedName(string originalName)
        {
            var fixedName = originalName;

            if (LanguageKeywords.Contains(fixedName))
            {
                fixedName = string.Format(FixPattern, fixedName);
            }

            return fixedName;
        }

        internal string GetElementTypeName(IEdmEntityType elementType, IEdmEntityContainer container)
        {
            var elementTypeName = elementType.Name;

            if (Context.EnableNamingAlias)
            {
                elementTypeName = Customization.CustomizeNaming(elementTypeName);
            }

            if (elementType.Namespace != container.Namespace)
            {
                elementTypeName = Context.GetPrefixedFullName(elementType, GetFixedName(elementTypeName), this);
            }

            return elementTypeName;
        }
    }
}