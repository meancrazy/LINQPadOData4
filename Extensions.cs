using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using LINQPad.Extensibility.DataContext;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;

namespace OData4.LINQPadDriver
{
	internal static class Extensions
	{
		public static ConnectionProperties GetConnectionProperties(this IConnectionInfo connectionInfo)
		{
			return new ConnectionProperties(connectionInfo);
		}

		public static IEdmModel GetModel(this ConnectionProperties properties)
		{
			var uri = properties.Uri;
			uri += uri.EndsWith("/") ? "$metadata" : "/$metadata";

			var settings = new XmlReaderSettings
			{
				XmlResolver = new XmlUrlResolver
				{
					Credentials = properties.GetCredentials(),
					Proxy = properties.GetWebProxy()
				}
			};

			using var reader = XmlReader.Create(uri, settings);

			var model = CsdlReader.Parse(reader);

			return model;
		}

		public static List<ExplorerItem> GetSchema(this IEdmModel model)
		{
			var schema = new List<ExplorerItem>();

			var sets = model.EntityContainer
							.EntitySets()
							.OrderBy(o => o.Name)
							.Select(o => new ExplorerItem(o.Name, ExplorerItemKind.QueryableObject, ExplorerIcon.Table)
							{
								IsEnumerable = true,
								ToolTipText = o.EntityType().Name,
								DragText = o.Name,

								Tag = o.EntityType()
							})
							.ToList();

			foreach (var set in sets)
			{
				var parentType = (IEdmEntityType)set.Tag;
				var children = new List<ExplorerItem>();

				for (; parentType != null; parentType = parentType.BaseEntityType())
				{
					children.AddRange(parentType.DeclaredStructuralProperties().Select(o => GetStructuralChildItem(parentType, o)));
					children.AddRange(parentType.DeclaredNavigationProperties().Select(o => GetNavigationChildItem(o, sets)));
				}

				set.Children = children;
			}

			schema.AddRange(sets);

			var actions = model.SchemaElements
							   .OfType<IEdmAction>()
							   .Select(o => new ExplorerItem(o.Name, ExplorerItemKind.QueryableObject, ExplorerIcon.ScalarFunction));

			schema.AddRange(actions);

			return schema;
		}

		private static ExplorerItem GetStructuralChildItem(IEdmEntityType parentType, IEdmStructuralProperty property)
		{
			var icon = parentType.HasDeclaredKeyProperty(property)
													? ExplorerIcon.Key
													: ExplorerIcon.Column;

			var name = $"{property.Name} ({property.Type.GetTypeName()})";
			var item = new ExplorerItem(name, ExplorerItemKind.Property, icon)
			{
				DragText = property.Name
			};

			return item;
		}

		private static ExplorerItem GetNavigationChildItem(IEdmNavigationProperty property, List<ExplorerItem> schema)
		{
			var partnerType = property.ToEntityType();

			var backReferenceType = partnerType.DeclaredNavigationProperties()
											   .FirstOrDefault(o => o.ToEntityType() == property.DeclaringEntityType());

			var isCollection = property.Type.IsCollection();
			var kind = isCollection ? ExplorerItemKind.CollectionLink : ExplorerItemKind.ReferenceLink;
			ExplorerIcon icon;

			if (backReferenceType == null)
				icon = ExplorerIcon.Column;
			else if (isCollection)
				icon = backReferenceType.Type.IsCollection() ? ExplorerIcon.ManyToMany : ExplorerIcon.OneToMany;
			else
				icon = backReferenceType.Type.IsCollection() ? ExplorerIcon.ManyToOne : ExplorerIcon.OneToOne;

			var item = new ExplorerItem(property.Name, kind, icon)
			{
				ToolTipText = partnerType.Name,
				HyperlinkTarget = schema.FirstOrDefault(a => (IEdmEntityType)a.Tag == partnerType),
				DragText = property.Name
			};

			return item;
		}

		private static string GetTypeName(this IEdmTypeReference type)
		{
			if (!(type.Definition is IEdmCollectionType edmCollectionType))
			{
				return (type.Definition as IEdmNamedElement)?.Name;
			}

			return !(edmCollectionType.ElementType.Definition is IEdmNamedElement element1) ? null : $"Collection({element1.Name})";
		}

		public static X509Certificate GetClientCertificate(this ConnectionProperties properties)
		{
			if (string.IsNullOrWhiteSpace(properties.ClientCertificateFile)) return null;

			X509Certificate cert;
			try
			{
				cert = X509Certificate.CreateFromCertFile(properties.ClientCertificateFile);
			}
			catch (Exception)
			{
				// todo: we can't load certificate file
				cert = null;
			}

			return cert;
		}
	}
}
