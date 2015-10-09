using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace OData4.Builder
{
    /// <summary>
    /// Service making names within a scope unique. Initialize a new instance for every scope.
    /// </summary>
    public sealed class UniqueIdentifierService
    {
        // This is the list of keywords we check against when creating parameter names from propert. 
        // If a name matches this keyword we prefix it.
        private static readonly string[] Keywords = { "class", "event" };

        /// <summary>
        /// Hash set to detect identifier collision.
        /// </summary>
        private readonly HashSet<string> _knownIdentifiers;

        /// <summary>
        /// Constructs a <see cref="UniqueIdentifierService"/>.
        /// </summary>
        /// <param name="caseSensitive">true if the language we are generating the code for is case sensitive, false otherwise.</param>
        internal UniqueIdentifierService(bool caseSensitive)
        {
            _knownIdentifiers = new HashSet<string>(caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Constructs a <see cref="UniqueIdentifierService"/>.
        /// </summary>
        /// <param name="identifiers">identifiers used to detect collision.</param>
        /// <param name="caseSensitive">true if the language we are generating the code for is case sensitive, false otherwise.</param>
        internal UniqueIdentifierService(IEnumerable<string> identifiers, bool caseSensitive)
        {
            _knownIdentifiers = new HashSet<string>(identifiers ?? Enumerable.Empty<string>(), caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Given an identifier, makes it unique within the scope by adding
        /// a suffix (1, 2, 3, ...), and returns the adjusted identifier.
        /// </summary>
        /// <param name="identifier">Identifier. Must not be null or empty.</param>
        /// <returns>Identifier adjusted to be unique within the scope.</returns>
        internal string GetUniqueIdentifier(string identifier)
        {
            Debug.Assert(!string.IsNullOrEmpty(identifier), "identifier is null or empty");

            // find a unique name by adding suffix as necessary
            var numberOfConflicts = 0;
            var uniqueIdentifier = identifier;
            while (_knownIdentifiers.Contains(uniqueIdentifier))
            {
                ++numberOfConflicts;
                uniqueIdentifier = identifier + numberOfConflicts.ToString(CultureInfo.InvariantCulture);
            }

            // remember the identifier in this scope
            Debug.Assert(!_knownIdentifiers.Contains(uniqueIdentifier), "we just made it unique");
            _knownIdentifiers.Add(uniqueIdentifier);

            return uniqueIdentifier;
        }

        /// <summary>
        /// Fix up the given parameter name and make it unique.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <returns>Fixed parameter name.</returns>
        internal string GetUniqueParameterName(string name)
        {
            name = Utils.CamelCase(name);

            // FxCop consider 'iD' as violation, we will change any property that is 'id'(case insensitive) to 'ID'
            if (StringComparer.OrdinalIgnoreCase.Equals(name, "id"))
            {
                name = "ID";
            }

            return GetUniqueIdentifier(name);
        }
    }
}